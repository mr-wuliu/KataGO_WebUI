using System.Diagnostics;
using KatagoDtos;
using Newtonsoft.Json;
using Serilog;
using WuliuGO.Models;

namespace WuliuGO.Services
{
    public class KatagoServer
    {
        private readonly IServiceProvider _serviceProvider;
        private const string KatagoPath = "KatagoAI/ai/katago.exe";
        private const string ConfigPath = "KatagoAI/ai/cfg/analysis_example.cfg";
        private const string ModelPath = "KatagoAI/ai/model/kata1-b28c512nbt-s7332806912-d4357057652.bin.gz";
        private Process? _katagoProcess;
        public KatagoServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private async Task<string> InsertQuery(KatagoQuery katagoQuery)
        {
            // 长生命周期服务请求短生命周期服务需要使用scope获取服务
            using var scope = _serviceProvider.CreateScope();
            var katagoRepository = scope.ServiceProvider.GetRequiredService<IKatagoRepository>();

            await katagoRepository.AddKatagoQueryAsync(katagoQuery);
            katagoQuery.QueryId = "go_" + katagoQuery.Id;
            _ = katagoRepository.UpdateKatagoQueryAsync(katagoQuery);

            return katagoQuery.QueryId;
        }
        public string StartKatago()
        {
            if (_katagoProcess != null && !_katagoProcess.HasExited)
            {
                return "already started and exited";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = KatagoPath,
                Arguments = $"analysis -config {ConfigPath} -model {ModelPath}",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _katagoProcess = new Process()
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            _katagoProcess.OutputDataReceived += (sender, args) => OnKatagoOutput(args.Data);
            _katagoProcess.ErrorDataReceived += (sender, args) => OnKatagoError(args.Data);
            _katagoProcess.Start();
            _katagoProcess.BeginOutputReadLine();
            _katagoProcess.BeginErrorReadLine();

            return "start KataGo process.";

        }
        public void OnKatagoOutput(string? data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var katagoRepository = scope.ServiceProvider.GetRequiredService<IKatagoRepository>();
                    var result = JsonConvert.DeserializeObject<KatagoOutput>(data);

                    if (result != null)
                    {
                        string queryId = result.Id;
                        var katagoQuery = katagoRepository.GetKatagoQueryByQueryIdAsync(queryId).Result;
                        if (katagoQuery != null)
                        {
                            // update database
                            katagoQuery.IsDuringSearch = result.IsDuringSearch;
                            katagoQuery.MoveInfos = JsonConvert.SerializeObject(result.MoveInfos);
                            katagoQuery.RootInfo = JsonConvert.SerializeObject(result.RootInfo);
                            katagoQuery.TurnNumber = result.TurnNumber;

                            katagoRepository.UpdateKatagoQueryAsync(katagoQuery).Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"[KataGo Error]: {ex.Message}");
                }
            }
        }
        public void OnKatagoError(string? data)
        {
            if (!string.IsNullOrEmpty(data))
                Log.Information($"[KataGo INFO]: {data}");
        }
        public bool StopKatago()
        {
            if (_katagoProcess != null && !_katagoProcess.HasExited)
            {
                _katagoProcess.Kill();
                _katagoProcess.WaitForExit(5000);
                _katagoProcess.Dispose();
                _katagoProcess = null;
                return true;
            }
            return false;
        }
        public bool GetStatus()
        {
            return _katagoProcess != null && !_katagoProcess.HasExited;
        }

        public async Task<string> AnaylyzeBoardAsync(QueryDto dto)
        {


            if (_katagoProcess == null || _katagoProcess.HasExited)
            {
                return "Katago is not running.";
            }
            // check params;
            if (dto.moves.Count == 0)
            {
                return "Invalid query params";
            }
            // 创建数据库记录
            var katagoQuery = new KatagoQuery
            {
                IsDuringSearch = true,
            };
            string queryId = await InsertQuery(
                new KatagoQuery
                {
                    IsDuringSearch = true,
                }
            );

            var query = new
            {
                id = queryId,
                moves = dto.moves,
                initialStones = Array.Empty<object>(),
                rules = "Chinese",
                komi = 6.5,
                boardXSize = 19,
                boardYSize = 19,
                includePolicy = true,
                maxVisits = 100,
            };
            string jsonQuery = JsonConvert.SerializeObject(query);
            _katagoProcess.StandardInput.WriteLine(jsonQuery);
            _katagoProcess.StandardInput.Flush();
            return queryId;
        }
        public async Task<KatagoQueryRest?> GetQueryByQueryId(string queryId)
        {
            using var scope = _serviceProvider.CreateScope();
            var katagoRepository = scope.ServiceProvider.GetRequiredService<IKatagoRepository>();

            var result = await katagoRepository.GetKatagoQueryByQueryIdAsync(queryId);

            if (result == null)
            {
                return null;
            }

            var katagoRest = new KatagoQueryRest
            {
                Id = result.QueryId
            };

            if (result.MoveInfos != null)
            {
                katagoRest.Moves = JsonConvert.DeserializeObject<List<MoveInfo>>(result.MoveInfos)?.Take(4).ToList();

            }
            if (result.RootInfo != null)
            {
                katagoRest.RootInfo = JsonConvert.DeserializeObject<RootInfo>(result.RootInfo);
            }
            return katagoRest;
        }
        public async Task<string> GetKatagoInfoAsync()
        {
            if (_katagoProcess == null || _katagoProcess.HasExited)
            {
                return "Katago is not running.";
            }
            var query = new
            {
                id = "info",
                action = "query_models",
            };
            _katagoProcess.StandardInput.WriteLine(JsonConvert.SerializeObject(query));
            _katagoProcess.StandardInput.Flush();
            string? result = await _katagoProcess.StandardOutput.ReadLineAsync();

            return result ?? "No response from KataGo";

        }
    }
}

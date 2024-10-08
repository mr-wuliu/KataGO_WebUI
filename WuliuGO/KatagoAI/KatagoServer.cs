using System.Diagnostics;
using KatagoDtos;
using Newtonsoft.Json;
using Serilog;
using WuliuGO.Models;

namespace WuliuGO.Services
{
    public class KatagoServer : IHostedService, IKatagoServer
    {
        private static bool _isInitialized;
        private readonly IServiceProvider _serviceProvider;
        private const string KatagoPath = "KatagoAI/ai/katago.exe";
        private const string ConfigPath = "KatagoAI/ai/cfg/analysis_example.cfg";
        private const string ModelPath = "KatagoAI/ai/model/kata1-b28c512nbt-s7332806912-d4357057652.bin.gz";
        private Process? _katagoProcess;

        public KatagoServer(IServiceProvider serviceProvider)
        {
            if (!_isInitialized) {
                Log.Information("Initializing KatagoService...");
                _isInitialized = true;
            } else {
                throw new Exception("KatagoService has already been initialized.");
            }
            _serviceProvider = serviceProvider;
            _isInitialized = true;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Starting KatagoService...");
            StartKatago();
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Stopping KatagoService...");
            StopKatago();
            return Task.CompletedTask;
        }

        private async Task<string> InsertQuery(Analysis katagoQuery)
        {
            // 长生命周期服务请求短生命周期服务需要使用scope获取服务
            using var scope = _serviceProvider.CreateScope();
            var katagoRepository = scope.ServiceProvider.GetRequiredService<IKatagoRepository>();

            await katagoRepository.AddKatagoQueryAsync(katagoQuery);
            katagoQuery.QueryId = "go_" + katagoQuery.Id;
            await katagoRepository.UpdateKatagoQueryAsync(katagoQuery);

            return katagoQuery.QueryId;
        }
        private string StartKatago()
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
        private bool StopKatago()
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
                            katagoQuery.IsRunning = result.IsDuringSearch;
                            katagoQuery.OutputMove = JsonConvert.SerializeObject(result.MoveInfos);
                            katagoQuery.RootInfo = JsonConvert.SerializeObject(result.RootInfo);
                            katagoQuery.Policy = JsonConvert.SerializeObject(result.Policy);

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

        public bool GetStatus()
        {
            Log.Information("Get Status");
            if (_katagoProcess != null)
            {
                Log.Information($"[KataGo Process]: {_katagoProcess.ProcessName} - {_katagoProcess.Id}");
            }
            return _katagoProcess != null && !_katagoProcess.HasExited;
        }

        public async Task<string> AnaylyzeBoardAsync(QueryDto dto)
        {
            Log.Information("Analyze Board");
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
            var queryInfo = new 
            {
                rule = "Chinese",
                komi = "6.5",
                boardXSize = 19,
                boardYSize = 19,
                includePolicy = true,
                maxVisits = 100,
            };
            string queryId = await InsertQuery(
                new Analysis
                {
                    IsRunning = true,
                    CreateTime = DateTime.UtcNow,
                    InputMove = JsonConvert.SerializeObject(dto.moves),
                    AnalysisInfo = JsonConvert.SerializeObject(queryInfo),
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

            if (result.OutputMove != null)
            {
                katagoRest.Moves = JsonConvert.DeserializeObject<List<MoveInfo>>(result.OutputMove)?.Take(4).ToList();

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
        public async Task<List<double>?> GetKatagoPolicy(string queryId)
        {
            using var scope = _serviceProvider.CreateScope();
            var katagoRepository = scope.ServiceProvider.GetRequiredService<IKatagoRepository>();

            var result = await katagoRepository.GetKatagoPolicyByQueryIdAsync(queryId);

            if (result == null )
            {
                return null;
            }

            return result;
        }
    }
}

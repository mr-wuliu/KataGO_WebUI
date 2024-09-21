using System.Diagnostics;
using System.Text;

public class KatagoServer
{
    private const string KatagoPath = "path/to/katago";
    private const string ConfigPath = "path/to/katago/config.cfg";
    private const string ModelPath = "path/to/katago/model.bin";

    public async Task<string> GetKatagoAnalysis(string boardState)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = KatagoPath,
            Arguments = $"analysis -config {ConfigPath} -model {ModelPath}",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        await process.StandardInput.WriteLineAsync(boardState);
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();

        var output = new StringBuilder();
        while (!process.StandardOutput.EndOfStream)
        {
            output.AppendLine(await process.StandardOutput.ReadLineAsync());
        }

        await process.WaitForExitAsync();

        return output.ToString();
    }
}

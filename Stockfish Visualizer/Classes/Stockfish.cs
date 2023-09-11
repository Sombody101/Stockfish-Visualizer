using Spectre.Console;
using Stockfish_Visualizer.Classes;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Stockfish_Visualizer;

public static class Stockfish
{
    private static Process? stockProc;

    // API: https://github.com/official-stockfish/Stockfish/wiki/UCI-&-Commands
    public static async Task LaunchAsync()
    {
        if (!Directory.Exists(Configuration.Temp))
        {
            _ = Directory.CreateDirectory(Configuration.Temp);
        }

        if (!File.Exists(Configuration.StockfishExe))
        {
            AnsiConsole.Progress()
               .AutoRefresh(false)
               .AutoClear(false)
               .HideCompleted(false)
               .Columns(new ProgressColumn[]
               {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn(Spinner.Known.Dots),
               })
               .Start(ctx =>
               {
                   var task = ctx.AddTask("[green]Extracting stockfish executable[/]");

                   using Stream resourceStream = Program.GetStockfishSteam();
                   using FileStream fileStream = new(Configuration.StockfishExe, FileMode.Create);

                   const int bufferSize = 65536;
                   byte[] buffer = new byte[bufferSize];
                   int bytesRead;
                   long totalRead = 0;

                   while ((bytesRead = resourceStream.Read(buffer, 0, buffer.Length)) > 0)
                   {
                       fileStream.Write(buffer, 0, bytesRead);
                       totalRead += bufferSize;

                       task.Increment(0.14);
                       task.Description = $"[green]Extracting stockfish executable[/] [[{totalRead / 1024 / 1024:N0}/46MB]]";
                       ctx.Refresh();
                   }
               });
        }

        stockProc = new Process();
        stockProc.StartInfo.FileName = Configuration.StockfishExe;
        stockProc.StartInfo.UseShellExecute = false;
        stockProc.StartInfo.RedirectStandardInput = true;
        stockProc.StartInfo.RedirectStandardOutput = true;
        stockProc.StartInfo.RedirectStandardError = true;
        stockProc.StartInfo.CreateNoWindow = true;

        stockProc.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Debugging.WriteLine("[green][[STOCKFISH]][/]\t " + e.Data.EscapeMarkup());
            }
        };

        stockProc.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                GameBoard.Window.AppendLog("[red]Stockfish error: [/]" + e.Data);
                Debugging.WriteLine("[red][[STOCKFISH]][/]\t " + e.Data.EscapeMarkup());
            }
        };

        stockProc.Exited += (sender, e) =>
        {
            Debugging.WriteLine("[red]Stockfish has exited[/]");
            GameBoard.Window.AppendLog("[red]Stockfish exit: [/]" + "Test string");
        };

        _ = stockProc.Start();

        stockProc.BeginOutputReadLine();
        stockProc.BeginErrorReadLine();

        await Task.Delay(500);

        SendCommandAsync("uci");

        await Task.Delay(200);

        LogMemoryUsage();
    }

    public static async Task<bool> IsReady()
    {
        SendCommandAsync("isready");
        var output = GetOutputAsync();

        return output.Result == "readyok";
    }

    public static async Task CloseAsync()
    {
        // Close the Stockfish process if it is running
        if (stockProc is not null && !stockProc.HasExited)
        {
            stockProc.Close();
            stockProc = null;
        }

        Configuration.ClearTempFiles();
    }

    public static async Task SendCommandAsync(string command)
    {
        if (stockProc is null || stockProc.HasExited)
            return;

        Debugging.WriteLine("[blue][[SENDING]][/]\t " + command);
        await stockProc.StandardInput.WriteLineAsync(command);
        await stockProc.StandardInput.FlushAsync();
    }

    public static async Task<string> GetOutputAsync()
    {
        if (stockProc is null || stockProc.HasExited)
            return string.Empty;

        return await stockProc.StandardOutput.ReadToEndAsync();
    }

    public static async Task<string> GetErrorAsync()
    {
        if (stockProc is null || stockProc.HasExited)
            return string.Empty;

        return await stockProc.StandardError.ReadToEndAsync();
    }

    public static async Task EndAsync()
    {
        if (stockProc is null || stockProc.HasExited)
            return;

        _ = stockProc.CloseMainWindow();
        stockProc.WaitForExit();
        stockProc.Dispose();
    }

    public static async Task RestartAsync()
    {
        await EndAsync();
        await LaunchAsync();
    }

    public static void LogMemoryUsage()
    {
        if (stockProc is null)
            return;

        var privateMemory = Process.GetCurrentProcess().PrivateMemorySize64;
        var stockfishMem = stockProc.PrivateMemorySize64;
        Debugging.WriteLine("Total process memory:\t" + (privateMemory / 1024 / 1024) + $"MB  ({privateMemory:N0} bytes)");
        Debugging.WriteLine("Total stockfish memory:\t" + (stockfishMem / 1024 / 1024) + $"MB ({stockfishMem:N0} bytes)");
        Debugging.WriteLine("Total memory:\t\t" + ((stockfishMem / 1024 / 1024) + (privateMemory / 1024 / 1024)) + $"MB ({(stockfishMem + privateMemory):N0} bytes)");
    }
}

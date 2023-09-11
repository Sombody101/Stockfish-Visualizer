using System.Reflection;
using System.Runtime.InteropServices;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Stockfish_Visualizer.Classes;

public static class Debugging
{
    [DllImport("kernel32")]
    private extern static bool AllocConsole();

    private static bool consoleCreated = false;
    private static List<object> buffer = new();
    private const int MaxBufferSize = 4096;

    public static void CreateConsole()
    {
        if (consoleCreated)
            return;

        if (!AllocConsole())
            throw new Exception("Failed to create console window");
        consoleCreated = true;

        WriteLine("[yellow][[WARNING]][/] Closing this window will also close the app!\r\n");

        if (buffer.Count is not 0)
        {
            WriteTitle(new Rule("CLEARING OUTPUT BUFFER").LeftJustified().RuleStyle(Style.Parse("green")));
            ClearBuffer();
            WriteTitle(new Rule("OUTPUT BUFFER CLEARED").LeftJustified().RuleStyle(Style.Parse("green")));
        }
    }

    public static void WriteLine(string line = "")
    {
        if (consoleCreated)
            AnsiConsole.MarkupLine(line);
        else
            AppendToBuffer(line + "\r\n");
    }

    public static void Write(string text = "")
    {
        if (consoleCreated)
            AnsiConsole.Write(text);
        else
            AppendToBuffer(text, true);
    }

    public static void WriteData(string header, params string[] data)
    {
        string text = $"{header}:\r\n";

        foreach (string d in data)
            text += "\t" + d + "\r\n";

        WriteLine(text);
    }

    public static void Log(string log)
        => WriteLine("[blue][[!]][/]\t" + log);

    public static void LogWarning(string log) 
        => WriteLine("[yellow][[!]][/]\t" + log);

    public static void LogError(string log)
        => WriteLine("[red][[!]][/]\t" + log);

    public static void WriteTitle(string title)
    {
        WriteTitle(new Rule(title).LeftJustified());
    }

    public static void WriteTitle(Rule rule)
    {
        if (consoleCreated)
        {
            AnsiConsole.Write(rule);
            Console.Write("\r\n");
        }
        else
        {
            AppendToBuffer(rule);
            AppendToBuffer("\r\n");
        }
    }

    public static void ClearBuffer()
    {
        foreach (object obj in buffer)
        {
            if (obj is string)
                AnsiConsole.Markup((string)obj);
            else if (obj is IRenderable)
                AnsiConsole.Write((IRenderable)obj);
            else
                Console.Write(obj);
        }
    }

    private static void AppendToBuffer(object line, bool appendToLast = false)
    {
        if (appendToLast)
            buffer[^1] += (string)line;
        else
        {
            if (buffer.Count >= MaxBufferSize)
            {
                buffer.Clear();
                buffer.Add("[yellow][[WARNING][/] Buffer reset due to line count being over " + MaxBufferSize);
            }

            buffer.Add(line);
        }
    }
}
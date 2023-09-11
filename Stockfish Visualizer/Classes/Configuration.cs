using Stockfish_Visualizer.Settings;

namespace Stockfish_Visualizer.Classes;

public static class Configuration
{
    // Paths
    public static readonly string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
    public static readonly string Assets = AppDirectory + "\\assets";

    public static readonly string SettingsFiles = Assets + "\\settings";
    public static readonly string StockfishSettingsJson = SettingsFiles + "\\stockfish-config.json";
    public static readonly string VisualizerSettingsJson = SettingsFiles + "\\visualizer-config.json";

    public static readonly string Temp = Assets + "\\temp";
    public static readonly string StockfishExe = Temp + "\\stockfish.exe";

    // Application settings
    public static VisualizerSettings VisualizerSettings { get; set; }
    public static StockfishSettings StockfishSettings { get; set; }

    static Configuration()
    {
        if (!Directory.Exists(Assets))
            Directory.CreateDirectory(Assets);

        if (!Directory.Exists(SettingsFiles))
            Directory.CreateDirectory(SettingsFiles);

        if (!Directory.Exists(Temp))
            Directory.CreateDirectory(Temp);

        VisualizerSettings = SettingsLoader.LoadVisualizerSettings();
        StockfishSettings = SettingsLoader.LoadStockfishSettings();
    }

    public static void ClearTempFiles()
    {
        if (Directory.Exists(Temp))
            Directory.Delete(Temp, true);
    }
}
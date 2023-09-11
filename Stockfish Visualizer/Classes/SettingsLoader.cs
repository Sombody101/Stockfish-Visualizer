using Newtonsoft.Json;
using Stockfish_Visualizer.Classes;

namespace Stockfish_Visualizer.Settings;

public class VisualizerSettings
{
    public Size WindowSize { get; set; } = new(1300, 620);
}

public class StockfishSettings
{
    public string placeholder = "lol";
}

public static class SettingsLoader
{
    public static void SetVisualizerSettings(this VisualizerSettings settings)
    {
        File.WriteAllText(Configuration.VisualizerSettingsJson, JsonConvert.SerializeObject(settings));
    }

    public static VisualizerSettings LoadVisualizerSettings(bool forceNew = false)
    {
        if (!File.Exists(Configuration.VisualizerSettingsJson) || forceNew)
        {
            Debugging.Log("Failed to find visualizer configuration file | Creating new one with default values");
            var newVisualizerSettings = new VisualizerSettings();
            File.WriteAllText(Configuration.VisualizerSettingsJson, JsonConvert.SerializeObject(newVisualizerSettings));
            return newVisualizerSettings;
        }

        return JsonConvert.DeserializeObject<VisualizerSettings>(File.ReadAllText(Configuration.VisualizerSettingsJson)) ?? LoadVisualizerSettings(true);
    }

    public static void SetStockfishSettings(this StockfishSettings settings)
    {
        File.WriteAllText(Configuration.StockfishSettingsJson, JsonConvert.SerializeObject(settings));
    }

    public static StockfishSettings LoadStockfishSettings(bool forceNew = false)
    {
        if (!File.Exists(Configuration.StockfishSettingsJson) || forceNew)
        {
            Debugging.Log("Failed to find stockfish configuration file | Creating new one with default values");
            var newStockfishSettings = new StockfishSettings();
            File.WriteAllText(Configuration.StockfishSettingsJson, JsonConvert.SerializeObject(newStockfishSettings));
            return newStockfishSettings;
        }

        return JsonConvert.DeserializeObject<StockfishSettings>(File.ReadAllText(Configuration.StockfishSettingsJson)) ?? LoadStockfishSettings(true);
    }
}
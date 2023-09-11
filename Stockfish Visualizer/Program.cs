using System.Reflection;

namespace Stockfish_Visualizer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ApplicationExit += (sender, e) =>
            {
                // cleanup
            };

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Visualizer());
        }

        // Handle exceptions on the UI thread
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            VisualizerException.CreateException(e.Exception);
        }

        // Handle exceptions on non-UI threads
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            VisualizerException.CreateException((Exception)e.ExceptionObject);
        }

        /// <summary>
        /// Get a steam of the stockfish binary to prevent a massive spike in memory usage
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Stream GetStockfishSteam()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("Stockfish_Visualizer.Resources.stockfish.exe") ?? throw new Exception("Failed to find stockfish in resources");
        }
    }
}
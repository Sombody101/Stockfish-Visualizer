using Timer = System.Timers.Timer;

namespace Stockfish_Visualizer.Classes;

public class ResizeTimer
{
    private Timer timer;
    private int initialInterval;
    private bool isRunning = false;
    private SynchronizationContext syncContext;

    public event EventHandler OnTimerEnd;

    public ResizeTimer(int initialMSeconds)
    {
        syncContext = SynchronizationContext.Current!;
        initialInterval = initialMSeconds;
        timer = new Timer(initialInterval);
        timer.Elapsed += (sender, e) =>
        {
            timer.Stop();

            syncContext.Send(state =>
            {
                OnTimerEnd?.Invoke(this, EventArgs.Empty);
            }, null);

        };
    }

    public bool IsRunning()
        => isRunning;

    public void Dispose()
        => timer.Dispose();

    public void Start()
    {
        timer.Stop();
        timer.Interval = initialInterval;
        timer.Start();
        isRunning = true;
    }

    public void Reset()
    {
        timer.Stop();
        timer.Interval = initialInterval;
        timer.Start();
    }
}


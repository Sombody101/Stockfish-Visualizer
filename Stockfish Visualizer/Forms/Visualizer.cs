using Stockfish_Visualizer.Classes;
using Stockfish_Visualizer.Forms;
using System.Runtime.InteropServices;

namespace Stockfish_Visualizer;

public partial class Visualizer : Form
{
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ClassStyle |= 0x20000; // CS_DROPSHADOW
            return cp;
        }
    }

    private bool windowLoaded = false;
    public Visualizer()
    {
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

#if DEBUG
        Debugging.CreateConsole();
#endif

        InitializeComponent();

        //this.MouseDown += MoveForm;
        Titlebar.MouseDown += MoveForm;
        AppTitleLabel.MouseDown += MoveForm;

        Click += (sender, e) =>
        {
            GameBoard.CleanPanelColors();
        };

        MinimizeButton.Click += (sender, e) =>
        {
            WindowState = FormWindowState.Minimized;
        };

        ExitButton.Click += (sender, e) => Application.Exit();

        MinimumSize = new(500, 300);

        GameBoard.Initialize(this);
    }

    private void Visualizer_Load(object sender, EventArgs e)
    {
        Debugging.WriteLine();
        Debugging.WriteTitle("Stockfish Initialization");
        Debugging.WriteLine("Starting stockfish... (Async)");

        AppendLog("[green]Starting Stockfish...");
        Task.Run(async () =>
        {
            await Task.Delay(200); // Wait for window resizing (if any)
            await Stockfish.LaunchAsync();

            Debugging.Log(await Stockfish.IsReady() ? "Stockfish ready" : "Stockfish failed to load");
            await Stockfish.SendCommandAsync("setoption name Threads value " + Environment.ProcessorCount);
        });

        windowLoaded = true;

        UserPrompt.ShowDialog("this is the text field", "this is the caption", "param1", "param 2", "param3");
    }

    public void AppendLog(string log)
    {
        var logs = log.Split("[/]");

        foreach (var l in logs)
        {
            GamePresenter.SelectionStart = GamePresenter.TextLength;
            GamePresenter.SelectionLength = 0;

            int tagEnd = l.IndexOf(']');

            GamePresenter.SelectionColor = Color.FromName(l[1..tagEnd]);
            GamePresenter.AppendText(l[(tagEnd + 1)..]);
            GamePresenter.SelectionColor = GamePresenter.ForeColor;
        }
    }

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ReleaseCapture();

    private void MoveForm(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            _ = SendMessage(Handle, 0xA1, 0x2, 0);
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.Shift | Keys.D)) // Debug window (Not made yet)
        {
            return true;
        }
        else if (keyData == (Keys.Control | Keys.Shift | Keys.C)) // Debug console (Launches by default when in debug mode)
        {
            Debugging.CreateConsole();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private const int
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        bottom = 10;

    private Rectangle m_Top { get => new(0, 0, ClientSize.Width, bottom); }
    private Rectangle m_Left { get => new(0, 0, bottom, ClientSize.Height); }
    private Rectangle m_Bottom { get => new(0, ClientSize.Height - bottom, ClientSize.Width, bottom); }
    private Rectangle m_Right { get => new(ClientSize.Width - bottom, 0, bottom, ClientSize.Height); }
    private Rectangle m_TopLeft { get => new(0, 0, bottom, bottom); }
    private Rectangle m_TopRight { get => new(ClientSize.Width - bottom, 0, bottom, bottom); }
    private Rectangle m_BottomLeft { get => new(0, ClientSize.Height - bottom, bottom, bottom); }
    private Rectangle m_BottomRight { get => new(ClientSize.Width - bottom, ClientSize.Height - bottom, bottom, bottom); }

    /// <summary>
    /// Handles window resizing
    /// </summary>
    /// <param name="message"></param>
    protected override void WndProc(ref Message message)
    {
        base.WndProc(ref message);

        if (message.Msg == 0x84) // WM_NCHITTEST
        {
            var cursor = PointToClient(Cursor.Position);

            if (m_TopLeft.Contains(cursor))
                message.Result = HTTOPLEFT;
            else if (m_TopRight.Contains(cursor))
                message.Result = HTTOPRIGHT;
            else if (m_BottomLeft.Contains(cursor))
                message.Result = HTBOTTOMLEFT;
            else if (m_BottomRight.Contains(cursor))
                message.Result = HTBOTTOMRIGHT;

            else if (m_Top.Contains(cursor))
                message.Result = HTTOP;
            else if (m_Left.Contains(cursor))
                message.Result = HTLEFT;
            else if (m_Right.Contains(cursor))
                message.Result = HTRIGHT;
            else if (m_Bottom.Contains(cursor))
                message.Result = HTBOTTOM;
        }
    }

    private ResizeTimer? resizeTimer;
    protected override void OnResize(EventArgs e)
    {
        // Remove curved window while resizing to prevent expansion being hidden
        Region = new();
        base.OnResize(e);

        if (resizeTimer is not null)
            resizeTimer.Reset();
        else
        {
            resizeTimer = new(windowLoaded ? 150 : 15);
            Debugging.WriteLine("Resize timer started");
            resizeTimer.OnTimerEnd += (sender, e) =>
            {
                GameBoard.RedrawBoard();

                Debugging.WriteData($"Visualizer resized",
                    $"PanelSize:\t{GameBoard.PanelSize}px",
                    $"Width:\t\t{Width}px",
                    $"Form Height:\t{Height}px");

                if (resizeTimer is not null)
                {
                    resizeTimer.Dispose();
                    resizeTimer = null;
                }

                this.MakeCurved(20);
            };

            resizeTimer.Start();
        }
    }
}
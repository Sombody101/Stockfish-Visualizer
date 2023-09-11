using Stockfish_Visualizer.Classes;
using System.Runtime.InteropServices;

namespace Stockfish_Visualizer;

public partial class VisualizerException : Form
{
    protected override CreateParams CreateParams
    {
        get
        {
            const int CS_DROPSHADOW = 0x20000;
            CreateParams cp = base.CreateParams;
            cp.ClassStyle |= CS_DROPSHADOW;
            return cp;
        }
    }

    public static VisualizerException CreateException(Exception ex)
    {
        var ve = new VisualizerException(ex);
        ve.Show();
        return ve;
    }

    public static VisualizerException CreateException(string exceptionMessage)
        => CreateException(new Exception(exceptionMessage));

    public VisualizerException(Exception e, bool hideOtherForms = true)
    {
        InitializeComponent();

        ErrorIcon.Location = new(Width / 2 - (ErrorIcon.Width / 2), ErrorIcon.Location.Y);

        ErrorReason.Text = e.Message;
        ErrorReasonPanel.Size = new(ErrorReason.Width + 50, ErrorReason.Height + 50);
        ErrorReasonPanel.Location = new(Width / 2 - (ErrorReasonPanel.Width / 2), ErrorReasonPanel.Location.Y);
        ErrorReason.Location = new(ErrorReasonPanel.Width / 2 - (ErrorReason.Width / 2), ErrorReasonPanel.Height / 2 - (ErrorReason.Height / 2));

        ErrorIcon.MouseDown += MoveForm;
        ErrorReason.MouseDown += MoveForm;
        MouseDown += MoveForm;

        this.MakeCurved(20);
        ErrorReasonPanel.MakeCurved(20);

        if (hideOtherForms)
        {
            foreach (Form form in Application.OpenForms)
                if (form != this)
                    form.Hide();
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
}

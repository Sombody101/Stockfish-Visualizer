using Stockfish_Visualizer.Controls;
using System.Drawing.Drawing2D;

namespace Stockfish_Visualizer.Classes;

public static class FormUtilities
{

    // This used to be how the form moved before
    //public static void MakeDraggable(this Form form)
    //{
    //    EventHandlers eventHandlers = new(form);
    //    form.MouseDown += eventHandlers.DragMouseDown;
    //    form.MouseMove += eventHandlers.DragMouseMove;
    //    form.MouseUp += eventHandlers.DragMouseUp;
    //}
    //
    //private class EventHandlers
    //{
    //    private bool isDragging = false;
    //    private Point lastCursor;
    //    private Point lastForm;
    //
    //    public EventHandlers(Form form)
    //    {
    //        form.HandleCreated += (sender, e) =>
    //        {
    //            form.MouseDown += DragMouseDown;
    //            form.MouseMove += DragMouseMove;
    //            form.MouseUp += DragMouseUp;
    //        };
    //
    //        form.HandleDestroyed += (sender, e) =>
    //        {
    //            form.MouseDown -= DragMouseDown;
    //            form.MouseMove -= DragMouseMove;
    //            form.MouseUp -= DragMouseUp;
    //        };
    //    }
    //
    //    public void DragMouseDown(object sender, MouseEventArgs e)
    //    {
    //        isDragging = true;
    //        lastCursor = Cursor.Position;
    //        lastForm = ((Form)sender).Location;
    //    }
    //
    //    public void DragMouseMove(object sender, MouseEventArgs e)
    //    {
    //        if (isDragging)
    //        {
    //            Point currentCursor = Cursor.Position;
    //            ((Form)sender).Location = new Point(lastForm.X + (currentCursor.X - lastCursor.X),
    //                lastForm.Y + (currentCursor.Y - lastCursor.Y));
    //        }
    //    }
    //
    //    public void DragMouseUp(object sender, MouseEventArgs e)
    //    {
    //        isDragging = false;
    //    }
    //}

    public static void SetFormSize(this Form form, int percentW, int percentH)
    {
        Screen primaryScreen = Screen.PrimaryScreen ?? throw new Exception("Not in GUI mode (?)");

        int screenWidth = primaryScreen.Bounds.Width;
        int screenHeight = primaryScreen.Bounds.Height;

        form.Width = (int)((double)percentW / 100 * screenWidth);
        form.Height = (int)((double)percentH / 100 * screenHeight);
    }

    public static void MakeCurved(this Control control, int curve)
    {
        using GraphicsPath path = new GraphicsPath();
        path.AddArc(0, 0, curve, curve, 180, 90);
        path.AddArc(control.ClientRectangle.Width - curve, 0, curve, curve, 270, 90);
        path.AddArc(control.ClientRectangle.Width - curve,
            control.ClientRectangle.Height - curve, curve, curve, 0, 90);
        path.AddArc(0, control.ClientRectangle.Height - curve, curve, curve, 90, 90);
        path.CloseFigure();
        control.Region = new Region(path);
    }

    public static readonly Dictionary<PieceType, Image> WhiteIndex = new()
    {
        { PieceType.Pawn,   Pieces.WPawn },
        { PieceType.Rook,   Pieces.WRook },
        { PieceType.Knight, Pieces.WKnight },
        { PieceType.Bishop, Pieces.WBishop },
        { PieceType.Queen,  Pieces.WQueen },
        { PieceType.King,   Pieces.WKing },
    };

    public static readonly Dictionary<PieceType, Image> BlackIndex = new()
    {
        { PieceType.Pawn,   Pieces.BPawn },
        { PieceType.Rook,   Pieces.BRook },
        { PieceType.Knight, Pieces.BKnight },
        { PieceType.Bishop, Pieces.BBishop },
        { PieceType.Queen,  Pieces.BQueen },
        { PieceType.King,   Pieces.BKing },
    };
}
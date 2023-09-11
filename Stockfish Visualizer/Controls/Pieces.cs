using Stockfish_Visualizer.Classes;
using System.Drawing.Drawing2D;

namespace Stockfish_Visualizer.Controls;

public enum PieceType : byte
{
    Pawn,
    Knight,
    Rook,
    Bishop,
    Queen,
    King,
}

public enum MoveType
{
    Move,
    Attack,

    // Special moves
    EnPassant,      // Move pawn, then remove one on another panel
    PawnPromotion,  // Prompt user for which piece to upgrade to 
    Castle,         // Move two pieces (no take)
}

/// <inheritdoc/>
[Serializable]
public class Piece : PictureBox
{
    public bool IsWhite { get; private set; }
    public PieceType Type { get; private set; }

    public Position Position { get; set; }
    public Position LastPosition { get; set; }

    public bool MovementDisabled { get; set; }
    public int Moves { get; set; } = 0;

    public Piece(PieceType type, Position position, bool isWhite = true)
    {
        Type = type;
        Position = LastPosition = position;
        IsWhite = isWhite;
        SizeMode = PictureBoxSizeMode.Zoom;
        Name = (isWhite ? "W" : "B") + type;
        Image = isWhite ? FormUtilities.WhiteIndex[type] : FormUtilities.BlackIndex[type];

#if !DEBUG
        MovementDisabled = !isWhite;
#endif

        MouseDown += new MouseEventHandler(ChessPiece_MouseDown);
        MouseMove += new MouseEventHandler(ChessPiece_MouseMove);
        MouseUp += new MouseEventHandler(ChessPiece_MouseUp);
        MouseEnter += new EventHandler(ChessPiece_MouseEnter);

        BringToFront();
    }

    // Event handlers
    private bool isSelected = false;
    private bool isDragging = false;
    private Point offset;
    private Control? fallbackParent;

    /// <summary>
    /// Triggered when the user presses their mouse button down
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessPiece_MouseDown(object? sender, MouseEventArgs e)
    {
        GameBoard.SelectedPiece = Position;

        if (e.Button is MouseButtons.Left)
        {
            isSelected = true;
        }
    }

    /// <summary>
    /// Triggered every time the mouse moves while on the piece
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessPiece_MouseMove(object? sender, MouseEventArgs e)
    {
        if (isSelected && !MovementDisabled)
        {
            if (!isDragging)
            {
                offset = Location = new Point(e.X, e.Y);
                Size = new(GameBoard.PanelSize - 20, GameBoard.PanelSize - 20);
                fallbackParent = Parent;
                Parent = GameBoard.Window;
                BringToFront();
            }

            isDragging = true;
            int newX = e.X + Left - offset.X;
            int newY = e.Y + Top - offset.Y;

            const int formLeft = 0;
            const int formTop = 0;
            int formRight = GameBoard.Window.ClientSize.Width - Width;
            int formBottom = GameBoard.Window.ClientSize.Height - Height;

            if (newX >= formLeft && newX <= formRight &&
                newY >= formTop && newY <= formBottom)
            {
                Left = newX;
                Top = newY;
                BringToFront();
            }
        }
    }

    /// <summary>
    /// Triggered when the user releases their mouse button on a piece
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessPiece_MouseUp(object? sender, MouseEventArgs e)
    {
        if (MovementDisabled)
        {
            Cursor = Cursors.No;
            return;
        }

        if (e.Button is MouseButtons.Left && isSelected)
        {
            if (!isDragging)
            {
                isDragging = isSelected = false;
                return;
            }

            isDragging = isSelected = false;
            Cursor = Cursors.Default;
            Visible = true;

            void Fallback()
            {
                Parent = fallbackParent;
                Location = new(0, 0);
            }

            Parent = FindNewParent();
            Size = new(GameBoard.PanelSize, GameBoard.PanelSize);

            if (Parent is Form or null)
            {
                Fallback();
                return;
            }

            var newPos = new Position(Parent.Name);
            // check if placement is valid or not (assign point to lastPosition if its invalid)
            if (Position.Coord == newPos.Coord || !MoveCalc.ProcessMove(this, newPos))
            {
                Fallback();
                return;
            }

            Location = new Point(0, 0);
            //MovePiece();
            Moves++;
        }
    }

    /// <summary>
    /// Triggered when the users cursor is hovering over the piece
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessPiece_MouseEnter(object? sender, EventArgs e)
    {
        Cursor = MovementDisabled ? Cursors.No : Cursors.Hand;

        //if (Configuration.DebugMode)
        //{
        //    if (Parent is Form)
        //        return;
        //    var parentPos = new Position(Parent!.Name);
        //
        //    Deb.WriteLine($"Parent-{Parent.Name}|{parentPos.X}.{parentPos.Y} : This-{Position.Coord}|{Position.X}.{Position.Y} ({(IsWhite ? "White" : "Black")})");
        //}
    }

    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        using var gp = new GraphicsPath();
        gp.AddEllipse(new Rectangle(0, 0, Width, Height));
        Region = new Region(gp);
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
        if (Size.Width != GameBoard.PanelSize)
            Size = new Size(GameBoard.PanelSize, GameBoard.PanelSize);
        base.OnPaint(pe);
    }

    // Other private members
    private Control FindNewParent()
    {
        Point cursorPosition = Cursor.Position;
        Point formCursorPosition = GameBoard.Window.PointToClient(cursorPosition);

        foreach (Panel panel in GameBoard.Panels)
            if (panel.Bounds.Contains(formCursorPosition))
                return panel;

        return GameBoard.Window;
    }

    private void MovePiece()
    {
        //Deb.Write(Position.Coord + " => " + move.Position.Coord);

        //var move = MoveCalc.ProcessMove(this);

        //var curp = GameBoard.Board[move.Position.X, move.Position.Y];
        //if (curp == this)
        //    return;
        //if (curp is not null)
        //{
        //    //if (IsWhite)
        //    //    GameBoard.TakenBlack.Add(curp);
        //    //else
        //    //    GameBoard.TakenWhite.Add(curp);
        //    //GameBoard.Panels[move.Position.X, move.Position.Y].Controls.Clear();
        //    //Deb.Write(" | " + (curp.IsWhite ? "W" : "B") + curp.Type + " taken");
        //}

        //Deb.Write("\r\n");
        //LastPosition = Position;
        //GameBoard.Board[Position.X, Position.Y] = null;
        //Position = move.Position;
        //GameBoard.Board[move.Position.X, move.Position.Y] = this;
        //GameBoard.Panels[move.Position.X, move.Position.Y].Controls.Add(this);
    }
}
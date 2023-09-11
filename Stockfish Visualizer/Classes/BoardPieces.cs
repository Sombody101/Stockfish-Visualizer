using Stockfish_Visualizer.Controls;
using Deb = Stockfish_Visualizer.Classes.Debugging;

namespace Stockfish_Visualizer.Classes;

public static class GameBoard
{
    public static Visualizer Window { get; private set; }

    /// <summary>
    /// A multidimensional array of the pieces being rendered on the panels on the form
    /// </summary>
    public static Piece?[,] Board { get; private set; } = new Piece[gridSize, gridSize];

    /// <summary>
    /// A multidimensional array of the panels being rendered on the form
    /// </summary>
    public static CPanel[,] Panels { get; private set; } = new CPanel[gridSize, gridSize];

    public static int PanelSize { get; private set; } = 70;

    public static List<Piece> TakenWhite { get; private set; } = new();
    public static List<Piece> TakenBlack { get; private set; } = new();

    public static Color Square1 { get; set; } = ColorTranslator.FromHtml("#a87a65");
    public static Color Square2 { get; set; } = ColorTranslator.FromHtml("#edd8bf");
    public static Color SelectedSquare1 { get; set; } = ColorTranslator.FromHtml("#79a865");
    public static Color SelectedSquare2 { get; set; } = ColorTranslator.FromHtml("#c1edbf");
    public static Color CheckSquare1 { get; set; } = ColorTranslator.FromHtml("#a63232");
    public static Color CheckSquare2 { get; set; } = ColorTranslator.FromHtml("#e34d4d");

    private static bool whiteInCheck = false;
    public static bool WhiteInCheck
    {
        get => whiteInCheck;
        set
        {
            whiteInCheck = value;

            var king = GetPieceByName("WKing");

            if (king is null)
                return;

            if (value)
            {
                Panels[king.Position.X, king.Position.Y].BackColor = (king.Position.X + king.Position.Y) % 2 is 0 ? CheckSquare1 : CheckSquare2;
                return;
            }

            Panels[king.Position.X, king.Position.Y].BackColor = (king.Position.X + king.Position.Y) % 2 is 0 ? Square1 : Square2;
        }
    }

    private static bool blackInCheck = false;
    public static bool BlackInCheck
    {
        get => blackInCheck;
        set
        {
            blackInCheck = value;

            var king = GetPieceByName("BKing");

            if (king is null)
                return;

            if (value)
            {
                Panels[king.Position.X, king.Position.Y].BackColor = (king.Position.X + king.Position.Y) % 2 is 0 ? CheckSquare1 : CheckSquare2;
                return;
            }

            Panels[king.Position.X, king.Position.Y].BackColor = (king.Position.X + king.Position.Y) % 2 is 0 ? Square1 : Square2;
        }
    }

    public static List<Move> MovesForSelectedPiece { get; set; } = new();

    private static Position selectedPiece = new(0, 0);

    /// <summary>
    /// The piece the user has currently selected (Auto highlights possible moves on set)
    /// </summary>
    public static Position SelectedPiece
    {
        get => selectedPiece;
        set
        {
            // Cleanup previous piece colors
            CleanPanelColors();

            selectedPiece = value;
            MoveCalc.GetPossiblePositions(Board[value.X, value.Y]!, out var nMoves);
            MovesForSelectedPiece = nMoves;

            foreach (Move move in MovesForSelectedPiece)
            {
                Panels[move.Position.X, move.Position.Y].BackColor = Board[move.Position.X, move.Position.Y]?.Type is PieceType.King && (WhiteInCheck || BlackInCheck)
                    ? (move.Position.X + move.Position.Y) % 2 is 0 ? CheckSquare1 : CheckSquare2
                    : (move.Position.X + move.Position.Y) % 2 is 0 ? SelectedSquare1 : SelectedSquare2;
                Panels[move.Position.X, move.Position.Y].MovePosition = move;
            }

            Panels[value.X, value.Y].BackColor = (value.X + value.Y) % 2 is 0 ? SelectedSquare1 : SelectedSquare2;
            isClean = false;
        }
    }

    private static bool isClean = true;
    public static void CleanPanelColors()
    {
        if (selectedPiece is not null && !isClean)
        {
            // Remove highlighting from all possible panels
            foreach (Move move in MovesForSelectedPiece)
            {
                Panels[move.Position.X, move.Position.Y].BackColor = Board[move.Position.X, move.Position.Y]?.Type is PieceType.King && (WhiteInCheck || BlackInCheck)
                    ? (move.Position.X + move.Position.Y) % 2 is 0 ? CheckSquare1 : CheckSquare2
                    : (move.Position.X + move.Position.Y) % 2 is 0 ? Square1 : Square2;
                Panels[move.Position.X, move.Position.Y].MovePosition = null!;
            }

            // Highlight selected piece's panel
            if (Board[selectedPiece.X, selectedPiece.Y]?.Type is not PieceType.King || !(WhiteInCheck || BlackInCheck))
                Panels[selectedPiece.X, selectedPiece.Y].BackColor = (selectedPiece.X + selectedPiece.Y) % 2 is 0 ? Square1 : Square2;
            MovesForSelectedPiece = new();
            isClean = true;
        }
    }

    public static void Initialize(Visualizer _window, bool setupGame = true)
    {
        Window = _window;
        PanelSize = Window.Height / 10; // Same as the grid size, accounting for margins

        if (setupGame)
        {
            var then = DateTime.Now;

            Deb.WriteTitle("Board initialization");
            CreateBoard((int)(Window.Width * 0.5), (Window.Height + 30 - (PanelSize * gridSize)) / 2);
            InitializePieces();
            MapPieces();

            Debugging.WriteLine($"Render took {(DateTime.Now - then).Milliseconds}ms");
        }
    }

    public static Piece? GetPieceByName(string name)
    {
        foreach (var piece in Board)
            if (piece is not null && piece.Name == name)
                return piece;

        return null;
    }

    private const int gridSize = 8;
    public static void CreateBoard(int startX, int startY, bool redraw = false)
    {
        if (!redraw)
            Panels = new CPanel[gridSize, gridSize];

        int posX = startX;
        int posY = startY + (PanelSize * (gridSize - 1));

        for (int num = 0; num < gridSize; num++)
        {
            for (int let = 0; let < gridSize; let++)
            {
                if (!redraw)
                {
                    CPanel panel = new()
                    {
                        Size = new Size(PanelSize, PanelSize),
                        Location = new Point(posX, posY),
                        BackColor = (let + num) % 2 is 0 ? Square1 : Square2,
                        Parent = Window,
                        Name = new Position(let, num).Coord
                    };

                    panel.Click += (sender, e) => { CleanPanelColors(); };
                    Window.Controls.Add(panel);
                    Panels[let, num] = panel;
                }
                else
                {
                    var panel = Panels[let, num];
                    panel.Size = new Size(PanelSize, PanelSize);
                    panel.Location = new Point(posX, posY);

                    var piece = Board[let, num];
                    if (piece is not null)
                    {
                        piece.Size = new Size(PanelSize, PanelSize);
                        piece.Location = new(0, 0);
                    }

                    //Task.Delay(25).GetAwaiter().GetResult();
                    //Window.Refresh();
                }

                posX += PanelSize;
            }

            posY -= PanelSize;
            posX = startX;
        }

        if (!redraw)
        {
            Deb.WriteLine("Board[[,]]");
            for (int num = 0; num < 8; num++)
                for (int let = 0; let < 8; let++)
                    Deb.Write($"{let}.{num}:{Panels[let, num].Name} {(let is 7 ? "\r\n" : "")}");
        }
    }

    public static void MapPieces()
    {
        for (int num = 0; num < gridSize; num++)
        {
            for (int let = 0; let < gridSize; let++)
            {
                var panel = Panels[let, num];

                panel.SendToBack();
                panel.Controls.Clear();

                var piece = Board[let, num];
                if (piece is not null)
                {
                    piece.Size = new Size(PanelSize, PanelSize);
                    piece.Anchor = AnchorStyles.None;
                    piece.Parent = panel;
                    panel.Controls.Add(piece);
                }
            }
        }
    }

    public static void InitializePieces()
    {
        Board = new Piece[gridSize, gridSize];

        // White pieces
        Board[0, 0] = new(PieceType.Rook, new("A1"));
        Board[1, 0] = new(PieceType.Knight, new("B1"));
        Board[2, 0] = new(PieceType.Bishop, new("C1"));
        Board[3, 0] = new(PieceType.Queen, new("D1"));
        Board[4, 0] = new(PieceType.King, new("E1"));
        Board[5, 0] = new(PieceType.Bishop, new("F1"));
        Board[6, 0] = new(PieceType.Knight, new("G1"));
        Board[7, 0] = new(PieceType.Rook, new("H1"));

        // Pawns
        for (int i = 0; i < gridSize; i++)
            Board[i, 1] = new(PieceType.Pawn, new(i, 1));

        // Black pieces
        Board[0, 7] = new(PieceType.Rook, new("A8"), false);
        Board[1, 7] = new(PieceType.Knight, new("B8"), false);
        Board[2, 7] = new(PieceType.Bishop, new("C8"), false);
        Board[3, 7] = new(PieceType.Queen, new("D8"), false);
        Board[4, 7] = new(PieceType.King, new("E8"), false);
        Board[5, 7] = new(PieceType.Bishop, new("F8"), false);
        Board[6, 7] = new(PieceType.Knight, new("G8"), false);
        Board[7, 7] = new(PieceType.Rook, new("H8"), false);

        // Pawns
        for (int i = 0; i < gridSize; i++)
            Board[i, 6] = new(PieceType.Pawn, new(i, 6), false);

        Deb.WriteLine("\r\nInitializePieces()");
        for (int num = 0; num < 8; num++)
            for (int let = 0; let < 8; let++)
                if (Board[let, num] is not null)
                    Deb.Write($"{let}.{num}:{Board[let, num]!.Position.Coord}({Board[let, num]!.Type.ToString()[0]}) {(let is 7 ? "\r\n" : "")}");

        Deb.WriteLine();
    }

    public static void RedrawBoard()
    {
        if (Window is null)
            return;

        PanelSize = Window.Height / 10;

        int posX = (int)(Window.Width * 0.75) - (PanelSize * 4);
        int posY = (Window.Height + 30 - (PanelSize * gridSize)) / 2;

        Window.GamePresenter.Size = Window.GamePresenter.MaximumSize = new(Window.Width / 2, Window.Height - Window.Titlebar.Height);

        // Show items vertically when form is taller than it is wide
        if (PanelSize * 8 + 5 > Window.Width / 2)
        {
            PanelSize = Window.Height / 18;
            posX = Window.Width / 2 - (PanelSize * 4);
            posY = ((int)(Window.Height * .75)) - (PanelSize * 4);
            Window.GamePresenter.Size = Window.GamePresenter.MaximumSize = new(Window.Width, Window.Height / 2 - Window.Titlebar.Height - 5);
        }

        CreateBoard(posX, posY, true);
    }
}
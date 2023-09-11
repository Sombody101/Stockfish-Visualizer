using Stockfish_Visualizer.Controls;

namespace Stockfish_Visualizer.Classes;

public class Move
{
    public Move(Position position, MoveType type)
    {
        Position = position;
        Type = type;
    }

    public Position Position { get; private set; }
    public MoveType Type { get; private set; }
}

public static class MoveCalc
{
    public static bool GetPossiblePositions(this Piece piece, out List<Move> moves)
    {
        switch (piece.Type)
        {
            case PieceType.Pawn:
                moves = ForPawn(piece);
                return true;
            case PieceType.Rook:
                moves = GetRookMoves(piece);
                return true;
            case PieceType.Knight:
                break;
            case PieceType.Bishop:
                break;
            case PieceType.Queen:
                break;
            case PieceType.King:
                break;
        }

        moves = new();
        return false;
    }

    public static List<Move> ForPawn(Piece piece)
    {
        List<Move> pawnMoves = new();

        int direction = piece.IsWhite ? 1 : -1;

        int x = piece.Position.X;
        int y = piece.Position.Y;

        // Forward move
        if (IsValidSquare(x, y + direction) && IsSquareEmpty(x, y + direction))
        {
            pawnMoves.Add(new(new(x, y + direction), MoveType.Move));

            // Initial double move
            if (piece.Moves == 0 && IsValidSquare(x, y + 2 * direction) && IsSquareEmpty(x, y + 2 * direction))
                pawnMoves.Add(new(new(x, y + 2 * direction), MoveType.Move));
        }

        // Diagonal attacks
        if (IsValidSquare(x - 1, y + direction) && IsSquareOccupied(x - 1, y + direction, !piece.IsWhite))
            pawnMoves.Add(new(new(x - 1, y + direction), MoveType.Attack));

        if (IsValidSquare(x + 1, y + direction) && IsSquareOccupied(x + 1, y + direction, !piece.IsWhite))
            pawnMoves.Add(new(new(x + 1, y + direction), MoveType.Attack));

        return pawnMoves;
    }

    public static List<Move> GetRookMoves(Piece piece, bool ignoreSpecial = false)
    {
        List<Move> rookMoves = new();

        int x = piece.Position.X;
        int y = piece.Position.Y;

        // Horizontal moves
        for (int dx = -1; dx <= 1; dx += 2)
        {
            for (int xOffset = 1; xOffset < 8; xOffset++)
            {
                int newX = x + dx * xOffset;
                if (!IsValidSquare(newX, y)) break;

                if (IsSquareEmpty(newX, y))
                    rookMoves.Add(new(new(newX, y), MoveType.Move));
                else
                {
                    if (IsSquareOccupied(newX, y, !piece.IsWhite))
                        rookMoves.Add(new(new(newX, y), MoveType.Attack));
                    break;
                }
            }
        }

        // Vertical moves
        for (int dy = -1; dy <= 1; dy += 2)
        {
            for (int yOffset = 1; yOffset < 8; yOffset++)
            {
                int newY = y + dy * yOffset;

                if (!IsValidSquare(x, newY))
                    break;

                if (IsSquareEmpty(x, newY))
                    rookMoves.Add(new(new(x, newY), MoveType.Move));
                else
                {
                    if (IsSquareOccupied(x, newY, !piece.IsWhite))
                        rookMoves.Add(new(new(x, newY), MoveType.Attack));
                    break;
                }
            }
        }

        // Caslting
        if (!ignoreSpecial)
        {

        }

        return rookMoves;
    }

    public static bool ProcessMove(this Piece piece, Position newPos)
    {
        var move = GameBoard.MovesForSelectedPiece.Select(m => m.Position.Coord == newPos.Coord);



        return false;
    }

    private static bool IsValidSquare(int x, int y)
        => x >= 0 && x < 8 && y >= 0 && y < 8;

    private static bool IsSquareEmpty(int x, int y)
        => GameBoard.Board[x, y] is null;

    private static bool IsSquareOccupied(int x, int y, bool isEnemyPiece)
    {
        var piece = GameBoard.Board[x, y];

        if (piece is null)
            return false;

        return isEnemyPiece ? !piece.IsWhite : piece.IsWhite;
    }
}
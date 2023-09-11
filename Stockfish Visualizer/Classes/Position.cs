namespace Stockfish_Visualizer.Classes;

/// <summary>
/// This is a class used for keeping track of the pieces on the board. It's based 1 because the positions range from [A-H][1-8] while the board array ranges from [0-7][0-7]
/// </summary>
public class Position
{
    public int X { get; set; } = -1;
    public int Y { get; set; } = -1;
    public string Coord;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
        Coord = GetStringFromCoords(x, y);
    }

    public Position(string coord)
    {
        (X, Y) = GetCoordsFromString(coord);
        Coord = coord;
    }

    /// <summary>
    /// Returns two ints that represent the input coordinate
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    private static (int, int) GetCoordsFromString(string coord)
    {
        GetCoordsFromString(coord, out int x, out int y);
        return (x, y);
    }

    /// <summary>
    /// Returns two ints that represent the input coordinate
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static void GetCoordsFromString(string coord, out int x, out int y)
    {
        if (coord.Length is not 2)
            throw new Exception("Improper string coordinate length");

        if (!int.TryParse(coord[1].ToString(), out int _y))
            throw new Exception("Non-numeric Y value in string coordinate");

        if (_y is > 8 or < 1)
            throw new Exception("Out of bounds Y value in string coordinate");

        char _x = char.ToUpper(coord[0]);

        if (((int)_x) is > 72 or < 65)
            throw new Exception("Out of bounds X value in string coordinate");

        x = _x - 65;
        y = _y - 1;
    }

    /// <summary>
    /// Returns the string version of the position (Coord is now a field, making it easier and faster to get it)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static string GetStringFromCoords(int x, int y)
    {
        return x < 0 || x > 9
            ? throw new Exception("X coordinate out of bounds")
            : y < 0 || y > 9 ? throw new Exception("Y coordinate out of bounds") : $"{(char)(x + 'A')}{y + 1}";
    }
}
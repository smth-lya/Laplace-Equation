namespace LaplaceSOR.Visualization.Models;

public struct Cell
{
    public CellType Type;
    public double Value;

    public Cell(CellType type, double value)
    {
        Type = type;
        Value = value;
    }
}


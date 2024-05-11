namespace LaplaceSOR.Contracts;

/// <summary>
/// Represents a cell used for approximating Laplace's equation.
/// </summary>
public struct LaplaceCell
{
    /// <summary>
    /// The type of the cell, whether it's fixed or floating.
    /// </summary>
    public CellType Type;

    /// <summary>
    /// The value stored in the cell.
    /// </summary>
    public double Value;

    /// <summary>
    /// Initializes a new instance of the Cell structure with the specified type and value.
    /// </summary>
    /// <param name="type">The type of the cell.</param>
    /// <param name="value">The value of the cell.</param>
    public LaplaceCell(CellType type, double value)
    {
        Type = type;
        Value = value;
    }
}


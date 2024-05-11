namespace LaplaceSOR.Contracts;

/// <summary>
/// Types of cells for approximating Laplace's equation.
/// </summary>
public enum CellType
{
    /// <summary>
    /// A cell whose value can freely change during approximation.
    /// </summary>
    Float,

    /// <summary>
    /// A fixed cell whose value is predetermined and remains constant.
    /// </summary>
    Fixed
}

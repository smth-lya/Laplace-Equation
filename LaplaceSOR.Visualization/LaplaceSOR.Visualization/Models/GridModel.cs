using System.Diagnostics.CodeAnalysis;

namespace LaplaceSOR.Visualization.Models;

public class GridModel
{
    public Cell[,] _grid;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public GridModel(int width, int height)
    {
        Width = width;
        Height = height;

        InitializeGrid();
    }


    [MemberNotNull(nameof(_grid))]
    private void InitializeGrid()
    {
        _grid = new Cell[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _grid[y, x] = new Cell(CellType.Float, 0);
            }
        }
    }

    public double GetCellValue(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return _grid[y, x].Value;
        }
        else
        {
            return 0;
        }
    }
    public void UpdateCell(int x, int y, double value, CellType type)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            _grid[y, x].Value = value;
            _grid[y, x].Type = type;
        }
    }
}

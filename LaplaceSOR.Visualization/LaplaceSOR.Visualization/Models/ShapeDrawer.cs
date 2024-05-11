using LaplaceSOR.Contracts;
using LaplaceSOR.Visualization.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace LaplaceSOR.Visualization.Models;
public class ShapeDrawer
{
    private GridModel _gridModel;
    private LaplaceGridViewModel _viewModel;

    private ILaplaceEquationSolver? _solver;

    private SKPoint _cellSize;


    public ShapeDrawer(GridModel gridModel, LaplaceGridViewModel viewModel, ILaplaceEquationSolver? solver)
    {
        _gridModel = gridModel;
        _viewModel = viewModel;

        _solver = solver;
    }

    public void OnCanvasViewPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        if (_solver != null)
            _gridModel._grid = _solver.Grid;

        DrawGrid(canvas);
    }
    public void TouchEventHandler(object? sender, SKTouchEventArgs e)
    {
        var canvasView = sender as SKCanvasView;

        if (canvasView == null) return;

        if (e.MouseButton == SKMouseButton.Left)
        {
            SKPoint location = FixLocationByGrid(e.Location);

            int x = (int)(location.X / _cellSize.X);
            int y = (int)(location.Y / _cellSize.Y);

            double value = _viewModel.DrawingValue;

            switch (_viewModel.ShapeType)
            {
                case ShapeType.Point:
                    _gridModel.UpdateCell(x, y, value, _viewModel.CellType);
                    break;
                case ShapeType.Circle:
                    DrawCircle(_viewModel.BrushSize, location);
                    break;
                case ShapeType.Chaos:
                    DrawChaosCircle(_viewModel.BrushSize, location);
                    break;
                default:
                    break;
            }

            canvasView.InvalidateSurface();
        }
    }

    private void DrawGrid(SKCanvas canvas)
    {
        int height = _gridModel.Height;
        int width = _gridModel.Width;

        _cellSize = new SKPoint(canvas.DeviceClipBounds.Width / (float)width, canvas.DeviceClipBounds.Height / (float)height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SKColor color = ValueToColor(_gridModel.GetCellValue(x, y));

                SKRect rect = SKRect.Create(x * _cellSize.X, y * _cellSize.Y, _cellSize.X, _cellSize.Y);

                using (SKPaint paint = new SKPaint { Color = color })
                {
                    canvas.DrawRect(rect, paint);
                }
            }
        }
    }

    private void DrawCircle(float diameter, SKPoint center)
    {
        int height = _gridModel.Height;
        int width = _gridModel.Width;

        center = FixLocationByGrid(center);

        center.X /= _cellSize.X;
        center.Y /= _cellSize.Y;

        var squaredRadius = diameter * diameter / 4;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float X = x - center.X;
                float Y = y - center.Y;

                float sum = X * X + Y * Y;

                if (sum <= squaredRadius)
                {
                    _gridModel.UpdateCell(x, y, _viewModel.DrawingValue, _viewModel.CellType);
                }
            }

        }
    }
    private void DrawChaosCircle(float radius, SKPoint center)
    {
        int height = _gridModel.Height;
        int width = _gridModel.Width;

        float aspect = (float)width / height;

        center.X /= _cellSize.X;
        center.Y /= _cellSize.Y;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float X = x / (float)width * 2.0f - 1.0f;
                float Y = y / (float)height * 2.0f - 1.0f;

                X -= center.X / width * 2.0f - 1.0f;
                Y -= center.Y / height * 2.0f - 1.0f;

                X *= aspect;

                float sum = X * X + Y * Y;

                var color = 1 / Math.Sqrt(sum) * radius;

                if (color <= radius) continue;

                _gridModel._grid[y, x].Value = color;
                _gridModel._grid[y, x].Type = _viewModel.CellType;
            }

        }
    }


    private SKPoint FixLocationByGrid(SKPoint point)
    {
        point.X = (int)(point.X / _cellSize.X) * _cellSize.X;
        point.Y = (int)(point.Y / _cellSize.Y) * _cellSize.Y;

        return point;
    }
    private SKColor ValueToColor(double value)
    {
        var NColors = _viewModel.NColors;
        if (NColors == 0) NColors = 2;
        double ratio = value / 100;
        int colorInterval = 100 / NColors;
        int colorIndex = (int)(ratio * NColors);
        int red = (byte)((colorIndex * colorInterval / 100.0) * 255);
        int blue = (byte)(((NColors - colorIndex) * colorInterval / 100.0) * 255);
        return new SKColor((byte)red, 0, (byte)blue);
    }
}

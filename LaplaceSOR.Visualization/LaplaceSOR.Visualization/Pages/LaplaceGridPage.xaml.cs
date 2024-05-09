using LaplaceSOR.Contracts;
using LaplaceSOR.Visualization.ViewModel;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LaplaceSOR.Visualization.Pages;

public interface ICell
{
    CellType Type { get; set; }
    int Value { get; set; }
}
public enum CellType
{
    Float,
    Fixed
}
public class Cell : ICell
{
    public CellType Type { get; set; }
    public int Value { get; set; }

    public Cell(CellType type, int value)
    {
        Type = type;
        Value = value;
    }
}

public partial class LaplaceGridPage : ContentPage
{
    private ILaplaceEquationSolver? _solver;
    private double[,] _grid;

    private LaplaceGridViewModel _vm;

    SKPoint _currentPoint;

    private float cellsize => (float)726 / _vm.SizeX; 

    public LaplaceGridPage(LaplaceGridViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        _vm = vm;

        InitializeGrid();

        _grid = new double[_vm.SizeY, _vm.SizeX];
        _vm.BrushSize = _vm.SizeX / 4;

        canvasView.EnableTouchEvents = true;
        canvasView.Touch += CanvasView_Touch;
    }

    private void CanvasView_Touch(object? sender, SKTouchEventArgs e)
    {
        _currentPoint = FixLocationByGrid(e.Location);


        if (e.MouseButton == SKMouseButton.Left)
        {

            int Y = (int)(_currentPoint.Y / cellsize);
            int X = (int)(_currentPoint.X / cellsize);

            if (Y < 0 || Y >= _grid.GetLength(0) || X < 0 || X >= _grid.GetLength(1))
            {
                return;
            }

            _grid[Y, X] = _vm.CellType == CellType.Fixed ? 50 : 100;

            canvasView.InvalidateSurface();

        }
        else if (e.MouseButton == SKMouseButton.Right)
        {

            DrawCircle(_vm.BrushSize, _currentPoint);
            
            canvasView.InvalidateSurface();

        }

        MousePosText.Text = $"Mouse: \nX: {e.Location.X:F0} Y: {e.Location.Y:F0}";
        
        MouseNormalizedPosText.Text = $"Normalized Mouse: \nX: {_currentPoint.X / cellsize} Y: {_currentPoint.Y / cellsize}";
    
    }

    [MemberNotNull(nameof(_grid))]
    private void InitializeGrid()
    {
        int width = 20;
        int height = 20;

        _vm.SizeX = width;
        _vm.SizeY = height;
        
        _grid = new double[height, width];
        for (int x = 0; x < width; x++)
        {
            _grid[0, x] = 100;
            _grid[height - 1, x] = 100;
        }

        for (int y = 0; y < height; y++)
        {
            _grid[y, 0] = 100;
            _grid[y, width - 1] = 100; 
        }
    }
    private async void OnGridUpdated()
    {
        while (true)
        {
            canvasView.InvalidateSurface();
            await Task.Delay(1000);
        }
    }
    private void OnCanvasViewPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        DrawLaplaceSolution(canvas);
        DrawGrid(canvas);
    }

    private void DrawCircle(float radius, SKPoint center)
    {
        int height = _grid.GetLength(0);
        int width = _grid.GetLength(1);

        float aspect = (float)width / height;

        center = FixLocationByGrid(center);

        center.X /= cellsize;
        center.Y /= cellsize;

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

                var color = sum < radius / (height + width) ? 100 : 0;

                if (color != 0)
                {
                    _grid[y, x] = color;
                }
            }
            
        }
    }

    private void DrawCircle2(float diameter, SKPoint center)
    {
        int height = _grid.GetLength(0);
        int width = _grid.GetLength(1);

        center = FixLocationByGrid(center);

        center.X /= cellsize;
        center.Y /= cellsize;
        
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
                    _grid[y, x] = 100;
                }
            }

        }
    }

    private void DrawLaplaceSolution(SKCanvas canvas)
    {
        if (_solver == null) return;

        int width = _solver.Width;
        int height = _solver.Height;
        double[,] grid = _solver.Grid;

        float cellWidth = canvasView.CanvasSize.Width / width;
        float cellHeight = canvasView.CanvasSize.Height / height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //SKColor color = ValueToColor(grid[y, x]);

                //SKRect rect = SKRect.Create(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                //using (SKPaint paint = new SKPaint { Color = color })
                //{
                //    canvas.DrawRect(rect, paint);
                //}
            
            }
        }
    }

    private void DrawGrid(SKCanvas canvas)
    {
        int height = _grid.GetLength(0);
        int width = _grid.GetLength(1);

        var grid = _grid;

        var cellWidth = cellsize;
        var cellHeight = cellsize;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SKColor color = ValueToColor(grid[y, x]);

                SKRect rect = SKRect.Create(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
                using (SKPaint paint = new SKPaint { Color = _vm.CellType == CellType.Fixed ? SKColors.White : color, StrokeWidth = 6, Style = SKPaintStyle.Stroke })
                {
                    canvas.DrawRect(rect, paint);
                }

                using (SKPaint paint = new SKPaint { Color = color, Style = SKPaintStyle.Fill })
                {
                    canvas.DrawRect(rect, paint);
                }
            }
        }
    }

    private SKPoint FixLocationByGrid(SKPoint point)
    {
        point.X = (int)(point.X / cellsize) * cellsize;
        point.Y = (int)(point.Y / cellsize) * cellsize;

        return point; 
    }

    private SKColor ValueToColor(double value)
    {
        double ratio = value / 100;

        return new SKColor((byte)(ratio * 255), 0, (byte)((1 - ratio) * 255));
    }
    private void OnSolveButtonClicked(object sender, EventArgs e)
    {
        if (_solver == null) return;

        new Thread(() => _solver.Solve()).Start();
        DisplayAlert("Ok", "Ok", "Ok");
    }



    private async void OnFilePickerClicked(object? sender, EventArgs e)
    {
        var assemblyPath = await PickAssemblyFilePathAsync();
        
        if (assemblyPath == null) return;

        _solver = await LoadLaplaceSolverFromAssemblyAsync(assemblyPath);

        if (_solver != null)
        {
            //_solver.GridUpdated += OnGridUpdated;
            _solver.SetGrid(_grid, 4);
            StartButton.Opacity = 1;
            //OnGridUpdated();
            StartButton.BackgroundColor = Colors.YellowGreen;
        }
    }
    private async Task<string?> PickAssemblyFilePathAsync()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a Laplace assembly file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>() { [DevicePlatform.WinUI] = [".dll"] })
            });

            if (result == null)
            {
                await DisplayAlert("Try again!", "Path is empty", "OK");
            }

            return result?.FullPath;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }

        return null;
    }
    private async Task<ILaplaceEquationSolver?> LoadLaplaceSolverFromAssemblyAsync(string assemblyPath)
    {
        Assembly assembly;

        try
        {
            assembly = Assembly.LoadFile(assemblyPath);
        }
        catch (FileNotFoundException)
        {
            await DisplayAlert("File Not Founded", $"The assembly file \"{assemblyPath}\" was not found.", "OK");
            return null;
        }
        catch (Exception ex)
        {
            await DisplayAlert(ex.ToString(), "Error loading solver from assembly: " + ex.Message, "OK");
            return null;
        }

        var type = assembly.GetTypes()
                           .FirstOrDefault(type => type.IsAssignableTo(typeof(ILaplaceEquationSolver)));

        if (type == null)
        {
            await DisplayAlert("Incompatible assembly. Type is NULL", "The assembly does not contain a type implementing ILaplaceEquationSolver interface.", "OK");
            return null;
        }

        if (Activator.CreateInstance(type, true) is object solverInstance)
        {
            return (ILaplaceEquationSolver)solverInstance;
        }
        else
        {
            await DisplayAlert("Invalid Operation Exception", "Failed to create instance", "OK");
            return null;
        }
    }

    private async void OnButtonPressed(object? sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == null) return;

        var task1 = button.ScaleTo(0.8, 150, Easing.Linear);

        var task2 = button.FadeTo(0.7, 150, Easing.Linear);

        await Task.WhenAll(task1, task2);
    }
    private async void OnButtonReleased(object? sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == null) return;

        var task1 = button.ScaleTo(1.0);

        var task2 = button.FadeTo(1.0);

        await Task.WhenAny(task1, task2);
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        _grid = new double[_vm.SizeY, _vm.SizeX];
        canvasView.InvalidateSurface();
    }
}
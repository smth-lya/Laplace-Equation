using LaplaceSOR.Contracts;
using LaplaceSOR.Visualization.Models;
using LaplaceSOR.Visualization.ViewModels;
using SkiaSharp.Views.Maui;
using System.Diagnostics.CodeAnalysis;

namespace LaplaceSOR.Visualization.Pages;

public partial class LaplaceGridPage : ContentPage
{
    private readonly LaplaceGridViewModel _viewModel;

    private GridModel _gridModel;
    private ShapeDrawer _shapeDrawer;
    private SolverLoader _solverLoader;

    private ILaplaceEquationSolver? _solver;

    private CancellationTokenSource _source;
    private CancellationToken _token;

    public LaplaceGridPage(LaplaceGridViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
        _viewModel = vm;

        SetBaseSettings();
        InitializeModels();
    }

    private void SetBaseSettings()
    {
        _viewModel.EpsilonInt = 1;
        _viewModel.EpsilonE = -3;

        _viewModel.Timeslice = 100;

        _viewModel.Omega = 1.5;

        _viewModel.SizeX = 20;
        _viewModel.SizeY = 20;

        _viewModel.NColors = 100;

        _viewModel.DrawingValue = 100;
        _viewModel.BrushSize = _viewModel.SizeX / 4;
    }

    [MemberNotNull(nameof(_gridModel), nameof(_shapeDrawer), nameof(_solverLoader))]
    private void InitializeModels()
    {
        _source = new CancellationTokenSource();
        _token = _source.Token; 

        _gridModel = new GridModel(_viewModel.SizeX, _viewModel.SizeY);
        _shapeDrawer = new ShapeDrawer(_gridModel, _viewModel, _solver);
        _solverLoader = new SolverLoader();

        canvasView.PaintSurface += _shapeDrawer.OnCanvasViewPaintSurface;
        canvasView.EnableTouchEvents = true;
        canvasView.Touch += _shapeDrawer.TouchEventHandler;
    }
    private void DisableModels()
    {
        _source.Cancel();

        canvasView.PaintSurface -= _shapeDrawer.OnCanvasViewPaintSurface;
        canvasView.Touch -= _shapeDrawer.TouchEventHandler;
    }

    private void OnReset(object? sender, EventArgs e)
    {
        DisableModels();
        InitializeModels();
        canvasView.InvalidateSurface();

        _solver?.LoadGrid(_gridModel._grid);
    }

    private async void OnLoadSolverAssembly(object? sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == null) return;

        _solver = await _solverLoader.LoadSolverFromAssemblyAsync();

        SwitchStateOfLoadButton(button, _solver != null);
        StartButton.IsEnabled = _solver != null;

        if (_solver == null) return;

        _solver.LoadGrid(_gridModel._grid);
        _solver.GridUpdated += canvasView.InvalidateSurface;

        DisableModels();
        InitializeModels();

        canvasView.InvalidateSurface();
    }

    private void SwitchStateOfLoadButton(Button button, bool isLoaded)
    {
        if (isLoaded)
        {
            button.Text = "Loaded";
            button.TextColor = Colors.Black;
            button.BackgroundColor = Colors.YellowGreen;
        }
        else
        {
            button.Text = "Select File";
            button.TextColor = Colors.White;
            button.BackgroundColor = Colors.Red;
        }
    }

    private async void OnStartSolving(object? sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == null) return;

        if (button.Text == "Stop")
        {
            await _source.CancelAsync();

            button.BackgroundColor = Colors.YellowGreen;
            button.Text = "Start";

            _source.TryReset();

            return;
        }

        button.BackgroundColor = Colors.Gray;
        button.Text = "Stop";

        await Task.Run(() => _solver?.Solve(omega:_viewModel.Omega, eps:_viewModel.EpsilonInt * Math.Pow(10, _viewModel.EpsilonE), maxIterations: _viewModel.MaxIter, millisecondsTimeout: _viewModel.Timeslice, cancellationToken:_token));

        button.BackgroundColor = Colors.YellowGreen;
        button.Text = "Start";
    }
}


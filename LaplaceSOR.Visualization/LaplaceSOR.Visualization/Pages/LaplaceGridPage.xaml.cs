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
        _viewModel.SizeX = 20;
        _viewModel.SizeY = 20;

        _viewModel.NColors = 100;

        _viewModel.DrawingValue = 100;
        _viewModel.BrushSize = _viewModel.SizeX / 4;
    }

    [MemberNotNull(nameof(_gridModel), nameof(_shapeDrawer), nameof(_solverLoader))]
    private void InitializeModels()
    {
        _gridModel = new GridModel(_viewModel.SizeX, _viewModel.SizeY);
        _shapeDrawer = new ShapeDrawer(_gridModel, _viewModel);
        _solverLoader = new SolverLoader();

        canvasView.PaintSurface += _shapeDrawer.OnCanvasViewPaintSurface;
        canvasView.EnableTouchEvents = true;
        canvasView.Touch += _shapeDrawer.TouchEventHandler;
    }
    private void DisableModels()
    {
        canvasView.PaintSurface -= _shapeDrawer.OnCanvasViewPaintSurface;
        canvasView.Touch -= _shapeDrawer.TouchEventHandler;
    }

    private void OnReset(object sender, EventArgs e)
    {
        DisableModels();
        InitializeModels();
        canvasView.InvalidateSurface();
    }
}


using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaplaceSOR.Contracts;
using LaplaceSOR.Visualization.Models;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Windows.Input;

namespace LaplaceSOR.Visualization.ViewModels
{
    public partial class LaplaceGridViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _timeslice;

        [ObservableProperty]
        private int _epsilonInt;

        [ObservableProperty]
        private int _epsilonE;

        [ObservableProperty]
        private double _omega;

        [ObservableProperty]
        private int _maxIter;

        [ObservableProperty]
        private int _sizeX;

        [ObservableProperty]
        private int _sizeY;

        [ObservableProperty]
        private int _nColors;

        [ObservableProperty]
        private int _drawingValue;

        [ObservableProperty]
        private float _brushSize;

        [ObservableProperty]
        private CellType _cellType; 

        [ObservableProperty]
        private List<CellType> _cellTypes;

        [ObservableProperty]
        private ShapeType _shapeType;

        [ObservableProperty]
        private List<ShapeType> _shapes;
        
        public ICommand ToggleProportionsCommand => new Command(() =>
        {
            ProportionsLocked = !ProportionsLocked;
        });
        
        [ObservableProperty]
        private bool _proportionsLocked;

        public LaplaceGridViewModel()
        {
            _cellTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>().ToList();
            _shapes = Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>().ToList();

            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SizeX) || e.PropertyName == nameof(SizeY))
            {
                if (ProportionsLocked)
                {
                    if (e.PropertyName == nameof(SizeX))
                    {
                        SizeY = SizeX;
                    }
                    else if (e.PropertyName == nameof(SizeY))
                    {
                        SizeX = SizeY;
                    }
                }
            }
        }
    }
}
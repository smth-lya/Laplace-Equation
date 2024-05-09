using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaplaceSOR.Visualization.Pages;

namespace LaplaceSOR.Visualization.ViewModel
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
        private string? _omega;

        [ObservableProperty]
        private string? _maxIter;

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
        private Shape _shapeType;

        [ObservableProperty]
        private List<Shape> _shapes;

        public LaplaceGridViewModel()
        {
            _cellTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>().ToList();

            _shapes = Enum.GetValues(typeof(Shape)).Cast<Shape>().ToList();   

            
        }

        [RelayCommand]
        private void Start()
        {

        }

        [RelayCommand]
        private void Zero()
        {

        }

        [RelayCommand]
        private void Reset()
        {
            
        }

        [RelayCommand]
        private void EmptyText(string s)
        {

        }
    }


}


public enum Shape
{
    Point,
    Wonky
}
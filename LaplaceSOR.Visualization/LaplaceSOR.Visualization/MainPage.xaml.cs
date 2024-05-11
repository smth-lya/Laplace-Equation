using LaplaceSOR.Visualization.ViewModels;

namespace LaplaceSOR.Visualization
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}

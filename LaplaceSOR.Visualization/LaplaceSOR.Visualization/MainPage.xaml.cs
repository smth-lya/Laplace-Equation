using LaplaceSOR.Visualization.ViewModel;

namespace LaplaceSOR.Visualization
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm; 
        }

        private static async void OnButtonPressed(object? sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) return;

            var task1 = button.ScaleTo(0.9, 100, Easing.Linear);

            var task2 = button.FadeTo(0.7, 100, Easing.Linear);
          
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
    }
}

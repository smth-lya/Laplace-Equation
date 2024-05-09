using LaplaceSOR.Visualization.Pages;

namespace LaplaceSOR.Visualization
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LaplaceGridPage), typeof(LaplaceGridPage));
        }
    }
}

﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaplaceSOR.Visualization.Pages;

namespace LaplaceSOR.Visualization.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task Transition()
        {
            await Shell.Current.GoToAsync(nameof(LaplaceGridPage));
        }
    }
}

using LaplaceSOR.Contracts;
using System.Reflection;

namespace LaplaceSOR.Visualization.Models;

public class SolverLoader
{
    public async Task<ILaplaceEquationSolver?> LoadSolverFromAssemblyAsync()
    {
        try
        {
            var assemblyPath = await PickAssemblyFilePathAsync();
            
            if (assemblyPath == null) 
                return null;

            var result = await LoadLaplaceSolverFromAssemblyAsync(assemblyPath);

            return result;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return null;
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

            return result?.FullPath;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return null;
        }
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
            await DisplayAlert("File Not Found", $"The assembly file \"{assemblyPath}\" was not found.", "OK");
            return null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error loading solver from assembly: " + ex.Message, "OK");
            return null;
        }

        var type = assembly.GetTypes().FirstOrDefault(type => type.IsAssignableTo(typeof(ILaplaceEquationSolver)));

        if (type == null)
        {
            await DisplayAlert("Incompatible assembly", "The assembly does not contain a type implementing ILaplaceEquationSolver interface.", "OK");
            return null;
        }

        if (Activator.CreateInstance(type, true) is ILaplaceEquationSolver solverInstance)
        {
            return solverInstance;
        }
        else
        {
            await DisplayAlert("Invalid Operation Exception", "Failed to create instance", "OK");
            return null;
        }
    }
    private async Task DisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}

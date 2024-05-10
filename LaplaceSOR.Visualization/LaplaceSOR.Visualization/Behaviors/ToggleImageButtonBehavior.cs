namespace LaplaceSOR.Visualization.Behaviors;

public class ToggleImageButtonBehavior : Behavior<ImageButton>
{
    public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(ToggleImageButtonBehavior));
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
   
    protected override void OnAttachedTo(ImageButton button)
    {
        button.Clicked += ToggleButtonClicked;

        base.OnAttachedTo(button);
    }
    protected override void OnDetachingFrom(ImageButton button)
    {
        button.Clicked -= ToggleButtonClicked;

        base.OnDetachingFrom(button);
    }

    private async void ToggleButtonClicked(object? sender, EventArgs e)
    {
        var toggleButton = sender as ImageButton;

        if (toggleButton == null) return;

        IsActive = !IsActive;
        
        if (IsActive)
        {
            await toggleButton.ScaleTo(0.9, 50, Easing.Linear);
            await toggleButton.ScaleTo(1, 50, Easing.Linear);

            ((Border)toggleButton.Parent).BackgroundColor = Colors.LightGray;
        }
        else
        {
            ((Border)toggleButton.Parent).BackgroundColor = Colors.White;
        }
    }
}

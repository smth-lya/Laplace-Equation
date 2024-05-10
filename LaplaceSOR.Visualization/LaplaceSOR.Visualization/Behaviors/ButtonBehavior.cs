namespace LaplaceSOR.Visualization.Behaviors
{
    public class ButtonBehavior : Behavior<Button>
    {
        protected override void OnAttachedTo(Button button)
        {
            button.Pressed += OnButtonPressed;
            button.Released += OnButtonReleased;

            base.OnAttachedTo(button);
        }
        protected override void OnDetachingFrom(Button button)
        {
            button.Pressed -= OnButtonPressed;
            button.Released -= OnButtonReleased;

            base.OnDetachingFrom(button);
        }

        private async void OnButtonPressed(object? sender, EventArgs e)
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

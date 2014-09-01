public static class Focus
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value", typeof(bool), typeof(Focus), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

    public static bool GetValue(DependencyObject obj)
    {
        return (bool)obj.GetValue(ValueProperty);
    }

    public static void SetValue(DependencyObject obj, bool value)
    {
        obj.SetValue(ValueProperty, value);
    }

    private static void OnIsFocusedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue)
        {
            var ui = sender as UIElement;
            if (ui != null && !DesignerProperties.GetIsInDesignMode(ui))
            {
                ui.Dispatcher.BeginInvoke(
                    DispatcherPriority.Input, 
                    new ThreadStart(() => Keyboard.Focus(ui)));
            }
        }
    }
}

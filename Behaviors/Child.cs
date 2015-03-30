public class Child
{
    public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
        "Margin", typeof (Thickness?), typeof (Child),
        new PropertyMetadata(default(Thickness?), OnMarginChanged));

    public static void SetMargin(DependencyObject element, Thickness? value)
    {
        element.SetValue(MarginProperty, value);
    }

    public static Thickness? GetMargin(DependencyObject element)
    {
        return (Thickness?) element.GetValue(MarginProperty);
    }

    static void OnMarginChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        var panel = dependencyObject as Panel;
        if (panel == null) return;

        panel.Loaded += PanelOnLoaded;
    }

    static void PanelOnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        var panel = sender as Panel;
        if (panel == null) return;

        panel.Loaded -= PanelOnLoaded;

        foreach (var child in panel.Children.OfType<FrameworkElement>().Where(x => x.GetValue(FrameworkElement.MarginProperty) != null))
        {
            if (GetMargin(panel).HasValue)
                child.SetValue(FrameworkElement.MarginProperty, GetMargin(panel));
            else
                child.SetValue(FrameworkElement.MarginProperty, DependencyProperty.UnsetValue);
        }
    }
}

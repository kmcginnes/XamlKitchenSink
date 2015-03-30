public class Child
{
    public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
        "Margin", typeof(Thickness?), typeof(Child),
        new FrameworkPropertyMetadata(default(Thickness?), FrameworkPropertyMetadataOptions.AffectsMeasure, OnMarginChanged));

    public static void SetMargin(DependencyObject element, Thickness? value)
    {
        element.SetValue(MarginProperty, value);
    }

    public static Thickness? GetMargin(DependencyObject element)
    {
        return (Thickness?)element.GetValue(MarginProperty);
    }

    static void OnMarginChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        var panel = dependencyObject as Panel;
        if (panel == null) return;

        panel.Loaded += PanelOnLoaded;

        var oldValue = (Thickness?) args.OldValue;
        UpdateChildMargin(panel, oldValue);
    }

    static void PanelOnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        var panel = sender as Panel;
        if (panel == null) return;

        panel.Loaded -= PanelOnLoaded;

        UpdateChildMargin(panel);
    }

    static void UpdateChildMargin(Panel panel, Thickness? oldValue = null)
    {
        var childrenWithoutExplicitMargin =
            panel.Children
                .OfType<FrameworkElement>()
                .Where(x => MarginNotSet(x, oldValue));

        var margin = GetMargin(panel) ?? DependencyProperty.UnsetValue;

        foreach (var child in childrenWithoutExplicitMargin)
        {
            child.SetValue(FrameworkElement.MarginProperty, margin);
        }
    }

    static bool MarginNotSet(FrameworkElement child, Thickness? oldValue)
    {
        var childMargin = child.ReadLocalValue(FrameworkElement.MarginProperty) ?? DependencyProperty.UnsetValue;

        return childMargin == DependencyProperty.UnsetValue || childMargin.Equals(oldValue);
    }
}

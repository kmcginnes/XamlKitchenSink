public static class BooleanVisibility
{
    static void OnArgumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Behavior.Update(d);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof (bool), typeof (BooleanVisibility), new PropertyMetadata(true, OnArgumentChanged));

    public static void SetValue(DependencyObject element, bool value)
    {
        element.SetValue(ValueProperty, value);
    }

    public static bool GetValue(DependencyObject element)
    {
        return (bool) element.GetValue(ValueProperty);
    }

    public static readonly DependencyProperty WhenTrueProperty = DependencyProperty.RegisterAttached(
        "WhenTrue", typeof(Visibility), typeof(BooleanVisibility), new PropertyMetadata(Visibility.Visible, OnArgumentChanged));

    public static void SetWhenTrue(DependencyObject element, Visibility value)
    {
        element.SetValue(WhenTrueProperty, value);
    }

    public static Visibility GetWhenTrue(DependencyObject element)
    {
        return (Visibility) element.GetValue(WhenTrueProperty);
    }

    public static readonly DependencyProperty WhenFalseProperty = DependencyProperty.RegisterAttached(
        "WhenFalse", typeof(Visibility), typeof(BooleanVisibility), new PropertyMetadata(Visibility.Collapsed, OnArgumentChanged));

    public static void SetWhenFalse(DependencyObject element, Visibility value)
    {
        element.SetValue(WhenFalseProperty, value);
    }

    public static Visibility GetWhenFalse(DependencyObject element)
    {
        return (Visibility) element.GetValue(WhenFalseProperty);
    }

    private static readonly AttachedBehavior Behavior = 
        AttachedBehavior.Register(host => new BooleanVisibilityBehavior(host));

    private sealed class BooleanVisibilityBehavior : Behavior<UIElement>
    {
        internal BooleanVisibilityBehavior(DependencyObject host) : base(host) { }

        protected override void Update(UIElement host)
        {
            host.Visibility = GetValue(host) ? GetWhenTrue(host) : GetWhenFalse(host);
        }
    }
}

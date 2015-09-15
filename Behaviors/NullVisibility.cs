public static class NullVisibility
{
    static void OnArgumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Behavior.Update(d);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof(object), typeof(NullVisibility), new PropertyMetadata(null, OnArgumentChanged));

    public static void SetValue(DependencyObject element, object value)
    {
        element.SetValue(ValueProperty, value);
    }

    public static object GetValue(DependencyObject element)
    {
        return element.GetValue(ValueProperty);
    }

    public static readonly DependencyProperty WhenNullProperty = DependencyProperty.RegisterAttached(
        "WhenNull", typeof(Visibility), typeof(NullVisibility), new PropertyMetadata(Visibility.Visible, OnArgumentChanged));

    public static void SetWhenNull(DependencyObject element, Visibility value)
    {
        element.SetValue(WhenNullProperty, value);
    }

    public static Visibility GetWhenNull(DependencyObject element)
    {
        return (Visibility)element.GetValue(WhenNullProperty);
    }

    public static readonly DependencyProperty WhenNotNullProperty = DependencyProperty.RegisterAttached(
        "WhenNotNull", typeof(Visibility), typeof(NullVisibility), new PropertyMetadata(Visibility.Collapsed, OnArgumentChanged));

    public static void SetWhenNotNull(DependencyObject element, Visibility value)
    {
        element.SetValue(WhenNotNullProperty, value);
    }

    public static Visibility GetWhenNotNull(DependencyObject element)
    {
        return (Visibility)element.GetValue(WhenNotNullProperty);
    }

    public static readonly DependencyProperty InvertProperty = DependencyProperty.RegisterAttached(
        "Invert", typeof(bool), typeof(NullVisibility), new PropertyMetadata(default(bool), OnArgumentChanged));

    public static void SetInvert(DependencyObject element, bool value)
    {
        element.SetValue(InvertProperty, value);
    }

    public static bool GetInvert(DependencyObject element)
    {
        return (bool)element.GetValue(InvertProperty);
    }

    private static readonly AttachedBehavior Behavior =
        AttachedBehavior.Register(host => new NullVisibilityBehavior(host));

    private sealed class NullVisibilityBehavior : Behavior<UIElement>
    {
        internal NullVisibilityBehavior(DependencyObject host) : base(host) { }

        protected override void Update(UIElement host)
        {

            host.Visibility = GetInvert(host)
                ? IsValueNull(host) ? GetWhenNull(host) : GetWhenNotNull(host)
                : IsValueNull(host) ? GetWhenNotNull(host) : GetWhenNull(host);
        }

        private bool IsValueNull(UIElement host)
        {
            var value = GetValue(host);

            if (value == null)
                return true;

            var stringValue = value as String;
            if (stringValue != null)
            {
                return String.IsNullOrEmpty(stringValue);
            }

            return false;
        }
    }
}

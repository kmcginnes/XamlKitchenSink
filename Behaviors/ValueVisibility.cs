public static class ValueVisibility
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof(object), typeof(ValueVisibility), new PropertyMetadata(OnArgumentsChanged));

    public static readonly DependencyProperty TargetValueProperty = DependencyProperty.RegisterAttached(
        "TargetValue", typeof(object), typeof(ValueVisibility), new PropertyMetadata(OnArgumentsChanged));

    public static readonly DependencyProperty WhenMatchedProperty = DependencyProperty.RegisterAttached(
        "WhenMatched", typeof(Visibility), typeof(ValueVisibility), new FrameworkPropertyMetadata(Visibility.Visible, OnArgumentsChanged));

    public static readonly DependencyProperty WhenNotMatchedProperty = DependencyProperty.RegisterAttached(
        "WhenNotMatched", typeof(Visibility), typeof(ValueVisibility), new FrameworkPropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

    private static readonly AttachedBehavior Behavior = 
        AttachedBehavior.Register(host => new ValueVisibilityBehavior(host));

    private static void OnArgumentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Behavior.Update(d);
    }

    public static object GetValue(UIElement uiElement)
    {
        return uiElement.GetValue(ValueProperty);
    }

    public static void SetValue(UIElement uiElement, object value)
    {
        uiElement.SetValue(ValueProperty, value);
    }

    public static object GetTargetValue(UIElement uiElement)
    {
        return uiElement.GetValue(TargetValueProperty);
    }

    public static void SetTargetValue(UIElement uiElement, object value)
    {
        uiElement.SetValue(TargetValueProperty, value);
    }

    public static Visibility GetWhenMatched(UIElement uiElement)
    {
        return (Visibility)uiElement.GetValue(WhenMatchedProperty);
    }

    public static void SetWhenMatched(UIElement uiElement, Visibility visibility)
    {
        uiElement.SetValue(WhenMatchedProperty, visibility);
    }

    public static Visibility GetWhenNotMatched(UIElement uiElement)
    {
        return (Visibility)uiElement.GetValue(WhenNotMatchedProperty);
    }

    public static void SetWhenNotMatched(UIElement uiElement, Visibility visibility)
    {
        uiElement.SetValue(WhenNotMatchedProperty, visibility);
    }

    private sealed class ValueVisibilityBehavior : Behavior<UIElement>
    {
        internal ValueVisibilityBehavior(DependencyObject host) : base(host) { }

        protected override void Update(UIElement host)
        {
            var value = GetValue(host);
            var targetValue = GetTargetValue(host);

            if (targetValue == null)
            {
                host.Visibility = GetWhenNotMatched(host);
                return;
            }
            
            try
            {
                var convertedTargetValue = Convert.ChangeType(targetValue, value.GetType());

                var whenMatched = GetWhenMatched(host);
                var whenNotMatched = GetWhenNotMatched(host);

                host.Visibility = value.Equals(convertedTargetValue)
                    ? whenMatched
                    : whenNotMatched;
            }
            catch (Exception)
            {
                host.Visibility = GetWhenNotMatched(host);
            }
        }
    }
}

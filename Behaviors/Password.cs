public static class Password
{
    static void OnArgumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Behavior.Update(d);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof(string), typeof(Password), new FrameworkPropertyMetadata(string.Empty, OnArgumentChanged));

    public static void SetValue(DependencyObject element, string value)
    {
        element.SetValue(ValueProperty, value);
    }

    public static string GetValue(DependencyObject element)
    {
        return (string) element.GetValue(ValueProperty);
    }

    private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
        "IsUpdating", typeof (bool), typeof (Password), new PropertyMetadata(default(bool)));

    public static void SetIsUpdating(DependencyObject element, bool value)
    {
        element.SetValue(IsUpdatingProperty, value);
    }

    public static bool GetIsUpdating(DependencyObject element)
    {
        return (bool) element.GetValue(IsUpdatingProperty);
    }

    private static readonly AttachedBehavior Behavior =
        AttachedBehavior.Register(host => new PasswordBehavior(host));

    private sealed class PasswordBehavior : Behavior<PasswordBox>
    {
        internal PasswordBehavior(DependencyObject host) : base(host) { }

        protected override void Attach(PasswordBox host)
        {
            base.Attach(host);
            host.PasswordChanged += PasswordChanged;
        }

        protected override void Detach(PasswordBox host)
        {
            base.Detach(host);
            host.PasswordChanged -= PasswordChanged;
        }

        void PasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var passwordBox = (PasswordBox) sender;
            SetIsUpdating(passwordBox, true);
            SetValue(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        protected override void Update(PasswordBox host)
        {
            Detach(host);

            if (!GetIsUpdating(host))
            {
                host.Password = GetValue(host);
            }
            Attach(host);
        }
    }
}

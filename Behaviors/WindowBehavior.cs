public class WindowBehavior
{
    static readonly Type OwnerType = typeof (WindowBehavior);

    public static readonly DependencyProperty HideCloseButtonProperty =
        DependencyProperty.RegisterAttached("HideCloseButton", typeof (bool),
            OwnerType, new FrameworkPropertyMetadata(false, 
                new PropertyChangedCallback(HideCloseButtonChangedCallback)));

    [AttachedPropertyBrowsableForType(typeof (Window))]
    public static bool GetHideCloseButton(Window obj)
    {
        return (bool) obj.GetValue(HideCloseButtonProperty);
    }

    [AttachedPropertyBrowsableForType(typeof (Window))]
    public static void SetHideCloseButton(Window obj, bool value)
    {
        obj.SetValue(HideCloseButtonProperty, value);
    }

    static void HideCloseButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = d as Window;
        if (window == null) return;

        var hideCloseButton = (bool) e.NewValue;
        if (hideCloseButton && !GetIsHiddenCloseButton(window))
        {
            if (!window.IsLoaded)
            {
                window.Loaded += HideWhenLoadedDelegate;
            }
            else
            {
                HideCloseButton(window);
            }
            SetIsHiddenCloseButton(window, true);
        }
        else if (!hideCloseButton && GetIsHiddenCloseButton(window))
        {
            if (!window.IsLoaded)
            {
                window.Loaded -= ShowWhenLoadedDelegate;
            }
            else
            {
                ShowCloseButton(window);
            }
            SetIsHiddenCloseButton(window, false);
        }
    }

    const int GWL_STYLE = -16;
    const int WS_SYSMENU = 0x80000;

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    static readonly RoutedEventHandler HideWhenLoadedDelegate = (sender, args) =>
    {
        if (sender is Window == false) return;
        var w = (Window) sender;
        HideCloseButton(w);
        w.Loaded -= HideWhenLoadedDelegate;
    };

    static readonly RoutedEventHandler ShowWhenLoadedDelegate = (sender, args) =>
    {
        if (sender is Window == false) return;
        var w = (Window) sender;
        HideCloseButton(w);
        w.Loaded -= ShowWhenLoadedDelegate;
    };

    static void HideCloseButton(Window w)
    {
        var hwnd = new WindowInteropHelper(w).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
    }

    static void ShowCloseButton(Window w)
    {
        var hwnd = new WindowInteropHelper(w).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) | WS_SYSMENU);
    }

    static readonly DependencyPropertyKey IsHiddenCloseButtonKey =
        DependencyProperty.RegisterAttachedReadOnly(
            "IsHiddenCloseButton",
            typeof (bool),
            OwnerType,
            new FrameworkPropertyMetadata(false));

    public static readonly DependencyProperty IsHiddenCloseButtonProperty =
        IsHiddenCloseButtonKey.DependencyProperty;

    [AttachedPropertyBrowsableForType(typeof (Window))]
    public static bool GetIsHiddenCloseButton(Window obj)
    {
        return (bool) obj.GetValue(IsHiddenCloseButtonProperty);
    }

    static void SetIsHiddenCloseButton(Window obj, bool value)
    {
        obj.SetValue(IsHiddenCloseButtonKey, value);
    }
}

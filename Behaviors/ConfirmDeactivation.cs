public static class ConfirmDeactivation
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof (bool), typeof (ConfirmDeactivationBehavior),
            new UIPropertyMetadata(default(bool), new PropertyChangedCallback(OnIsEnabledChanged)));

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var selector = d as Selector;
        if (selector != null)
        {
            var view = CollectionViewSource.GetDefaultView(selector.Items.SourceCollection);
            if ((bool) e.NewValue)
            {
                selector.IsSynchronizedWithCurrentItem = true;
                view.CurrentChanging += view_CurrentChanging;
            }
            else
            {
                view.CurrentChanging -= view_CurrentChanging;
            }
        }
    }

    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool) element.GetValue(IsEnabledProperty);
    }

    static void view_CurrentChanging(object sender, CurrentChangingEventArgs e)
    {
        var items = (ItemCollection)sender;

        if (e.IsCancelable && items.CurrentItem != null)
        {
            var currentView = items.CurrentItem as FrameworkElement;
            if (currentView == null) return;
            var selector = FindVisualParent<Selector>(currentView);
            if (selector == null) return;

            var vetoingSource = currentView as IConfirmDeactivate ?? currentView.DataContext as IConfirmDeactivate;
            
            if (vetoingSource != null && !vetoingSource.ConfirmDeactivation())
            {
                e.Cancel = true;
            }

            selector.Dispatcher.BeginInvoke(new Action(items.Refresh));
        }
    }

    public static T FindVisualParent<T>(DependencyObject child)
      where T : DependencyObject
    {
        // get parent item
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        // we’ve reached the end of the tree
        if (parentObject == null) return null;

        // check if the parent matches the type we’re looking for
        T parent = parentObject as T;
        if (parent != null)
        {
            return parent;
        }
        else
        {
            // use recursion to proceed with next level
            return FindVisualParent<T>(parentObject);
        }
    }
}

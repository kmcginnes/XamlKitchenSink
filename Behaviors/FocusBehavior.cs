using System.Windows;

namespace WatchGuard.Elx.Modules.Shared
{

    // Sourced from: http://stackoverflow.com/questions/1356045/set-focus-on-textbox-in-wpf-from-view-model-c-wpf/1356781#1356781
    public static class FocusBehavior
    {

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
                "IsFocused", typeof(bool), typeof(FocusBehavior), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }


        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = (UIElement)d;
            if ((bool)e.NewValue)
            {
                uiElement.Focus(); // Don't care about false values.
            }
        }
    }
}

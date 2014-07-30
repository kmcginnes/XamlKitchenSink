public class ValueToVisibilityConverter : IValueConverter
{
    public ValueToVisibilityConverter()
    {
        TrueValue = Visibility.Visible;
        FalseValue = Visibility.Collapsed;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueToCompareTo = parameter ?? Value;
        var convertedValue = System.Convert.ChangeType(valueToCompareTo, value.GetType());
        return Equals(value, convertedValue)
            ? TrueValue
            : FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public Visibility TrueValue { get; set; }
    public Visibility FalseValue { get; set; }
    public object Value { get; set; }
}

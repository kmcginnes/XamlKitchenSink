public abstract class NotifyingObjectBase : INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <summary>
    /// RaiseAndSetIfChanged fully implements a Setter for a read-write
    /// property on a ReactiveObject, using CallerMemberName to raise the notification
    /// and the ref to the backing field to set the property.
    /// </summary>
    /// <typeparam name="TObj">The type of the This.</typeparam>
    /// <typeparam name="TRet">The type of the return value.</typeparam>
    /// <param name="this">The <see cref="ReactiveObject"/> raising the notification.</param>
    /// <param name="backingField">A Reference to the backing field for this
    /// property.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="propertyName">The name of the property, usually
    /// automatically provided through the CallerMemberName attribute.</param>
    /// <returns>The newly set value, normally discarded.</returns>
    protected TRet RaiseAndSetIfChanged<TRet>(
        ref TRet backingField,
        TRet newValue,
        [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
        {
            return newValue;
        }
        RaisingPropertyChanging(propertyName);
        backingField = newValue;
        RaisingPropertyChanged(propertyName);
        return newValue;
    }
    private void RaisingPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    private void RaisingPropertyChanging(string propertyName) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;
}
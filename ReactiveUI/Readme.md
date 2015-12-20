# ReactiveUI Tips & Tricks

## Use RxUI Bindings

There's a huge advantage to taking binding code out of the XAML and putting it in the code behind. It's still just as untestable, but you gain some fantastic advantages:

* XAML is cleaner and easier to read
* Bindings are checked by the compiler (less of a problem with ReSharper
* Content can be set in the XAML and overriden by bindings (great for design time data)
* Converters are much simpler to implement

## Code Behind

The documenation for this is extremely poor. This is what you'll need. I suggest using a ReSharper template for the properties. It's a huge pain to set those up each time.

```c#
public partial class PlaybackView : UserControl, IViewFor<PlaybackViewModel>
{
  public PlaybackViewModel ViewModel
  {
    get { return (PlaybackViewModel)GetValue(ViewModelProperty); }
    set { SetValue(ViewModelProperty, value); }
  }
  
  public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register("ViewModel", typeof(PlaybackViewModel), typeof(PlaybackView), new PropertyMetadata(null));

  object IViewFor.ViewModel
  {
    get { return ViewModel; }
    set { ViewModel = (PlaybackViewModel)value; }
  }
}
```

I've also seen a base class used. It requires a bit more boilerplate in some areas, and less in others. I'm not sure which way I like better.

```c#
public interface IView
{
    object ViewModel { get; set; }
    IObservable<Unit> Done { get; }
    IObservable<Unit> Cancel { get; }
    IObservable<bool> IsBusy { get; }
}
```

```c#
/// <summary>
/// Base class for all of our user controls. This one does not import GitHub resource/styles and is used by the
/// publish control.
/// </summary>
public class SimpleViewUserControl : UserControl, IDisposable, IActivatable
{
    readonly Subject<Unit> _close = new Subject<Unit>();
    readonly Subject<Unit> _cancel = new Subject<Unit>();
    readonly Subject<bool> _isBusy = new Subject<bool>();

    public SimpleViewUserControl()
    {
        this.WhenActivated(d =>
        {
            d(this.Events()
                .KeyUp
                .Where(x => x.Key == Key.Escape && !x.Handled)
                .Subscribe(key =>
                {
                    key.Handled = true;
                    NotifyCancel();
                }));
        });
    }

    public IObservable<Unit> Done
    {
        get { return _close; }
    }

    public IObservable<Unit> Cancel
    {
        get { return _cancel; }
    }

    public IObservable<bool> IsBusy
    {
        get { return _isBusy; }
    }

    protected void NotifyDone()
    {
        _close.OnNext(Unit.Default);
        _close.OnCompleted();
    }

    protected void NotifyCancel()
    {
        _cancel.OnNext(Unit.Default);
        _cancel.OnCompleted();
    }

    protected void NotifyIsBusy(bool busy)
    {
        _isBusy.OnNext(busy);
    }

    bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_disposed) return;

            _close.Dispose();
            _cancel.Dispose();
            _isBusy.Dispose();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class SimpleViewUserControl<TViewModel, TImplementor> : SimpleViewUserControl, IViewFor<TViewModel>, IView
    where TViewModel : class where TImplementor : class
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        "ViewModel", typeof(TViewModel), typeof(TImplementor), new PropertyMetadata(null));

    object IViewFor.ViewModel
    {
        get { return ViewModel; }
        set { ViewModel = (TViewModel)value; }
    }

    object IView.ViewModel
    {
        get { return ViewModel; }
        set { ViewModel = (TViewModel)value; }
    }

    public TViewModel ViewModel
    {
        get { return (TViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }

    TViewModel IViewFor<TViewModel>.ViewModel
    {
        get { return ViewModel; }
        set { ViewModel = value; }
    }
}
```

And then to use it on a view you would need to implement a base class that defines the generic, and use that instead of `UserControl` in the XAML.

```c#
public class GenericPlaybackView : SimpleViewUserControl<PlaybackViewModel, GenericPlaybackView>

public partial class PlaybackView : GenericPlaybackView
{
}
```

```xaml
<local:GenericPlaybackView x:Class="WatchGuard.Elx.Playback.PlaybackView"
    ...>
</local:GenericPlaybackView>
```

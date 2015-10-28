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

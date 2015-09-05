# ReactiveUI Tips & Tricks

## Use RxUI Bindings

There's a huge advantage to taking binding code out of the XAML and putting it in the code behind. It's still just as untestable, but you gain some fantastic advantages:

* XAML is cleaner and easier to read
* Bindings are checked by the compiler (less of a problem with ReSharper
* Content can be set in the XAML and overriden by bindings (great for design time data)
* Converters are much simpler to implement

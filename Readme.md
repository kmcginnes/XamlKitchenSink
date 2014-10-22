XAML Kitchen Sink
=================

This is a github repository I started as a dumping ground for all the usefull chunks of code for XAML, view models, etc. This is mostly focused on WPF, but could be applicable to Silverlight, Windows Store Apps, Windows Phone 7/8 apps, etc.

SpicyTaco.AutoGrid
------------------
When you get fed up typing out `Grid`'s verbose `ColumnDefinitions` checkout [SpicyTaco.AutoGrid](https://github.com/kmcginnes/SpicyTaco.AutoGrid). There are many solutions to this problem. This is mine.

In addition to column and row definitions, it also offers `ChildMargin`, `ChildHorizontalAlignment`, and `ChildVerticalAlignment`.

Other Projects
--------------
[WPF Crutches](https://bitbucket.org/rstarkov/wpfcrutches) was started many years ago, but has some fantastic tools. [WPF Bag of Tricks](https://github.com/thinkpixellab/bot) is another great one.

If you need to support animated GIFs in your project, then look no further than [WpfAnimatedGif](https://github.com/thomaslevesque/WpfAnimatedGif) available on NuGet.

If you want an iOS inspired toggle switch, check out [Toggle Switch Control Library](https://github.com/ejensen/toggle-switch-control).

Looking for spinners, checkout [Xaml Spinner Kit](https://github.com/nigel-sampson/spinkit-xaml) or [Xaml Spinners](https://github.com/blackspikeltd/Xaml-Spinners-WPF).

If you want a great source of inspiration for attached behaviors, then look to [this great series](http://www.executableintent.com/attached-behaviors-part-1-booleanvisibility/) by Bryan Watts. In it he gives alternatives to the common converter. I've since created many useful behaviors based on his work that you can find in the behaviors folder.

Since incorporating ReactiveUI I've been rethinking a lot of interaction logic in my view models. You can see one example of how to bubble commands up from the child in a collection to its parent in my [proof of concept](https://github.com/kmcginnes/PoC.ReactiveCommandBubbling).

Links
-----
[Matrial Design Shadows](http://marcangers.com/material-design-shadows-in-wpf/) - Marc Angers did an excellent job reproducing the beautiful shadows from Google's Material Design guidelines.

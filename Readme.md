XAML Kitchen Sink
=================

This is a github repository I started as a dumping ground for all the usefull chunks of code for XAML, view models, etc. This is mostly focused on WPF, but could be applicable to Silverlight, Windows Store Apps, Windows Phone 7/8 apps, etc.

Other Projects
--------------
[WPF Crutches](https://bitbucket.org/rstarkov/wpfcrutches) was started many years ago, but has some fantastic tools.

If you want a great source of inspiration for attached behaviors, then look to [this great series](http://www.executableintent.com/attached-behaviors-part-1-booleanvisibility/) by Bryan Watts. In it he gives alternatives to the common converter. I've since created many useful behaviors based on his work that you can find in the behaviors folder.

Since incorporating ReactiveUI I've been rethinking a lot of interaction logic in my view models. You can see one example of how to bubble commands up from the child in a collection to its parent in my [proof of concept](https://github.com/kmcginnes/PoC.ReactiveCommandBubbling).

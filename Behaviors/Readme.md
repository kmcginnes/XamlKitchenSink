Behaviors
=========

Behaviors extend the functionality of existing WPF controls. They are useful for so  many things. Here's just a few.


FocusBehavior
-------------

This behavior allows you to control the keyboard focus of an element through bindings.

```XML
<TextBox x:Name="FirstName" Text="{Binding FirstName}"
         behaviors:FocusBehavior.IsFocused="{Binding IsFirstNameFocused}"/>
```

For example, suppose you have an `ItemsControl`. If you add

```CSharp
Loaded += (sender, args) => FirstName.Focus();
```

in the child view then it will set focus when you add items. However, it will also set focus when the view loads. This is undesirable if you want another element to have focus when the view loads.


BooleanVisibility
-----------------

Everyone has used the BooleanToVisibilityConverter. It is used so much that Microsoft added it to the BCL. But it is clunky to use. The following is the simplest example:

```XML
<UserControl.Resources>
  <BooleanToVislibilityConverter x:Key="BooleanToVislibilityConverter" />
</UserControl.Resources>

<TextBlock Text="I'm Batman!"
           Visibility="{Binding IsTheCapedCrusader, Converter={StaticResource BooleanToVisibilityConverter}}"/>
```

There's a better way. Using attached behaviors. All credit for this code goes to Bryan Watts for his [series on behaviors](http://www.executableintent.com/attached-behaviors-part-1-booleanvisibility/)

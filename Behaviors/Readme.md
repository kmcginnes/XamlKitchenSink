Behaviors
=========

Behaviors extend the functionality of existing WPF controls. They are useful for so  many things. Here's just a few.


Password
--------

The Password behavior allows binding of the plain text password from a PasswordBox. This isn't recomended for security reasons, but it's here if you want it.

```XML
<PasswordBox behaviors:Password.Value="{Binding Password, Mode=TwoWay}"/>
```

__Note:__ The `Mode=TwoWay` is required.

You've probably seen other attached behaviors doing the same thing. Most of those require two properties. One to turn on the password monitoring, the other to bind the value. I've reduced that down to one property; the binding.


Focus
-----

This behavior allows you to control the keyboard focus of an element through bindings.

```XML
<TextBox x:Name="FirstName" Text="{Binding FirstName}"
         behaviors:Focus.Value="{Binding IsFirstNameFocused}"/>
```

For example, suppose you have an `ItemsControl`. If you add

```CSharp
Loaded += (sender, args) => FirstName.Focus();
```

in the child view then it will set focus when you add items. However, it will also set focus when the view loads. This is undesirable if you want another element to have focus when the view loads.

Child
-----

This behavior is a dumping ground of sorts for all those behaviors you whish panels had for affecting all their children. If a child element already has a margin then this behavior will not affect it.

```XML
<StackPanel behaviors:Child.Margin="5">
  <Label Content="First Name:"/>
  <TextBox />
</StackPanel>
```

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

There's a better way. Using attached behaviors. All credit for this code goes to Bryan Watts for his [series on behaviors](http://www.executableintent.com/attached-behaviors-part-1-booleanvisibility/).

No static resource is required. We just bind our boolean value to the attached property and let WPF do all the heavy lifting.

```XML
<TextBlock Text="I'm Batman!"
           behaviors:BooleanVisibility.Value="{Binding IsTheCapedCrusader}"/>
```

That's the simplest scenario. What if we want to collapse the element when the value is true? Normally you'd have to create a custom converter that inverted the bool, or took the visiblity value to use for true/false values. It's similar here, just cleaner.

```XML
<TextBlock Text="Joker is here"
           behaviors:BooleanVisibility.Value="{Binding IsTheCapedCrusader}"
           behaviors:BooleanVisibility.WhenTrue="Collapsed"
           behaviors:BooleanVisibility.WhenFalse="Visible"/>
```


NullVisibility
--------------

Similar to BooleanVisibility, but for null checks.

```XML
<TextBlock Text="Joker is here"
           behaviors:NullVisibility.Value="{Binding IsTheCapedCrusader}"
           behaviors:NullVisibility.WhenNull="Collapsed"
           behaviors:NullVisibility.WhenNotNull="Visible"/>
```


ValueVisibility
--------------

Similar to BooleanVisibility, but for any value. Think enums, strings, numbers, etc.

```XML
<TextBlock Text="Batmobile is fully armored"
           behaviors:ValueVisibility.Value="{Binding BatmobileMode}"
           behaviors:ValueVisibility.TargetValue="{x:Static enums:BatmobileMode.Armored}"
           behaviors:ValueVisibility.WhenMatched="Visible"
           behaviors:ValueVisibility.WhenNotMatched="Collapsed"/>
```

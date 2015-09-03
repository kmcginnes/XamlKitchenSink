# Best Practices

## Declare all ResourceDictionaries in App.xaml

There's very little performance advantage to putting resource dictionary declarations in each XAML view. Just define them in App.xaml and save yourself a lot of headache.

## Use compile time safe binding

Either add a designer instance in the XAML to tell ReSharper what view model the view is bound to, or use RxUI binding in the code behind (preferred).

## Don't put views or resources in separate assemblies

Usually you would do this for some sort of reuse. This is very rarely a good idea for UI eleemnts. Copy and paste is acceptable here, because more often than not you will need to tweak those UI elements. The biggest disadvantage is that any resource defined in another assembly will not be rendered in the designer.

## Create T4 template for XAML color definitions

At some point I'll create a T4 template that will make defining XAML colors much simpler, because out of the box it's a huge pain.

Something like:

Primary: #0275d8

into:

<Color x:Key="Primary">#0275d8</Color>
<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource Primary}"/>

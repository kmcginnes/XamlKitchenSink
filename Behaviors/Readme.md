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

Converters
==========

This section contains converters that I've found useful.

Value To Visibility
-------------------

Defined in resources as:

``` XML
<converters:ValueToVisibilityConverter x:Key="ValueToVisibility"/>
```

Sometimes you just want to make an element visibile when some value is equal to a constant:

``` XML
<Button Content="Delete"
        Visibility="{Binding Path=Id, 
                             Converter={StaticResource ValueToVisibility}, 
                             ConverterParameter=1}"/>
```

You can define that constant at the converters declaration time:

``` XML
<converters:ValueToVisibilityConverter x:Key="VisibleWhenOne" Value="1"/>
```

Sometimes you want the element to collapse or hide whenever the value bound is equal to your constant:

``` XML
<converters:ValueToVisibilityConverter x:Key="CollapsedWhenOne" Value="1" TrueValue="Collapsed" FalseValue="Visible"/>
```

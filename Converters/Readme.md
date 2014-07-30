Converters
==========

This section contains converters that I've found useful.

Value To Visibility
-------------------

Sometimes you just want to make an element visibile when some value is equal to a constant:

``` XML
<Button Content="Delete"
        Visibility="{Binding Path=Id, 
                        Converter={StaticResource ValueToVisibility}, 
               ConverterParameter=1}"/>
```

Designer View Models
====================

Getting mock data to show in the Visual Studio designer when following the MVVM pattern has always been a difficult thing for me.

Now, the solution seems so simple.

The Problem
-----------

Three letters: __I O C__

The typical way you would load data into your design time views would look like this:

```C#
public class PersonViewModel
{
  public PersonViewModel()
  {
    if(InDesignMode)
    {
      FirstName = "Kris";
      LastName = "McGinnes";
    }
  }
  
  // snip ...
}
```

```XML
<UserControl x:Class="Example.PersonView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             xmlns:vm="clr-namespace:Example.ViewModels"
             d:DataContext="{d:DesignInstance Type={x:Type vm:PersonViewModel}, IsDesignTimeCreatable=True}"
             cal:Bind.AtDesignTime="True">
  <!-- snip ... -->
</UserControl>
```

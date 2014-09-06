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
    if(Execute.InDesignMode)
    {
      FirstName = "Kris";
      LastName = "McGinnes";
    }
  }
  
  // snip ...
}
```

In your constructor you would determine if you were in design mode, and if so set some values.

Then in your XAML

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

You would set your designer data context to your view model's type and tell the designer to create it.

But the designer only works on default constructors. This means that if you are using constructor injection, you must create a parameterless constructor in addition to your dependency injected constructor.

```C#
public class PersonViewModel
{
  public PersonViewModel() : this(null) { }
  
  public PersonViewModel(ISomeDependency someDependency))
  {
    _someDependency = someDependency;
    
    if(Execute.InDesignMode)
    {
      FirstName = "Kris";
      LastName = "McGinnes";
    }
  }
  
  // snip ...
}
```

This is already getting ugly, and this is a very simple scenario. You can see where this might be headed.

* What if you use `someDependency` inside your constructor?
  * Do you wrap it with an `if(!Execute.InDesignMode)`?
  * Do you pass a real value rather than `null` from the parameterless constructor?
* What's to stop someone from using the parameterless constructor instead of the dependency injected one?


The Solution
------------

Perhaps this answer was out on the interwebs somewhere, but I wasn't finding it. While searching, I did stumble upon the building blocks of this idea.

I knew at some level I was going to need a __view model locator__. This would allow the user of a service locator to build up our view models using the dependency injected constructor.

```C#
public class DesignerViewModelLocator
{
    public PersonViewModel PersonViewModel { get { return IoC.Get<PersonViewModel>(); } }
    // snip ...
}
```

This is a simple class that has a getter only property that returns the result of a service locator call to get our view model.

Now, in order to use this locator, we must get an instance to it loaded in the designer. We do this in the App.xaml

```XML
<Application x:Class="WatchGuard.Elx.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:Example">
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <local:DesignerViewModelLocator x:Key="DesignerLocator"/>
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

Now we can use this instance of the locator inside our views and bind the designer data context to the properties on that instance.

```XML
<UserControl x:Class="Example.PersonView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             d:DataContext="{Binding Source={StaticResource DesignerLocator}, Path=PersonViewModel}"
             cal:Bind.AtDesignTime="True">
  <!-- snip ... -->
</UserControl>
```

We're able to remove the namespace of the view model's type. We also continue to get ReSharper's intellisense help in the XAML. The `DesignerLocator` instance is created always, but properties are only accessed at design time. So the cost is very minimal.

All in all, a very elegant solution.

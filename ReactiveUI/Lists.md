# ReactiveUI - Dealing With Lists

ReactiveUI can handle lists of views or items very easily, but there are some nuances that are not obvious.

## ItemsControl

```c#
public class CastViewModel : ReactiveObject
{
  public CastViewModel()
  {
    Characters = new ReactiveList<CharacterViewModel>();
  }
}
```

```xaml
<ItemsControl x:Name="Characters"/>
```

```c#
```

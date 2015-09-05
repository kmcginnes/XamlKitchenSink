Bubble Commands
===============

So often in MVVM do we run across the situation where, in a list of elements we must perform an operation involving both the single element and the list itself.

The cannonical example is the delete button on each element in the list. The actionable item exists on the singular element, but the action must manipulate the list as a whole.

There are many ways to solve this problem: Caliburn Actions, RelativeSource Ancestor, event aggregation, etc.

Here is another. This involves ReactiveUI's ReactiveCommand.

The goal is to put the command on the singular element, but allow the view model containing the list to control the list manipulation.

ReactiveCommand is both a typical command, and something completely different. It has an execute and a can execute set of operations. But it implements them in a very unique way.

CanExecute is a stream of events of type bool. In other words: `Observable<bool>`. 


Solution
--------

Child view model with command declaration and can execute.

```
public class PersonViewModel : ReactiveObject
{
    public PersonViewModel(string first, string last)
    {
        FirstName = first;
        LastName = last;

        Delete = ReactiveCommand.Create(Observable.Return(true));
    }

    public ReactiveCommand<object> Delete { get; private set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

Parent view model with command subscription. We use `ActOnEveryObject` extension method to ensure that as new items are added to the collection, the delete command gets wired up.

```
public class MainViewModel : ReactiveObject
{
    public MainViewModel()
    {
        People = new ReactiveList<PersonViewModel>();
        People.ActOnEveryObject(x => x.Delete.Subscribe(p => People.Remove((PersonViewModel) p)), _ => { });
        People.AddRange(new[]
        {
            new PersonViewModel("Jerry", "Seinfeld"),
            new PersonViewModel("George", "Costanza"),
            new PersonViewModel("Elaine", "Something"),
            new PersonViewModel("Newman", "Something"),
        });
    }

    public ReactiveList<PersonViewModel> People { get; private set; }
}
```



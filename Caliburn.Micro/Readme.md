# Caliburn.Micro

The order of operations for events is hard to remember, so here it is for reference

```
ShellViewModel ctor
ShellView ctor
ShellView Initialized
ShellView after InitializeComponent
ShellViewModel OnViewAttached
ShellViewModel OnInitialize
ShellViewModel OnActivate
ShellViewModel OnViewReady
ShellView Activated
ShellView Loaded
ShellViewModel OnViewLoaded
ShellView ContentRendered
```

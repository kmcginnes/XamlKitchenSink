public class AttachedBehavior
{
    public static AttachedBehavior Register(Func<DependencyObject, IBehavior> behaviorFactory)
    {
        return new AttachedBehavior(RegisterProperty(), behaviorFactory);
    }

    private readonly DependencyProperty _property;
    private readonly Func<DependencyObject, IBehavior> _behaviorFactory;

    public AttachedBehavior(DependencyProperty property, Func<DependencyObject, IBehavior> behaviorFactory)
    {
        _property = property;
        _behaviorFactory = behaviorFactory;
    }

    private static DependencyProperty RegisterProperty()
    {
        return DependencyProperty.RegisterAttached(
            GetPropertyName(),
            typeof(IBehavior),
            typeof(AttachedBehavior));
    }

    private static string GetPropertyName()
    {
        return "_" + Guid.NewGuid().ToString("N");
    }

    public void Update(DependencyObject host)
    {
        var behavior = (IBehavior)host.GetValue(_property);

        if (behavior == null)
        {
            TryCreateBehavior(host);
        }
        else
        {
            UpdateBehavior(host, behavior);
        }
    }

    private void TryCreateBehavior(DependencyObject host)
    {
        var behavior = _behaviorFactory(host);

        if (behavior.IsApplicable())
        {
            behavior.Attach();

            host.SetValue(_property, behavior);

            behavior.Update();
        }
    }

    private void UpdateBehavior(DependencyObject host, IBehavior behavior)
    {
        if (behavior.IsApplicable())
        {
            behavior.Update();
        }
        else
        {
            host.ClearValue(_property);

            behavior.Detach();
        }
    }
}

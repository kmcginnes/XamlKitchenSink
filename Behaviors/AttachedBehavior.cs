public interface IBehavior
{
    bool IsApplicable();
    void Attach();
    void Update();
    void Detach();
}

public abstract class Behavior<THost> : IBehavior where THost : DependencyObject
{
    private readonly WeakReference _hostReference;

    protected Behavior(DependencyObject host)
    {
        if (!(host is THost))
        {
            throw new ArgumentException("Host is not the expected type", "host");
        }

        _hostReference = new WeakReference(host);
    }

    private THost GetHost()
    {
        return (THost)_hostReference.Target;
    }

    protected virtual bool IsApplicable(THost host)
    {
        return true;
    }

    protected virtual void Attach(THost host) { }
    protected virtual void Detach(THost host) { }
    protected abstract void Update(THost host);

    bool IBehavior.IsApplicable()
    {
        var host = GetHost();

        return host != null && IsApplicable(host);
    }

    void IBehavior.Attach()
    {
        var host = GetHost();

        if (host != null)
        {
            Attach(host);
        }
    }

    void IBehavior.Update()
    {
        var host = GetHost();

        if (host != null)
        {
            Update(host);
        }
    }

    void IBehavior.Detach()
    {
        var host = GetHost();

        if (host != null)
        {
            Detach(host);
        }
    }
}

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

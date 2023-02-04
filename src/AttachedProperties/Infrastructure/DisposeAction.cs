namespace AttachedProperties.Infrastructure;

internal sealed class DisposeAction : IDisposable
{
    private readonly Action _disposeAction;

    public DisposeAction(Action disposeAction)
    {
        GuardAgainst.Null(disposeAction);

        _disposeAction = disposeAction;
    }

    public void Dispose()
    {
        _disposeAction();
    }
}

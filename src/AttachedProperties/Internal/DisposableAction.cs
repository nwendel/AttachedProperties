namespace AttachedProperties.Internal;

internal sealed class DisposableAction : IDisposable
{
    private readonly Action _disposableAction;

    internal DisposableAction(Action action, Action disposableAction)
    {
        action();
        _disposableAction = disposableAction;
    }

    public void Dispose()
    {
        _disposableAction();
    }
}

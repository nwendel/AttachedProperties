namespace AttachedProperties.Internal;

public class AttachedPropertyStore
{
    private readonly ConcurrentDictionary<AttachedPropertyBase, object?> _values = new();

    public int Count => _values.Count;

    public IReadOnlyDictionary<AttachedPropertyBase, object?> Values => new ReadOnlyDictionary<AttachedPropertyBase, object?>(_values);

    public bool TryGetValue(AttachedPropertyBase attachedProperty, [NotNullWhen(true)] out object? value)
    {
        GuardAgainst.Null(attachedProperty);

        var found = _values.TryGetValue(attachedProperty, out value);
        return found;
    }

    public void SetValue(AttachedPropertyBase attachedProperty, object? value)
    {
        GuardAgainst.Null(attachedProperty);

        _values[attachedProperty] = value;
    }

    public void RemoveValue(AttachedPropertyBase attachedProperty)
    {
        GuardAgainst.Null(attachedProperty);

        _values.TryRemove(attachedProperty, out var _);
    }
}

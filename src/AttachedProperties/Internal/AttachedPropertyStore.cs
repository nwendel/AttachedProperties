using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AttachedProperties.Infrastructure;

namespace AttachedProperties.Internal;

public class AttachedPropertyStore
{
    private readonly ConcurrentDictionary<AbstractAttachedProperty, object?> _values = new();

    public int Count => _values.Count;

    public IReadOnlyDictionary<AbstractAttachedProperty, object?> Values => new ReadOnlyDictionary<AbstractAttachedProperty, object?>(_values);

    public bool TryGetValue(AbstractAttachedProperty attachedProperty, [NotNullWhen(true)] out object? value)
    {
        GuardAgainst.Null(attachedProperty);

        var found = _values.TryGetValue(attachedProperty, out value);
        return found;
    }

    public void SetValue(AbstractAttachedProperty attachedProperty, object? value)
    {
        GuardAgainst.Null(attachedProperty);

        _values[attachedProperty] = value;
    }

    public void RemoveValue(AbstractAttachedProperty attachedProperty)
    {
        GuardAgainst.Null(attachedProperty);

        _values.TryRemove(attachedProperty, out var _);
    }
}

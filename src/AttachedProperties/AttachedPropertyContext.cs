using System.Runtime.CompilerServices;
using AttachedProperties.Internal;

namespace AttachedProperties;

/// <summary>
/// The context in which attached properties are defined and which is also used to set and
/// get attached property values.
/// </summary>
public sealed class AttachedPropertyContext : IDisposable
{
    /// <summary>
    /// An instance of the global context.
    /// </summary>
    public static readonly AttachedPropertyContext GlobalContext = new();

    private readonly ReaderWriterLockSlim _lock = new();
    private readonly HashSet<AttachedPropertyBase> _attachedProperties = new();
    private readonly ConditionalWeakTable<object, AttachedPropertyStore> _stores = new();

#if false
    private static readonly PropertyInfo _keysProperty = typeof(ConditionalWeakTable<object, AttachedPropertyStore>).GetTypeInfo().GetDeclaredProperty("Keys");
#endif

    /// <summary>
    /// Register a new attached property in this context.
    /// </summary>
    public void Register(AttachedPropertyBase attachedProperty)
    {
        GuardAgainst.Null(attachedProperty);

        using (WriteLockScope())
        {
            var fullName = attachedProperty.FullName;
            if (_attachedProperties.Select(x => x.FullName).Contains(fullName))
            {
                throw new AttachedPropertyException($"Attached property named {attachedProperty.Name} for type {attachedProperty.OwnerType.FullName} is already registered in current context");
            }

            _attachedProperties.Add(attachedProperty);
        }
    }

    /// <summary>
    /// Get a collection of all defined attached properties in this context.
    /// </summary>
    public IReadOnlyCollection<AttachedPropertyBase> GetProperties()
    {
        using (ReadLockScope())
        {
            return new ReadOnlyCollection<AttachedPropertyBase>(_attachedProperties.ToList());
        }
    }

#if false

    /// <summary>
    /// Gets a collection of all live instances which has associaated attached property values
    /// in this context.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<object> GetInstances()
    {
        using (ReadLockScope())
        {
            var keys = (ICollection<object>)_keysProperty.GetValue(_stores);
            return new ReadOnlyCollection<object>(keys.ToList());
        }
    }

#endif

    /// <summary>
    /// Gets all attached properties and their values for an insatnce in this context.
    /// </summary>
    /// <param name="instance">The instance which to retrieve attached properties for.</param>
    public IReadOnlyDictionary<AttachedPropertyBase, object?> GetInstanceProperties(object instance)
    {
        using (ReadLockScope())
        {
            return _stores.TryGetValue(instance, out var store)
                ? store.Values
                : new Dictionary<AttachedPropertyBase, object?>();
        }
    }

    /// <summary>
    /// Get an attached property value from an instance in the global context.
    /// </summary>
    /// <typeparam name="TOwner">Type of the instance which has the associated attached
    /// property value.</typeparam>
    /// <typeparam name="TProperty">Type of the attached property value.</typeparam>
    /// <param name="instance">The instance to retrieve attached property for.</param>
    /// <param name="attachedProperty">The attached property to get a value for.</param>
    public TProperty? GetInstanceValue<TOwner, TProperty>(TOwner instance, AttachedProperty<TOwner, TProperty> attachedProperty)
        where TOwner : class
    {
        GuardAgainst.Null(instance);
        GuardAgainst.Null(attachedProperty);

        EnsureRegistered(attachedProperty);
        using (ReadLockScope())
        {
            if (!_stores.TryGetValue(instance, out var store))
            {
                return default;
            }

            if (!store.TryGetValue(attachedProperty, out var value))
            {
                return default;
            }

            return (TProperty?)value;
        }
    }

    /// <summary>
    /// Set an attached property value for an instance in the global context.
    /// </summary>
    /// <typeparam name="TOwner">Type of the instance which the value will be associated
    /// with.</typeparam>
    /// <typeparam name="TProperty">Type of the attached property value.</typeparam>
    /// <param name="instance">The instance to set attached property for.</param>
    /// <param name="attachedProperty">The attached property to set a value for.</param>
    /// <param name="value">The value.</param>
    public void SetInstanceValue<TOwner, TProperty>(TOwner instance, AttachedProperty<TOwner, TProperty> attachedProperty, TProperty? value)
        where TOwner : class
    {
        GuardAgainst.Null(instance);
        GuardAgainst.Null(attachedProperty);

        EnsureRegistered(attachedProperty);
        using (WriteLockScope())
        {
            var store = _stores.GetOrCreateValue(instance);
            if (Equals(value, default(TProperty)))
            {
                store.RemoveValue(attachedProperty);
                if (store.Count == 0)
                {
                    _stores.Remove(instance);
                }
            }
            else
            {
                store.SetValue(attachedProperty, value);
            }
        }
    }

    public void Dispose()
    {
        _lock.Dispose();
    }

    private void EnsureRegistered(AttachedPropertyBase attachedProperty)
    {
        using (ReadLockScope())
        {
            if (!_attachedProperties.Contains(attachedProperty))
            {
                throw new AttachedPropertyException($"Attached property named {attachedProperty.Name} for type {attachedProperty.OwnerType.FullName} is not reigstered in current context");
            }
        }
    }

    private IDisposable ReadLockScope()
    {
        _lock.EnterReadLock();
        return new DisposeAction(_lock.ExitReadLock);
    }

    private IDisposable WriteLockScope()
    {
        _lock.EnterWriteLock();
        return new DisposeAction(_lock.ExitWriteLock);
    }
}

using System.Reflection;

namespace AttachedProperties.Internal;

public abstract class AbstractAttachedProperty
{
    protected AbstractAttachedProperty(Type ownerType, Type propertyType, string name, AttachedPropertyContext context)
    {
        if (ownerType == null)
        {
            throw new ArgumentNullException(nameof(ownerType));
        }

        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var propertyInfo = ownerType.GetRuntimeProperty(name);
        if (propertyInfo != null)
        {
            throw new ArgumentException($"Type {ownerType.FullName} already has a property named {name}", nameof(name));
        }

        OwnerType = ownerType;
        PropertyType = propertyType;
        Name = name;

        context.Register(this);
    }

    public Type OwnerType { get; }

    public Type PropertyType { get; }

    public string Name { get; }

    public string FullName => $"{OwnerType.FullName}.{Name}";
}

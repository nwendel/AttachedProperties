using System.Reflection;

namespace AttachedProperties.Internal;

public abstract class AttachedPropertyBase
{
    protected AttachedPropertyBase(Type ownerType, Type propertyType, string name, AttachedPropertyContext context)
    {
        GuardAgainst.Null(ownerType);
        GuardAgainst.NullOrWhiteSpace(name);
        GuardAgainst.Null(context);

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

using AttachedProperties.Internal;

namespace AttachedProperties;

/// <summary>
/// Defines an attached property for type TOwner where the property type is TProperty.
/// </summary>
public class AttachedProperty<TOwner, TProperty> : AbstractAttachedProperty
{
    /// <summary>
    /// Creates a <see cref="AttachedProperties"/> which is used to attach values to instances
    /// in the global context.
    /// </summary>
    public AttachedProperty(string name)
        : this(name, AttachedPropertyContext.GlobalContext)
    {
    }

    /// <summary>
    /// Creates a <see cref="AttachedProperties"/> which is used to attach values to instances
    /// in the specified context.
    /// </summary>
    public AttachedProperty(string name, AttachedPropertyContext context)
        : base(typeof(TOwner), typeof(TProperty), name, context)
    {
    }
}

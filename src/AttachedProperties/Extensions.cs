namespace AttachedProperties;

public static class Extensions
{
    public static TResult? GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty)
    {
        var value = instance.GetAttachedValue(attachedProperty, AttachedPropertyContext.GlobalContext);
        return value;
    }

    public static TResult? GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, AttachedPropertyContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var value = context.GetInstanceValue(instance, attachedProperty);
        return value;
    }

    public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult? value)
    {
        instance.SetAttachedValue(attachedProperty, value, AttachedPropertyContext.GlobalContext);
    }

    public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult? value, AttachedPropertyContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.SetInstanceValue(instance, attachedProperty, value);
    }
}

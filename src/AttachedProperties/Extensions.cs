namespace AttachedProperties;

public static class Extensions
{
    public static TResult? GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty)
        where T : class
    {
        var value = instance.GetAttachedValue(attachedProperty, AttachedPropertyContext.GlobalContext);
        return value;
    }

    public static TResult? GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, AttachedPropertyContext context)
        where T : class
    {
        GuardAgainst.Null(context);

        var value = context.GetInstanceValue(instance, attachedProperty);
        return value;
    }

    public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult? value)
        where T : class
    {
        instance.SetAttachedValue(attachedProperty, value, AttachedPropertyContext.GlobalContext);
    }

    public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult? value, AttachedPropertyContext context)
        where T : class
    {
        GuardAgainst.Null(context);

        context.SetInstanceValue(instance, attachedProperty, value);
    }
}

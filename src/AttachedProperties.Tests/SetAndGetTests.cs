namespace AttachedProperties.Tests;

public class SetAndGetTests
{
    [Fact]
    public void CanSetAndGet()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
        var tested = new object();

        tested.SetAttachedValue(attachedProperty, "asdf", context);
        var value = tested.GetAttachedValue(attachedProperty, context);

        Assert.Equal("asdf", value);
    }

    [Fact]
    public void CanGetDefaultWithoutSet()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", context);
        var tested = new object();

        var value = tested.GetAttachedValue(attachedProperty, context);

        Assert.Null(value);
    }

    [Fact]
    public void CanSetDefaultValueDoesNotStore()
    {
        using var tested = new AttachedPropertyContext();

        var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", tested);
        var instance = new object();
        instance.SetAttachedValue(attachedProperty, null, tested);
        var properties = tested.GetInstanceProperties(instance);

        Assert.Equal(0, properties.Count);
    }

    [Fact]
    public void CanSetValueThenDefaultValueRemoveFromStore()
    {
        using var tested = new AttachedPropertyContext();

        var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", tested);
        var instance = new object();
        instance.SetAttachedValue(attachedProperty, "asdf", tested);
        instance.SetAttachedValue(attachedProperty, null, tested);
        var properties = tested.GetInstanceProperties(instance);

        Assert.Equal(0, properties.Count);
    }

    [Fact]
    public void CanGetNonObjectGetValueNoSet()
    {
        using var tested = new AttachedPropertyContext();

        var attachedProperty = new AttachedProperty<object, int>("CanGetNonObjectGetValueNoSet", tested);
        var another = new AttachedProperty<object, int>("Another", tested);
        var instance = new object();
        instance.SetAttachedValue(another, 1, tested);
        var value = instance.GetAttachedValue(attachedProperty, tested);

        Assert.Equal(0, value);
    }
}

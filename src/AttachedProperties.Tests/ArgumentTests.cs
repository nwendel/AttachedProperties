using Xunit;

namespace AttachedProperties.Tests;

public class ArgumentTests
{
    [Fact]
    public void ThrowsOnSetNullAttachedProperty()
    {
        using var context = new AttachedPropertyContext();
        var tested = new object();

        Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(null!, "asdf", context));
    }

    [Fact]
    public void ThrowsOnSetNullContext()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
        var tested = new object();

        Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", null!));
    }

    [Fact]
    public void ThrowsOnSetNullInstance()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
        object tested = null!;

        Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", context));
    }

    [Fact]
    public void ThrowsOnGetNullAttachedProperty()
    {
        using var context = new AttachedPropertyContext();
        var tested = new object();

        Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue((AttachedProperty<object, string>)null!, context));
    }

    [Fact]
    public void ThrowsOnGetNullContext()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
        var tested = new object();

        Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue(attachedProperty, null!));
    }

    [Fact]
    public void ThrowsOnGetNullInstance()
    {
        using var context = new AttachedPropertyContext();
        var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
        object tested = null!;

        Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue(attachedProperty, context));
    }
}

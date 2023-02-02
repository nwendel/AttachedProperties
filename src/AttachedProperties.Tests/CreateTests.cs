using AttachedProperties.Tests.Classes;
using Xunit;

namespace AttachedProperties.Tests;

public class CreateTests
{
    [Fact]
    public void CanCreate()
    {
        using var context = new AttachedPropertyContext();
        var tested = new AttachedProperty<object, int>("SomeName", context);

        Assert.Equal("System.Object.SomeName", tested.FullName);
        Assert.Equal(typeof(object), tested.OwnerType);
        Assert.Equal(typeof(int), tested.PropertyType);
    }

    [Fact]
    public void ThrowsOnCreateNullName()
    {
        using var context = new AttachedPropertyContext();

        Assert.Throws<ArgumentNullException>("name", () => new AttachedProperty<object, object>(null!, context));
    }

    [Fact]
    public void ThrowsOnCreateNullContext()
    {
        var name = typeof(SomeClass).GetProperties().First().Name;

        Assert.Throws<ArgumentNullException>("context", () => new AttachedProperty<SomeClass, object>(name, null!));
    }

    [Fact]
    public void ThrowsOnCreateExistingProperty()
    {
        using var context = new AttachedPropertyContext();
        var name = typeof(SomeClass).GetProperties().First().Name;

        Assert.Throws<ArgumentException>("name", () => new AttachedProperty<SomeClass, object>(name, context));
    }

    [Fact]
    public void ThrowsOnCreateDuplicate()
    {
        using var context = new AttachedPropertyContext();

        var first = new AttachedProperty<object, int>("asdf", context);

        Assert.NotNull(first);
        Assert.Throws<AttachedPropertyException>(() => new AttachedProperty<object, int>("asdf", context));
    }

    [Fact]
    public void ThrowsOnGetNotRegistered()
    {
        using var context = new AttachedPropertyContext();
        var tested = new object();

        var attachedProperty = new AttachedProperty<object, int>("asdf", context);
        Assert.Throws<AttachedPropertyException>(() => _ = tested.GetAttachedValue(attachedProperty));
    }

    [Fact]
    public void ThrowsOnSetNotRegistered()
    {
        using var context = new AttachedPropertyContext();
        var tested = new object();

        var attachedProperty = new AttachedProperty<object, int>("asdf", context);
        Assert.Throws<AttachedPropertyException>(() => tested.SetAttachedValue(attachedProperty, 5));
    }
}

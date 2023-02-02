using System;
using System.Linq;
using AttachedProperties.Tests.Classes;
using Xunit;

namespace AttachedProperties.Tests
{

    /// <summary>
    /// 
    /// </summary>
    public class CreateTests
    {

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanCreate()
        {
            var context = new AttachedPropertyContext();
            var tested = new AttachedProperty<object, int>("SomeName", context);

            Assert.Equal("System.Object.SomeName", tested.FullName);
            Assert.Equal(typeof(object), tested.OwnerType);
            Assert.Equal(typeof(int), tested.PropertyType);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateNullName()
        {
            var context = new AttachedPropertyContext();

            Assert.Throws<ArgumentNullException>("name", () => new AttachedProperty<object, object>(null, context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateNullContext()
        {
            var name = typeof(SomeClass).GetProperties().First().Name;

            Assert.Throws<ArgumentNullException>("context", () => new AttachedProperty<SomeClass, object>(name, null));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateExistingProperty()
        {
            var context = new AttachedPropertyContext();
            var name = typeof(SomeClass).GetProperties().First().Name;

            Assert.Throws<ArgumentException>("name", () => new AttachedProperty<SomeClass, object>(name, context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateDuplicate()
        {
            var context = new AttachedPropertyContext();

            var first = new AttachedProperty<object, int>("asdf", context);

            Assert.NotNull(first);
            Assert.Throws<AttachedPropertyException>(() => new AttachedProperty<object, int>("asdf", context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnGetNotRegistered()
        {
            var context = new AttachedPropertyContext();
            var tested = new object();

            var attachedProperty = new AttachedProperty<object, int>("asdf", context);
            Assert.Throws<AttachedPropertyException>(() =>
            {
                var _ = tested.GetAttachedValue(attachedProperty);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnSetNotRegistered()
        {
            var context = new AttachedPropertyContext();
            var tested = new object();

            var attachedProperty = new AttachedProperty<object, int>("asdf", context);
            Assert.Throws<AttachedPropertyException>(() =>
            {
                tested.SetAttachedValue(attachedProperty, 5);
            });
        }

    }

}

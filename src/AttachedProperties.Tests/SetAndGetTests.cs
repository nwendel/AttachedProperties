using Xunit;

namespace AttachedProperties.Tests
{

    /// <summary>
    /// 
    /// </summary>
    public class SetAndGetTests
    {

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanSetAndGet()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
            var tested = new object();

            tested.SetAttachedValue(attachedProperty, "asdf", context);
            var value = tested.GetAttachedValue(attachedProperty, context);

            Assert.Equal("asdf", value);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetDefaultWithoutSet()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", context);
            var tested = new object();

            var value = tested.GetAttachedValue(attachedProperty, context);

            Assert.Equal(default(string), value);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanSetDefaultValueDoesNotStore()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, default(string), tested);
            var properties = tested.GetInstanceProperties(instance);

            Assert.Equal(0, properties.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanSetValueThenDefaultValueRemoveFromStore()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, string>("CanGetDefaultWithoutSet", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, "asdf", tested);
            instance.SetAttachedValue(attachedProperty, default(string), tested);
            var properties = tested.GetInstanceProperties(instance);

            Assert.Equal(0, properties.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetNonObjectGetValueNoSet()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("CanGetNonObjectGetValueNoSet", tested);
            var another = new AttachedProperty<object, int>("Another", tested);
            var instance = new object();
            instance.SetAttachedValue(another, 1, tested);
            var value = instance.GetAttachedValue(attachedProperty, tested);

            Assert.Equal(0, value);
        }

    }

}

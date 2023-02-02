using Xunit;

namespace AttachedProperties.Tests
{

    /// <summary>
    /// 
    /// </summary>
    public class ContextTests
    {

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetProperties()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("PropertyName", tested);
            var properties = tested.GetProperties();

            Assert.Equal(1, properties.Count);
            Assert.All(properties, x =>
            {
                Assert.Same(attachedProperty, x);
            });
        }

#if false

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetInstances()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("PropertyName", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, 1, tested);
            var instances = tested.GetInstances();

            Assert.Equal(1, instances.Count);
            Assert.All(instances, x =>
            {
                Assert.Same(instance, x);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetInstancesNoReference()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("PropertyName", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, 1, tested);
            instance = null;
            GC.Collect(0, GCCollectionMode.Forced, true);
            var instances = tested.GetInstances();

            Assert.Null(instance);
            Assert.Equal(0, instances.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetInstancesDefaultValue()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("PropertyName", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, default(int), tested);
            var instances = tested.GetInstances();

            Assert.Equal(0, instances.Count);
        }

#endif

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanGetPropertiesForInstance()
        {
            var tested = new AttachedPropertyContext();

            var attachedProperty = new AttachedProperty<object, int>("PropertyName", tested);
            var instance = new object();
            instance.SetAttachedValue(attachedProperty, 1, tested);
            var values = tested.GetInstanceProperties(instance);

            Assert.Equal(1, values.Count);
            Assert.All(values, x =>
            {
                Assert.Same(attachedProperty, x.Key);
                Assert.Equal(1, x.Value);
            });
        }

    }

}

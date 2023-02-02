using System;
using Xunit;

namespace AttachedProperties.Tests
{

    /// <summary>
    /// 
    /// </summary>
    public class ArgumentTests
    {

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnSetNullAttachedProperty()
        {
            var context = new AttachedPropertyContext();
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(null, "asdf", context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnSetNullContext()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", null));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnSetNullInstance()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
            object tested = null;

            Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnGetNullAttachedProperty()
        {
            var context = new AttachedPropertyContext();
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue((AttachedProperty<object, string>)null, context));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnGetNullContext()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue(attachedProperty, null));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnGetNullInstance()
        {
            var context = new AttachedPropertyContext();
            var attachedProperty = new AttachedProperty<object, string>("CanSetAndGet", context);
            object tested = null;

            Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue(attachedProperty, context));
        }

    }

}

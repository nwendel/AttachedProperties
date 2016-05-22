#region License
// Copyright (c) Niklas Wendel
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion
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

    }

}

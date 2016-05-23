#region License
// Copyright (c) Niklas Wendel 2016
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

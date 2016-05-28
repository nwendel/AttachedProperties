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
            AttachedProperty<object, string> attachedProperty = null;
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", context));
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
            AttachedProperty<object, string> attachedProperty = null;
            var tested = new object();

            Assert.Throws<ArgumentNullException>(() => tested.GetAttachedValue(attachedProperty, context));
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

            Assert.Throws<ArgumentNullException>(() => tested.SetAttachedValue(attachedProperty, "asdf", context));
        }

    }

}

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
using System;
using System.Linq;
using System.Reflection;
using Xunit;
using AttachedProperties.Tests.TestClasses;

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

            Assert.Throws<ArgumentNullException>("name", () =>
            {
                var tested = new AttachedProperty<object, object>(null, context);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateNullContext()
        {
            var name = typeof(SomeClass).GetTypeInfo().GetProperties().First().Name;

            Assert.Throws<ArgumentNullException>("context", () =>
            {
                var tested = new AttachedProperty<SomeClass, object>(name, null);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateExistingProperty()
        {
            var context = new AttachedPropertyContext();
            var name = typeof(SomeClass).GetTypeInfo().GetProperties().First().Name;

            Assert.Throws<ArgumentException>("name", () =>
            {
                var tested = new AttachedProperty<SomeClass, object>(name, context);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ThrowsOnCreateDuplicate()
        {
            var context = new AttachedPropertyContext();

            var first = new AttachedProperty<object, int>("asdf", context);
            Assert.Throws<AttachedPropertyException>(() =>
            {
                var _ = new AttachedProperty<object, int>("asdf", context);
            });
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

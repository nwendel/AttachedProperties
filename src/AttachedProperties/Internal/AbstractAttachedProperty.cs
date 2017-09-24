#region License
// Copyright (c) Niklas Wendel 2016-2017
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
using System.Reflection;

namespace AttachedProperties.Internal
{

    /// <summary>
    /// Abstract base class used to define <see cref="AttachedProperties"/>.
    /// </summary>
    public abstract class AbstractAttachedProperty
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        protected AbstractAttachedProperty(Type ownerType, Type propertyType, string name, AttachedPropertyContext context)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var propertyInfo = ownerType.GetRuntimeProperty(name);
            if (propertyInfo != null)
            {
                throw new ArgumentException(string.Format("Type {0} already has a property named {1}", ownerType.FullName, name), nameof(name));
            }

            OwnerType = ownerType;
            PropertyType = propertyType;
            Name = name;

            context.Register(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Type OwnerType { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName => string.Format("{0}.{1}", OwnerType.FullName, Name);

        #endregion

    }

}

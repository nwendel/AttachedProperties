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
using AttachedProperties.Internal;

namespace AttachedProperties
{

    /// <summary>
    /// Defines an attached property for type TOwner where the property type is TProperty.
    /// </summary>
    public class AttachedProperty<TOwner, TProperty> : AbstractAttachedProperty
    {

        #region Constructor

        /// <summary>
        /// Creates a <see cref="AttachedProperties"/> which is used to attach values to instances
        /// in the global context.
        /// </summary>
        /// <param name="name">Name of the attached property.</param>
        public AttachedProperty(string name) : this(name, AttachedPropertyContext.GlobalContext)
        {
        }

        /// <summary>
        /// Creates a <see cref="AttachedProperties"/> which is used to attach values to instances
        /// in a specified context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        public AttachedProperty(string name, AttachedPropertyContext context) : base(typeof(TOwner), typeof(TProperty), name, context)
        {
        }

        #endregion

    }

}

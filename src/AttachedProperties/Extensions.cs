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
using System.Collections.Generic;

namespace AttachedProperties
{

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {

        #region Get Attached Value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="attachedProperty"></param>
        /// <returns></returns>
        public static TResult GetAttachedValue<T, TResult>(this T self, AttachedProperty<T, TResult> attachedProperty)
        {
            var value = self.GetAttachedValue(attachedProperty, AttachedPropertyContext.GlobalContext);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TResult GetAttachedValue<T, TResult>(this T self, AttachedProperty<T, TResult> attachedProperty, AttachedPropertyContext context)
        {
            var value = context.GetInstanceValue(self, attachedProperty);
            return value;
        }

        #endregion

        #region Set Attached Value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        public static void SetAttachedValue<T, TResult>(this T self, AttachedProperty<T, TResult> attachedProperty, TResult value)
        {
            self.SetAttachedValue(attachedProperty, value, AttachedPropertyContext.GlobalContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        /// <param name="context"></param>
        public static void SetAttachedValue<T, TResult>(this T self, AttachedProperty<T, TResult> attachedProperty, TResult value, AttachedPropertyContext context)
        {
            context.SetInstanceValue(self, attachedProperty, value);
        }

        #endregion

    }

}

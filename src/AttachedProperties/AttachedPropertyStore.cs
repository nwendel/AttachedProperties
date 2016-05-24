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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AttachedProperties
{

    /// <summary>
    /// 
    /// </summary>
    public class AttachedPropertyStore
    {

        #region Fields

        private ConcurrentDictionary<AbstractAttachedProperty, object> _values = new ConcurrentDictionary<AbstractAttachedProperty, object>();

        #endregion

        #region Count

        /// <summary>
        /// 
        /// </summary>
        public int Count => _values.Count;

        #endregion

        #region Values

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<AbstractAttachedProperty, object> Values => new ReadOnlyDictionary<AbstractAttachedProperty, object>(_values);

        #endregion

        #region Get Value

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <returns></returns>
        public object GetValue(AbstractAttachedProperty attachedProperty)
        {
            object value;
            _values.TryGetValue(attachedProperty, out value);
            return value;
        }

        #endregion

        #region Set Value

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        public void SetValue(AbstractAttachedProperty attachedProperty, object value)
        {
            _values[attachedProperty] = value;
        }

        #endregion

        #region Remove Value

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachedProperty"></param>
        public void RemoveValue(AbstractAttachedProperty attachedProperty)
        {
            object _;
            _values.TryRemove(attachedProperty, out _);
        }

        #endregion

    }

}

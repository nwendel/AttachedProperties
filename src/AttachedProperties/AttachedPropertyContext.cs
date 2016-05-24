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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AttachedProperties
{

    /// <summary>
    /// 
    /// </summary>
    public class AttachedPropertyContext
    {

        #region Fields

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private HashSet<AbstractAttachedProperty> _attachedProperties = new HashSet<AbstractAttachedProperty>();
        private readonly ConditionalWeakTable<object, AttachedPropertyStore> _stores = new ConditionalWeakTable<object, AttachedPropertyStore>();

        #endregion

        #region Register

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachedProperty"></param>
        public void Register(AbstractAttachedProperty attachedProperty)
        {
            using (new DisposableAction(() => _lock.EnterWriteLock(), () => _lock.ExitWriteLock()))
            {
                var fullName = attachedProperty.FullName;
                if (_attachedProperties.Select(x => x.FullName).Contains(fullName))
                {
                    throw new AttachedPropertyException(
                        string.Format("Attached property named {0} for type {1} is already registered in current context",
                            attachedProperty.Name, attachedProperty.OwnerType.FullName));
                }

                _attachedProperties.Add(attachedProperty);
            }
        }

        #endregion

        #region Ensure Registered

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachedProperty"></param>
        private void EnsureRegistered(AbstractAttachedProperty attachedProperty)
        {
            using(new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                if (!_attachedProperties.Contains(attachedProperty))
                {
                    throw new AttachedPropertyException(
                        string.Format("Attached property named {0} for type {1} is not reigstered in current context",
                            attachedProperty.Name, attachedProperty.OwnerType.FullName));
                }
            }
        }

        #endregion

        #region Get Properties

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<AbstractAttachedProperty> GetProperties()
        {
            using (new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                return new ReadOnlyCollection<AbstractAttachedProperty>(_attachedProperties.ToList());
            }
        }

        #endregion

        #region Get Instance Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<AbstractAttachedProperty, object> GetInstanceProperties(object instance)
        {
            using (new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                AttachedPropertyStore store;
                var found = _stores.TryGetValue(instance, out store);
                if (!found)
                {
                    return null;
                }

                return store.Get();
            }
        }

        #endregion

        #region Get Instance Value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <returns></returns>
        public TProperty GetInstanceValue<TOwner, TProperty>(TOwner instance, AttachedProperty<TOwner, TProperty> attachedProperty)
        {
            EnsureRegistered(attachedProperty);
            using (new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                AttachedPropertyStore store;
                var found = _stores.TryGetValue(instance, out store);
                if (!found)
                {
                    return default(TProperty);
                }

                var value = store.GetValue(attachedProperty);
                return (TProperty)value;
            }
        }

        #endregion

        #region Set Instance Value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <returns></returns>
        public void SetInstanceValue<TOwner, TProperty>(TOwner instance, AttachedProperty<TOwner, TProperty> attachedProperty, TProperty value)
        {
            EnsureRegistered(attachedProperty);
            using (new DisposableAction(() => _lock.EnterWriteLock(), () => _lock.ExitWriteLock()))
            {
                var store = _stores.GetOrCreateValue(instance);
                if (object.Equals(value, default(TProperty)))
                {
                    store.RemoveValue(attachedProperty);
                }
                else
                {
                    store.SetValue(attachedProperty, value);
                }
            }
        }

        #endregion

        #region Global Instance

        /// <summary>
        /// 
        /// </summary>
        public static AttachedPropertyContext GlobalInstance = new AttachedPropertyContext();

        #endregion

    }

}

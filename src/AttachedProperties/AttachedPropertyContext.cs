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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AttachedProperties
{

    /// <summary>
    /// The context in which attached properties are defined and which is also used to set and
    /// get attached property values.
    /// </summary>
    public class AttachedPropertyContext
    {

        #region Fields

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly HashSet<AbstractAttachedProperty> _attachedProperties = new HashSet<AbstractAttachedProperty>();
        private readonly ConditionalWeakTable<object, AttachedPropertyStore> _stores = new ConditionalWeakTable<object, AttachedPropertyStore>();
        private static readonly PropertyInfo _keysProperty = typeof(ConditionalWeakTable<object, AttachedPropertyStore>).GetTypeInfo().GetProperty("Keys", BindingFlags.Instance | BindingFlags.NonPublic);

        #endregion

        #region Register

        /// <summary>
        /// Register a new attached property in this context.
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
        /// Get a collection of all defined attached properties in this context.
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

        #region Get Instances

        /// <summary>
        /// Gets a collection of all live instances which has associaated attached property values
        /// in this context.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<object> GetInstances()
        {
            using (new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                var keys = (ICollection<object>)_keysProperty.GetValue(_stores);
                return new ReadOnlyCollection<object>(keys.ToList());
            }
        }

        #endregion

        #region Get Instance Properties

        /// <summary>
        /// Gets all attached properties and their values for an insatnce in this context.
        /// </summary>
        /// <param name="instance">The instance which to retrieve attached properties for.</param>
        /// <returns></returns>
        public IReadOnlyDictionary<AbstractAttachedProperty, object> GetInstanceProperties(object instance)
        {
            using (new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock()))
            {
                AttachedPropertyStore store;
                var found = _stores.TryGetValue(instance, out store);
                if (!found)
                {
                    return new Dictionary<AbstractAttachedProperty, object>();
                }

                return store.Values;
            }
        }

        #endregion

        #region Get Instance Value

        /// <summary>
        /// Get an attached property value from an instance in the global context.
        /// </summary>
        /// <typeparam name="TOwner">Type of the instance which has the associated attached
        /// property value.</typeparam>
        /// <typeparam name="TProperty">Type of the attached property value.</typeparam>
        /// <param name="instance">The instance to retrieve attached property for.</param>
        /// <param name="attachedProperty">The attached property to get a value for.</param>
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

                object value;
                found = store.TryGetValue(attachedProperty, out value);
                if(!found)
                {
                    return default(TProperty);
                }
                return (TProperty)value;
            }
        }

        #endregion

        #region Set Instance Value

        /// <summary>
        /// Set an attached property value for an instance in the global context.
        /// </summary>
        /// <typeparam name="TOwner">Type of the instance which the value will be associated
        /// with.</typeparam>
        /// <typeparam name="TProperty">Type of the attached property value.</typeparam>
        /// <param name="instance">The instance to set attached property for.</param>
        /// <param name="attachedProperty">The attached property to set a value for.</param>
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
                    if(store.Count == 0)
                    {
                        _stores.Remove(instance);
                    }
                }
                else
                {
                    store.SetValue(attachedProperty, value);
                }
            }
        }

        #endregion

        #region Global Context

        /// <summary>
        /// An instance of the global context.
        /// </summary>
        public static AttachedPropertyContext GlobalContext = new AttachedPropertyContext();

        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using AttachedProperties.Internal;

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

#if false
        private static readonly PropertyInfo _keysProperty = typeof(ConditionalWeakTable<object, AttachedPropertyStore>).GetTypeInfo().GetDeclaredProperty("Keys");
#endif

        #endregion

        #region Register

        /// <summary>
        /// Register a new attached property in this context.
        /// </summary>
        /// <param name="attachedProperty"></param>
        public void Register(AbstractAttachedProperty attachedProperty)
        {
            using (WriteLockScope())
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
            using (ReadLockScope())
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
            using (ReadLockScope())
            {
                return new ReadOnlyCollection<AbstractAttachedProperty>(_attachedProperties.ToList());
            }
        }

        #endregion

        #region Get Instances

#if false

        /// <summary>
        /// Gets a collection of all live instances which has associaated attached property values
        /// in this context.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<object> GetInstances()
        {
            using (ReadLockScope())
            {
                var keys = (ICollection<object>)_keysProperty.GetValue(_stores);
                return new ReadOnlyCollection<object>(keys.ToList());
            }
        }

#endif

        #endregion

        #region Get Instance Properties

        /// <summary>
        /// Gets all attached properties and their values for an insatnce in this context.
        /// </summary>
        /// <param name="instance">The instance which to retrieve attached properties for.</param>
        /// <returns></returns>
        public IReadOnlyDictionary<AbstractAttachedProperty, object> GetInstanceProperties(object instance)
        {
            using (ReadLockScope())
            {
                var found = _stores.TryGetValue(instance, out var store);
                return found
                    ? store.Values
                    : new Dictionary<AbstractAttachedProperty, object>();
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
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (attachedProperty == null)
            {
                throw new ArgumentNullException(nameof(attachedProperty));
            }

            EnsureRegistered(attachedProperty);
            using (ReadLockScope())
            {
                var found = _stores.TryGetValue(instance, out var store);
                if (!found)
                {
                    return default(TProperty);
                }

                found = store.TryGetValue(attachedProperty, out var value);
                if (!found)
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
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetInstanceValue<TOwner, TProperty>(TOwner instance, AttachedProperty<TOwner, TProperty> attachedProperty, TProperty value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (attachedProperty == null)
            {
                throw new ArgumentNullException(nameof(attachedProperty));
            }

            EnsureRegistered(attachedProperty);
            using (WriteLockScope())
            {
                var store = _stores.GetOrCreateValue(instance);
                if (Equals(value, default(TProperty)))
                {
                    store.RemoveValue(attachedProperty);
                    if (store.Count == 0)
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

        #region Read / Write Lock Scope

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IDisposable ReadLockScope()
        {
            return new DisposableAction(() => _lock.EnterReadLock(), () => _lock.ExitReadLock());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IDisposable WriteLockScope()
        {
            return new DisposableAction(() => _lock.EnterWriteLock(), () => _lock.ExitWriteLock());
        }

        #endregion

        #region Global Context

        /// <summary>
        /// An instance of the global context.
        /// </summary>
        public static readonly AttachedPropertyContext GlobalContext = new AttachedPropertyContext();

        #endregion

    }

}

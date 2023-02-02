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
            if (context == null)
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

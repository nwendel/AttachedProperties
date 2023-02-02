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

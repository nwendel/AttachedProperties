using System;

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
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <returns></returns>
        public static TResult GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty)
        {
            var value = instance.GetAttachedValue(attachedProperty, AttachedPropertyContext.GlobalContext);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TResult GetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, AttachedPropertyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var value = context.GetInstanceValue(instance, attachedProperty);
            return value;
        }

        #endregion

        #region Set Attached Value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult value)
        {
            instance.SetAttachedValue(attachedProperty, value, AttachedPropertyContext.GlobalContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="instance"></param>
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        /// <param name="context"></param>
        public static void SetAttachedValue<T, TResult>(this T instance, AttachedProperty<T, TResult> attachedProperty, TResult value, AttachedPropertyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.SetInstanceValue(instance, attachedProperty, value);
        }

        #endregion

    }

}

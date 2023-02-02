using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AttachedProperties.Internal
{

    /// <summary>
    /// 
    /// </summary>
    public class AttachedPropertyStore
    {

        #region Fields

        private readonly ConcurrentDictionary<AbstractAttachedProperty, object> _values = new ConcurrentDictionary<AbstractAttachedProperty, object>();

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
        /// <param name="attachedProperty"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(AbstractAttachedProperty attachedProperty, out object value)
        {
            var found = _values.TryGetValue(attachedProperty, out value);
            return found;
        }

        #endregion

        #region Set Value

        /// <summary>
        /// 
        /// </summary>
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
            _values.TryRemove(attachedProperty, out var _);
        }

        #endregion

    }

}

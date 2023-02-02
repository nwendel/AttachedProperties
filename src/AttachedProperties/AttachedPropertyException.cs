using System;

namespace AttachedProperties
{

    /// <summary>
    /// 
    /// </summary>
    public class AttachedPropertyException : Exception
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AttachedPropertyException(string message) : base(message)
        {
        }

        #endregion

    }

}

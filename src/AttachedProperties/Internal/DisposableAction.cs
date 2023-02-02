using System;

namespace AttachedProperties.Internal
{

    /// <summary>
    /// 
    /// </summary>
    internal sealed class DisposableAction : IDisposable
    {

        #region Fields

        private readonly Action _disposableAction;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="disposableAction"></param>
        internal DisposableAction(Action action, Action disposableAction)
        {
            action();
            _disposableAction = disposableAction;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _disposableAction();
        }

        #endregion

    }

}

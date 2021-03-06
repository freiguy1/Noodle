﻿using System;

namespace Noodle
{
    /// <summary>
    /// Internal services notifiy the system of errors through this interface
    /// </summary>
    public class ErrorNotifier : IErrorNotifier
    {
        #region IErrorNotifier Members

        /// <summary>
        /// Notify the system of an exception
        /// </summary>
        /// <param name="ex"></param>
        public void Notify(Exception ex)
        {
            if (ErrorOccured != null)
                ErrorOccured(this, new NoodleEventArgs<Exception>(ex));
        }

        /// <summary>
        /// Raised when an error occurs
        /// </summary>
        public event EventHandler<NoodleEventArgs<Exception>> ErrorOccured;

        #endregion
    }
}

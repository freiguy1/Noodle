﻿using System;

namespace Noodle
{
    /// <summary>
    /// Internal services notifiy the system of errors through this interface
    /// </summary>
    public interface IErrorNotifier
    {
        /// <summary>
        /// Notify the system of an exception
        /// </summary>
        /// <param name="ex"></param>
        void Notify(Exception ex);
        
        /// <summary>
        /// Raised when an error occurs
        /// </summary>
        event EventHandler<NoodleEventArgs<Exception>> ErrorOccured;
    }
}

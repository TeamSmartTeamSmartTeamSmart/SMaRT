// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionInvalidException.cs" company="Novomatic Gaming Industries GmbH">
//   Copyright 2017 Novomatic Gaming Industries GmbH.
// </copyright>
// <summary>
//   The session invalid exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SMaRT.AgentService
{
    using System;

    /// <summary>
    /// The session invalid exception.
    /// </summary>
    public class SessionInvalidException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInvalidException"/> class.
        /// </summary>
        /// <param name="sessionOverdue">
        /// Indicates whether a session became invalid over time.
        /// </param>
        public SessionInvalidException(bool sessionOverdue)
        {
            this.SessionOverdue = sessionOverdue;
        }

        /// <summary>
        /// Gets a value indicating whether the session was overdue.
        /// </summary>
        public bool SessionOverdue { get; private set; }
    }
}
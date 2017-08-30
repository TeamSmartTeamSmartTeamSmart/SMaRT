// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionSession.cs" company="Novomatic Gaming Industries GmbH">
//   Copyright 2017 Novomatic Gaming Industries GmbH.
// </copyright>
// <summary>
//   Class for managing a connection with a WCF service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SMaRT.AgentService
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using log4net;

    using SMaRT.Shared.ConnectionObjects;

    using TimeoutException = System.ServiceProcess.TimeoutException;

    /// <summary>
    /// Class for managing a connection with a WCF service.
    /// </summary>
    /// <typeparam name="TProxy">
    /// The Interface of the Channel.
    /// </typeparam>
    public class ConnectionSession<TProxy> : IDisposable where TProxy : class 
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionSession<>));

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionSession{TProxy}"/> class. 
        /// </summary>
        /// <param name="maxDuration">
        /// The max Duration.
        /// </param>
        /// <param name="factory">
        /// The factory for creating a channel.
        /// </param>
        public ConnectionSession(double maxDuration, ChannelFactory<TProxy> factory)
        {
            Log.Info($"Created new session. Max time(ms):{maxDuration}");

            this.MaxDuration = maxDuration;
            this.SessionStart = DateTime.UtcNow;

            this.Factory = factory;

            this.SessionIrreparable = false;

            this.EnsureProxyCreated();
        }
        
        /// <summary>
        /// Gets the factory.
        /// </summary>
        private ChannelFactory<TProxy> Factory { get; }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        private TProxy Proxy { get; set; }

        /// <summary>
        /// The channel created.
        /// </summary>
        private IChannel Channel => (IChannel)this.Proxy;
        
        /// <summary>
        /// Gets the max duration.
        /// </summary>
        private double MaxDuration { get; }

        /// <summary>
        /// Gets the session beginning.
        /// </summary>
        private DateTime SessionStart { get; }

        /// <summary>
        /// Checks if the session is still valid
        /// </summary>
        private bool SessionValid => !this.SessionOverdue && !this.SessionIrreparable;

        /// <summary>
        /// Indicates whether the connection is useable.
        /// </summary>
        private bool ConnectionUseable => this.Channel?.State < CommunicationState.Closing;

        /// <summary>
        /// Indicates whether the session overdue.
        /// </summary>
        private bool SessionOverdue
            =>
                (DateTime.Now - this.SessionStart)
                < TimeSpan.FromMilliseconds(this.MaxDuration * 0.9);

        /// <summary>
        /// Gets or sets a value indicating whether session is irreparable.
        /// </summary>
        private bool SessionIrreparable { get; set; }

        /// <summary>
        /// Disposes the Object
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.Channel?.State != CommunicationState.Faulted)
                {
                    this.Channel?.Close();
                }
            }
            catch
            {
                this.Channel?.Abort();
            }

            this.SessionIrreparable = true;

            Log.Info("Closed Session");
        }

        /// <summary>
        /// Safely executes a instruction.
        /// </summary>
        /// <param name="instruction">
        /// The instruction.
        /// </param>
        /// <typeparam name="TReturn">
        /// The return type.
        /// </typeparam>
        /// <exception cref="SessionInvalidException">
        /// Thrown if the session is not valid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the argument is null.
        /// </exception>
        /// <returns>
        /// The <see cref="TProxy"/>.
        /// </returns>
        public TReturn ExecuteInstruction<TReturn>(Func<TProxy, TReturn> instruction)
        {
            this.EnsureProxyCreated();

            if (!this.SessionValid) 
            {
                Log.Debug($"Session is invalid.");

                throw new SessionInvalidException(this.SessionOverdue);
            }

            if (instruction == null)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            try
            {
                return instruction(this.Proxy);
            }
            catch (FaultException<SMaRTServiceFault>)
            {
                Log.Info("Error in calling the API.");
                throw;
            }
            catch (FaultException)
            {
                Log.Info("Error on Server.");
                throw;
            }
            catch (CommunicationException)
            {
                Log.Error($"Communication error happened. Closing connection.");

                this.Channel.Abort();
                throw new SessionInvalidException(this.SessionOverdue);
            }
            catch (TimeoutException)
            {
                Log.Error($"Connection Timeout happened. Closing connection.");

                this.Channel.Abort();
                throw new SessionInvalidException(this.SessionOverdue);
            }
            catch
            {
                this.Dispose();

                throw new SessionInvalidException(this.SessionOverdue);
            }
        }

        /// <summary>
        /// Executes the instruction from parameter.
        /// </summary>
        /// <param name="instruction">
        /// The instruction to execute.
        /// </param>
        public void ExecuteInstruction(Action<TProxy> instruction)
        {
            this.EnsureProxyCreated();

            if (!this.SessionValid)
            {
                Log.Debug($"Session is invalid.");

                throw new SessionInvalidException(this.SessionOverdue);
            }

            if (instruction == null)
            {
                throw new ArgumentNullException(nameof(instruction));
            }

            try
            {
                instruction(this.Proxy);
            }
            catch (FaultException<SMaRTServiceFault>)
            {
                Log.Info("Error in calling the API.");
                throw;
            }
            catch (FaultException)
            {
                Log.Info("Error on Server.");
                throw;
            }
            catch (CommunicationException)
            {
                Log.Error($"Communication error happened. Closing connection.");

                this.Dispose();
                throw new SessionInvalidException(this.SessionOverdue);
            }
            catch (TimeoutException)
            {
                Log.Error($"Connection Timeout happened. Closing connection.");

                this.Dispose();
                throw new SessionInvalidException(this.SessionOverdue);
            }
            catch
            {
                this.Dispose();

                throw new SessionInvalidException(this.SessionOverdue);
            }
        }

        /// <summary>
        /// Creates a new proxy.
        /// </summary>
        private void EnsureProxyCreated()
        {
            if (this.ConnectionUseable)
            {
                return;
            }
            
            this.Proxy = this.Factory.CreateChannel();

            if (this.Channel.State < CommunicationState.Closing)
            {
                return;
            }

            Log.Fatal("Cannot create connection.");

            this.SessionIrreparable = true;
        }
    }
}

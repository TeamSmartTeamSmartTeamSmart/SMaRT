// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckExecutor.cs" company="Novomatic Gaming Industries GmbH">
//   Copyright 2017 Novomatic Gaming Industries GmbH.
// </copyright>
// <summary>
//   Class for executing PowerShell scripts on local machine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SMaRT.AgentService
{
    using System;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using log4net;

    using SMaRT.Shared;
    using SMaRT.Shared.Util;

    /// <summary>
    /// Class for executing PowerShell scripts on local machine.
    /// </summary>
    public class CheckExecutor : IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(CheckExecutor));
        
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        private PowerShell script;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckExecutor"/> class.
        /// </summary>
        public CheckExecutor()
        {
            this.Configuration = RunspaceConfiguration.Create();
            this.InitExecutionEnvironment();
        }
        
        /// <summary>
        /// Gets the output of the execution.
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Gets the return code.
        /// </summary>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public string Error { get; private set; }
        
        /// <summary>
        /// Sets the PowerShell script.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when the value given is null.
        /// </exception>
        public string Script
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.script = PowerShell.Create();
                this.script.Runspace = this.Runspace;
                this.script.AddScript(value);

                Log.Info($"Created new Powershell script");
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        private RunspaceConfiguration Configuration { get; }

        /// <summary>
        /// Gets or sets the run space.
        /// </summary>
        private Runspace Runspace { get; set; }

        /// <summary>
        /// Gets or sets the invoker.
        /// </summary>
        private RunspaceInvoke Invoker { get; set; }

        /// <summary>
        /// Gets or sets the pipeline.
        /// </summary>
        private Pipeline Pipeline { get; set; }

        /// <summary>
        /// Adds a parameter for the script. Before calling this function, SetScript has to be called.
        /// </summary>
        /// <param name="name">
        /// The parameter name.
        /// </param>
        /// <param name="value">
        /// The value of the parameter.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Is thrown when SetScript has not been called first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when the name is null.
        /// </exception>
        public void AddParameter(string name, object value)
        {
            if (this.script == null)
            {
                throw new InvalidOperationException("No script set. Set 'Script' first.");
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.script.AddParameter(name, value);

            Log.Info("Added new parameter.");
        }

        /// <summary>
        /// Executes the script and with the added arguments. Before calling this function, SetScript has to be called.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Is thrown when SetScript has not been called first.
        /// </exception>
        public void ExecuteScript()
        {
            if (this.script == null)
            {
                throw new InvalidOperationException("No script set. Set 'Script' first.");
            }

            this.Error = string.Empty;
            this.Output = string.Empty;

            try
            {
                var output = this.script.Invoke();

                this.Output = output.EachToString();
                this.Error = this.script.Streams.Error.EachToString();

                if (this.script.HadErrors)
                {
                    this.ReturnCode = ReturnCode.Error;
                }
                else
                {
                    var firstOrDefault = output.FirstOrDefault();
                    int code;
                    
                    if ((firstOrDefault != null)
                        && int.TryParse(firstOrDefault.ToString(), out code))
                    {
                        // First Line is a single number
                        switch (code)
                        {
                            case 1:
                                this.ReturnCode = ReturnCode.Success;
                                break;
                            case 2:
                                this.ReturnCode = ReturnCode.Warning;
                                break;
                            case 3:
                                this.ReturnCode = ReturnCode.Error;
                                break;
                            default:
                                this.ReturnCode = ReturnCode.Warning;
                                break;
                        }
                    }
                    else
                    {
                        // No return code given
                        this.ReturnCode = ReturnCode.Warning;
                    }
                }
            }
            catch (Exception e)
            {
                this.Error += $"script crashed with following exception: {e.Message}";

                Log.Error($"Unecpected error while executing script.");
            }

            this.script = null;

            Log.Info($"Successfully executed script.");
        }

        /// <summary>
        /// Dispose method for IDisposable interface implementation.
        /// </summary>
        public void Dispose()
        {
            this.Runspace?.Dispose();
            this.Invoker?.Dispose();
            this.Pipeline?.Dispose();
        }

        /// <summary>
        /// Initializes the PowerShell script execution environment.
        /// </summary>
        private void InitExecutionEnvironment()
        {
            this.Runspace = RunspaceFactory.CreateRunspace(this.Configuration);
            this.Runspace.Open();

            this.Invoker = new RunspaceInvoke(this.Runspace);

            this.Pipeline = this.Runspace.CreatePipeline();
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgentService.cs" company="Novomatic Gaming Industries GmbH">
//   Copyright 2017 Novomatic Gaming Industries GmbH.
// </copyright>
// <summary>
//   The agent service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SMaRT.AgentService
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceProcess;
    using System.Timers;

    using log4net;

    using Master.ServiceAgent;

    using Shared.ConnectionObjects;

    using SMaRT.Shared.Util;

    /// <summary>
    /// The agent service.
    /// </summary>
    public partial class AgentService : ServiceBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AgentService));

        /// <summary>
        /// The interval of the check pulling.
        /// </summary>
        private static double interval = 10 * 60 * 1000; // 10 Minutes

        /// <summary>
        /// The id.
        /// </summary>
        private static int id;

        /// <summary>
        /// The channel factory. Used to create channels to connect to the master.
        /// </summary>
        private readonly ChannelFactory<ICheckService> channelFactory;

        /// <summary>
        /// The timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentService"/> class.
        /// </summary>
        public AgentService()
        {
            this.InitializeComponent();
            this.channelFactory = new ChannelFactory<ICheckService>("CheckServiceEndpoint");
        }

        /// <summary>
        /// The entry point of the application
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        public static void Main(string[] args)
        {
            int settingsId;
            if (int.TryParse(ConfigurationManager.AppSettings["id"], out settingsId))
            {
                id = settingsId;
            }
            else
            {
                Log.Error("No valid id found in app.config");
            }

            if (id == 0)
            {
                Log.Error("Id [0] is an invalid id. Looks like the id is not configured.");
            }

            Log.Info($"Starting SMaRT agent service. Id is {id}");

            var service = new AgentService();
            
            if (Environment.UserInteractive)
            {
                interval = 30 * 1000; // 30 seconds
                service.OnStart(args);
                Console.WriteLine("Press any key to stop the program");
                Console.Read();
                service.OnStop();

                Log.Debug("Started in interactive session.");
            }
            else
            {
                int settingsInterval;
                if (int.TryParse(ConfigurationManager.AppSettings["interval"], out settingsInterval))
                {
                    interval = settingsInterval;
                }

                Log.Debug($"Set the request interval to [{interval/1000}] seconds.");

                ServiceBase.Run(service);

                Log.Debug("Started as Windows service");
            }
        }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        public void Init()
        {
            this.timer = new Timer { Interval = interval };
            this.timer.Elapsed += this.ExecuteInstructions;
            this.timer.Enabled = true;

            Log.Debug($"Initialized service with interval of {interval} milliseconds.");
        }

        /// <summary>
        /// The method to start the service.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected override void OnStart(string[] args)
        {
            this.Init();
            this.ExecuteInstructions(this, null);
        }

        /// <summary>
        /// The on stop.
        /// </summary>
        protected override void OnStop()
        {
            this.timer.Enabled = false;
            Log.Info("Service stopped");
        }

        /// <summary>
        /// Just a dummy name
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExecuteInstructions(object sender, ElapsedEventArgs e)
        {
            using (var session = new ConnectionSession<ICheckService>(interval, this.channelFactory))
            {
                try
                {
                    // Execute the following instruction "pull Instructions" on remote host.
                    var instructions = session.ExecuteInstruction(proxy => proxy.PullInstructions(id));

                    using (var executor = new CheckExecutor())
                    {
                        foreach (var checkInstruction in instructions)
                        {
                            // Execute the following instruction "pull code" on remote host.
                            var checkCode = session.ExecuteInstruction(proxy => proxy.PullInstructionCode(checkInstruction.CheckID));
                            Log.Debug($"Executing Check {checkInstruction.CheckID} in revision {checkInstruction.CheckRevisionNR}");

                            // hand over the code to the powershell execution instance with parameters
                            executor.Script = checkCode.Code;
                            var xmlElements = checkInstruction.Parameters?.Descendants();
                            xmlElements?.ForEach(
                                elem => executor.AddParameter(
                                    elem.Attributes().First().Value,
                                    elem.Value));

                            // execute the code
                            var startTime = DateTime.Now;
                            executor.ExecuteScript();
                            Log.Debug($"Successfully exected check {checkCode.CheckID}");
                            var endTime = DateTime.Now;

                            var manifestedReturnValue = new CheckReturn()
                                                        {
                                                            CheckID = checkInstruction.CheckID,
                                                            CheckRevisionNR = checkInstruction.CheckRevisionNR,
                                                            InstructionDate = checkInstruction.InstructionDate,
                                                            StartTime = startTime,
                                                            EndTime = endTime,
                                                            Output = executor.Output,
                                                            Error = executor.Error,
                                                            ReturnCode = (int)executor.ReturnCode
                                                        };
                            session.ExecuteInstruction(proxy => proxy.PushReturn(id, manifestedReturnValue));
                            Log.Debug($"Successfully returned result of check {checkInstruction.CheckID}");
                        }
                    }
                }
                catch (SessionInvalidException ex)
                {
                    Log.Error(
                        "The current session did fail. "
                        + (ex.SessionOverdue ? 
                            "The session was overdue." : 
                            "There was an connection issue."));
                }
                catch (FaultException<SMaRTServiceFault> ex)
                {
                    Log.Error($"There was an unhandled error of the API. Code: {ex.Code.Name} Details: {ex.Message}");
                }
                catch (FaultException ex)
                {
                    Log.Error($"There was an unhandled server-sided exception. Code: {ex.Code.Name} Details: {ex.Message}");
                }
            }
        }
    }
}

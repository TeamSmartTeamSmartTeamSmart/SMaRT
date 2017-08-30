namespace SMaRT.Master
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;

    using SMaRT.Master.ServiceAgent;
    using SMaRT.Master.ServiceDashboard;
    using SMaRT.Shared.ConnectionObjects;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(
                serviceConfig =>
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        serviceConfig.UseLog4Net();

                        serviceConfig.Service<MasterService>(
                            serviceInstance =>
                                {
                                    serviceInstance.ConstructUsing(() => new MasterService());
                                    serviceInstance.WhenStarted(execute => execute.OnStart());
                                    serviceInstance.WhenStopped(execute => execute.OnStop());
                                });

                        serviceConfig.EnableServiceRecovery(
                            recoveryOption =>
                                {
                                    recoveryOption.RestartService(1); // first failure
                                    recoveryOption.RestartService(2); // second failure
                                    recoveryOption.RestartService(3); // subsequent failure

                                    recoveryOption.SetResetPeriod(1); // reset fail count after one day
                                });

                        serviceConfig.SetServiceName("SMaRTMasterService");
                        serviceConfig.SetDisplayName("SMaRT Master Service");
                        serviceConfig.SetDescription("Master ServiceHost for the database "
                                                     + "connection and the services of SMaRT");
                    });
        }
    }
}

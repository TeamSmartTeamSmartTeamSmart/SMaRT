namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;

    using log4net;

    using SMaRT.Master.ServiceAgent;
    using SMaRT.Master.ServiceDashboard;

    using Topshelf.Logging;

    public class MasterService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MasterService));
        private static ServiceHost hostAgent;
        private static WebServiceHost hostRest;

        public bool OnStart()
        {
            Log.Info("SMaRT Master started.");
            AutoMapperConfiguration.Configure();

            hostAgent = new ServiceHost(typeof(CheckService));
            hostRest = new WebServiceHost(typeof(DashboardService));
            try
            {
                // Start agent service
                Log.Info("Agent Service is starting..." + hostRest.BaseAddresses.First().ToString());
                hostAgent.Open();
                Log.Info("Agent Service started correctly.");

                // Start rest service
                Log.Info("REST Service is starting...");
                hostRest.Open();
                Log.Info("REST Service started correctly.");

                return true;
            }
            catch (CommunicationException exc)
            {
                Console.WriteLine("An exception occurred: {0}", exc.Message);
                Log.Error("An exception occured!", exc);
                hostRest.Abort();
                hostAgent.Abort();
                Log.Fatal("REST Service aborted.");
                Log.Fatal("Agent Service aborted.");

                return false;
            }
        }

        public bool OnStop()
        {
            try
            {
                hostRest.Close();
                hostAgent.Close();
                Log.Info("REST Service stopped correctly.");
                Log.Info("Agent Service stopped correctly.");

                return true;
            }
            catch (CommunicationException exc)
            {
                Console.WriteLine("An exception occurred: {0}", exc.Message);
                Log.Error("An exception occured!", exc);
                hostRest.Abort();
                hostAgent.Abort();
                Log.Fatal("REST Service aborted.");
                Log.Fatal("Agent Service aborted.");

                return false;
            }
        }
    }
}

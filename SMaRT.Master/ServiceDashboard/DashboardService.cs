namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;

    using AutoMapper;

    using SMaRT.Shared.ConnectionObjects;

    public partial class DashboardService : IDashboardService
    {
        private readonly SMaRTModel entities;

        /// <summary>
        /// Initialize the EntityFramework-Connection
        /// </summary>
        public DashboardService()
        {
            this.entities = new SMaRTModel();
        }

        public void Options()
        {
            // For Origin: The "*" should really be a list of valid cross site domains for better security.
            // For Methods: The list should be the list of support methods for the endpoint.
            // For Allowed Headers: The list should be the supported header for your application.
            //
            WebOperationContext.Current?.OutgoingResponse.Headers.Add(
                "Access-Control-Allow-Methods",
                "POST, GET, OPTIONS, DELETE, PUT");
            WebOperationContext.Current?.OutgoingResponse.Headers.Add(
                "Access-Control-Allow-Headers",
                "Content-Type, Accept, Authorization");

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public DashboardService(SMaRTModel entities)
        {
            this.entities = entities;
        }
    }
}

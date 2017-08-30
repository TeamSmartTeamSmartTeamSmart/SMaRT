namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web.Script.Services;

    using SMaRT.Shared.ConnectionObjects;

    // [ServiceContract]
    public partial interface IDashboardService
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void Options();
    }
}

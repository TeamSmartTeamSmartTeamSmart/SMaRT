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
        // Routes for execution
        [OperationContract]
        [WebGet(
            UriTemplate = "/execution/{checkId},{entityId},{startTime}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO GetExecutionById(string checkId, string entityId, string startTime);

        [OperationContract]
        [WebGet(
            UriTemplate = "/execution/{checkId},{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO[] GetExecutionListById(string checkId, string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/execution/{checkId},{entityId}/newest",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO GetNewestExecutionById(string checkId, string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/execution/{checkId},{entityId}/from={fromTime}/to={toTime}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO[] GetExecutionListByIdInterval(string checkId, string entityId, string fromTime, string toTime);

        [OperationContract]
        [WebGet(
            UriTemplate = "/execution/{checkId},{entityId}/count={count}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO[] GetExecutionListByIdCount(string checkId, string entityId, string count);
    }
}

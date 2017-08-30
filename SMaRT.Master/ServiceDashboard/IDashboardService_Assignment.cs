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
        // Routes for assignment

        [OperationContract]
        [WebGet(
            UriTemplate = "/assignment",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO[] GetAssignmentList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/assignment/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO[] GetAssignmentRevisionList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/assignment/{checkId},{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO GetAssignmentById(string checkId, string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/assignment/{checkId},{entityId}/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO[] GetAllAssignmentRevisionsById(string checkId, string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/assignment/{checkId},{entityId}/executions",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO[] GetExecutionListOfAssignmentById(string checkId, string entityId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/assignment",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void AddAssignment(CheckAssignmentDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/assignment/{checkId},{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO UpdateAssignment(string checkId, string entityId, CheckAssignmentDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/assignment/{checkId},{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DeleteAssignment(string checkId, string entityId);
    }
}

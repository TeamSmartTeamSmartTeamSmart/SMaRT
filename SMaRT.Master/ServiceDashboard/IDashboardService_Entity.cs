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

    public partial interface IDashboardService
    {
        // Routes for CheckableEntity
        [OperationContract]
        [WebGet(
            UriTemplate = "/entity",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetEntityList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetEntityRevisionList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/agent",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetAgentList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/group",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetGroupList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/agent/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetAgentRevisionList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/group/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetGroupRevisionList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO GetEntityById(string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO[] GetAllEntityRevisionsById(string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}/assignments",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO[] GetAssignmentListOfEntityById(string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}/executions",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckExecutionDTO[] GetExecutionListOfEntityById(string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}/childs",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        GroupMembershipDTO[] GetChildtListOfEntityById(string entityId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/entity/{entityId}/parents",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        GroupMembershipDTO[] GetParentOfEntityById(string entityId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/entity",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void AddEntity(CheckableEntityDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/entity/{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckableEntityDTO UpdateEntity(string entityId, CheckableEntityDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/entity/{entityId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DeleteEntity(string entityId);
    }
}

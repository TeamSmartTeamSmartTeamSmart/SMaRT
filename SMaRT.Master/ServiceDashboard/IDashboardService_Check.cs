namespace SMaRT.Master.ServiceDashboard
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using SMaRT.Shared.ConnectionObjects;

    [ServiceContract]
    public partial interface IDashboardService
    {
        // Routes for Check
        [OperationContract]
        [WebGet(
            UriTemplate = "/check",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO[] GetCheckList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/check/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO[] GetCheckRevisionList();

        [OperationContract]
        [WebGet(
            UriTemplate = "/check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO GetCheckById(string checkId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/check/{checkId}/all",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO[] GetAllCheckRevisionsById(string checkId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/check/{checkId}/assignments",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckAssignmentDTO[] GetAssignmentListOfCheckById(string checkId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/check",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void AddCheck(CheckDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO UpdateCheck(string checkId, CheckDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DeleteCheck(string checkId);
    }
}

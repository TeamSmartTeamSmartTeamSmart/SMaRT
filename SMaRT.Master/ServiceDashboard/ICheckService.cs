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

    [ServiceContract]
    public interface ICheckService
    {
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/Check/",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Check> GetCheckList();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/Check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO GetCheckById(string checkId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/Check/",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void AddCheck(CheckDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/Check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CheckDTO UpdateCheck(string checkId, CheckDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/Check/{checkId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DeleteCheck(string checkId);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/Check/Assignment/{checkId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<CheckDTO> GetCheckAssignmentList(string checkId);
    }
}

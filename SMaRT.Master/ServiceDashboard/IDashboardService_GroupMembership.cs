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
        // Routes for GroupMembership
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/groupmembership",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void AddGroupMembership(GroupMembershipDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/groupmembership/{parentId},{childId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        GroupMembershipDTO UpdateGroupMembership(string parentId, string childId, GroupMembershipDTO c);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/groupmembership/{parentId},{childId}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DeleteGroupMembership(string parentId, string childId);
    }
}

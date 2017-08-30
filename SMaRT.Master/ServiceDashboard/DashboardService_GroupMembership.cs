namespace SMaRT.Master.ServiceDashboard
{
    using System.Linq;
    using System.Net;

    using AutoMapper;

    using SMaRT.Shared.ConnectionObjects;

    public partial class DashboardService
    {
        public void AddGroupMembership(GroupMembershipDTO c)
        {
            if (this.entities.CheckableEntities.Any(ce => ce.EntityID == c.ParentID && ce.IsActive)
                && this.entities.CheckableEntities.Any(ce => ce.EntityID == c.ChildID && ce.IsActive))
            {
                c.ParentRevisionNR = this.entities.CheckableEntities.Where(ce => ce.EntityID == c.ParentID && ce.IsActive).Max(ce => ce.RevisionNR);
                c.ChildRevisionNR = this.entities.CheckableEntities.Where(ce => ce.EntityID == c.ChildID && ce.IsActive).Max(ce => ce.RevisionNR);
                c.IsActive = true;

                var member = Mapper.Map<GroupMembershipDTO, GroupMembership>(c);

                member.IsNewest = true;

                this.entities.GroupMemberships.Add(member);
                this.entities.SaveChanges();

                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.Created);
            }

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
        }

        public void DeleteGroupMembership(string parentId, string childId)
        {
            int pId, cId;
            if (!int.TryParse(parentId, out pId) & !int.TryParse(childId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.CheckableEntities.Any(ce => ce.EntityID == pId && ce.IsActive)
                && this.entities.CheckableEntities.Any(ce => ce.EntityID == cId && ce.IsActive))
            {
                // Set last active to non active
                var lastActive = this.entities.GroupMemberships.SingleOrDefault(ca => ca.ParentID == pId && ca.ChildID == cId && ca.IsActive);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;

                    // Update Assignments
                    var allRelatedAssignments = this.entities.CheckAssignments.Where(ch => ch.IsActive && ch.CheckID == pId);
                    foreach (var ca in allRelatedAssignments)
                    {
                        ca.IsActive = false;
                    }
                }

                this.entities.SaveChanges();
            }

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.OK);
        }

        public GroupMembershipDTO UpdateGroupMembership(string parentId, string childId, GroupMembershipDTO gm)
        {
            int pId, cId;
            if (!int.TryParse(parentId, out pId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            if (!int.TryParse(childId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.GroupMemberships.Any(g => g.ChildID == cId && g.ParentID == pId))
            {
                // Set last active to non active
                var lastActive = this.entities.GroupMemberships.SingleOrDefault(g => g.ChildID == cId && g.ChildRevisionNR == gm.ChildRevisionNR && g.ParentID == pId && g.ParentRevisionNR == gm.ParentRevisionNR);
                if (lastActive != null)
                {
                    lastActive.IsActive = gm.IsActive;

                    // Insert into DB
                    var groupmembership = Mapper.Map<GroupMembershipDTO, GroupMembership>(gm);

                    this.entities.SaveChanges();
                }

                return gm;
            }
            else
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.NotFound);
                return null;
            }
        }
    }
}

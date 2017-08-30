namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using AutoMapper;
    using SMaRT.Shared.ConnectionObjects;

    public partial class DashboardService
    {
        public CheckableEntityDTO[] GetEntityList()
        {
            var lastRevisionEntities = this.entities.CheckableEntities.Where(ce => ce.IsNewest);
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(lastRevisionEntities.ToArray());
            return entityListMapped;
        }

        public CheckableEntityDTO[] GetEntityRevisionList()
        {
            var entityList = this.entities.CheckableEntities.ToArray();
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(entityList);
            return entityListMapped;
        }

        public CheckableEntityDTO GetEntityById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var entity = this.entities.CheckableEntities.SingleOrDefault(c => c.IsNewest && c.EntityID == id);
            return Mapper.Map<CheckableEntity, CheckableEntityDTO>(entity);
        }

        public CheckableEntityDTO[] GetAllEntityRevisionsById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var entity = this.entities.CheckableEntities.Where(c => c.EntityID == id).ToArray();
            return Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(entity);
        }

        public CheckAssignmentDTO[] GetAssignmentListOfEntityById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var assignments = this.entities.CheckableEntities.SingleOrDefault(c => c.IsNewest && c.EntityID == id)?.CheckAssignments.Where(ca => ca.IsNewest).ToArray();
            var assignmentListMapped = Mapper.Map<CheckAssignment[], CheckAssignmentDTO[]>(assignments);
            return assignmentListMapped;
        }

        public CheckExecutionDTO[] GetExecutionListOfEntityById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var executions = this.entities.CheckableEntities.SingleOrDefault(c => c.EntityID == id)?.CheckExecutions.ToArray();
            var executionListMapped = Mapper.Map<CheckExecution[], CheckExecutionDTO[]>(executions);
            return executionListMapped;
        }

        public GroupMembershipDTO[] GetChildtListOfEntityById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var childs = this.entities.CheckableEntities.SingleOrDefault(c => c.IsNewest && c.EntityID == id)?.ParentOf.Where(p => p.IsNewest).ToArray();
            var childListMapped = Mapper.Map<GroupMembership[], GroupMembershipDTO[]>(childs);
            return childListMapped;
        }

        public GroupMembershipDTO[] GetParentOfEntityById(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var parents = this.entities.CheckableEntities.SingleOrDefault(c => c.IsNewest && c.EntityID == id)?.ChildOf.Where(c => c.IsNewest).ToArray();
            var parentListMapped = Mapper.Map<GroupMembership[], GroupMembershipDTO[]>(parents);
            return parentListMapped;
        }

        public CheckableEntityDTO[] GetAgentList()
        {
            var entityList = this.entities.CheckableEntities.Where(c => c.IsNewest).ToList();
            List<CheckableEntity> agentList = new List<CheckableEntity>();
            foreach (var entity in entityList)
            {
                // If no relation to GroupMembership than it's an Agent
                if (entity.ParentOf.Count == 0)
                {
                    agentList.Add(entity);
                }
            }
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(agentList.ToArray());
            return entityListMapped;
        }

        public CheckableEntityDTO[] GetGroupList()
        {
            var entityList = this.entities.CheckableEntities.Where(c => c.IsNewest).ToList();
            List<CheckableEntity> groupList = new List<CheckableEntity>();
            foreach (var entity in entityList)
            {
                // If relation to GroupMemmership than it's a Group
                if (entity.ParentOf.Count > 0)
                {
                    groupList.Add(entity);
                }
            }
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(groupList.ToArray());
            return entityListMapped;
        }

        public CheckableEntityDTO[] GetAgentRevisionList()
        {
            var entityList = this.entities.CheckableEntities.ToList();
            List<CheckableEntity> agentList = new List<CheckableEntity>();
            foreach (var entity in entityList)
            {
                // If no relation to GroupMemmership than it's an Agent
                if (entity.ParentOf.Count == 0)
                {
                    agentList.Add(entity);
                }
            }
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(agentList.ToArray());
            return entityListMapped;
        }

        public CheckableEntityDTO[] GetGroupRevisionList()
        {
            var entityList = this.entities.CheckableEntities.ToList();
            List<CheckableEntity> groupList = new List<CheckableEntity>();
            foreach (var entity in entityList)
            {
                // If relation to GroupMemmership than it's a Group
                if (entity.ParentOf.Count > 0)
                {
                    groupList.Add(entity);
                }
            }
            var entityListMapped = Mapper.Map<CheckableEntity[], CheckableEntityDTO[]>(groupList.ToArray());
            return entityListMapped;
        }

        public void AddEntity(CheckableEntityDTO c)
        {
            c.EntityID = this.entities.CheckableEntities.Max(ce => ce.EntityID) + 1;
            c.RevisionNR = 1;
            c.IsActive = true;
            c.FromDate = DateTime.Now;

            var check = Mapper.Map<CheckableEntityDTO, CheckableEntity>(c);

            check.IsNewest = true;

            this.entities.CheckableEntities.Add(check);
            this.entities.SaveChanges();

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.Created);
        }

        public CheckableEntityDTO UpdateEntity(string entityId, CheckableEntityDTO c)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.CheckableEntities.Any(ce => ce.EntityID == id))
            {
                // Set last active to non active
                var lastActive = this.entities.CheckableEntities.SingleOrDefault(ce => ce.EntityID == id && ce.IsNewest);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;
                    lastActive.IsNewest = false;

                    // Do the changes
                    c.EntityID = id;
                    c.RevisionNR = lastActive.RevisionNR + 1;
                    c.IsActive = true;
                    c.FromDate = DateTime.Now;

                    // Insert into DB
                    var entity = Mapper.Map<CheckableEntityDTO, CheckableEntity>(c);

                    entity.IsNewest = true;

                    this.entities.CheckableEntities.Add(entity);

                    // Update Assignments
                    var allRelatedAssignments = this.entities.CheckAssignments.Where(ch => ch.IsActive && ch.CheckID == id);
                    foreach (var ca in allRelatedAssignments)
                    {
                        ca.IsActive = false;
                        ca.IsNewest = false;

                        CheckAssignment newAssignment = new CheckAssignment()
                                                            {
                                                                Check = ca.Check,
                                                                EntityID = ca.EntityID,
                                                                EntityRevisionNR = lastActive.RevisionNR + 1,
                                                                RevisionNR = ca.RevisionNR + 1,
                                                                Interval = ca.Interval,
                                                                FromDate = ca.FromDate,
                                                                Parameters = ca.Parameters,
                                                                IsActive = ca.IsActive,
                                                                IsNewest = true
                                                            };
                        this.entities.CheckAssignments.Add(newAssignment);
                    }

                    // Update Groups - Parent
                    var parentFrom = lastActive.ParentOf.AsQueryable();
                    foreach (var parent in parentFrom)
                    {
                        parent.IsActive = false;
                        parent.IsNewest = false;

                        GroupMembership member = new GroupMembership()
                                                     {
                                                         Parent = entity,
                                                         Child = parent.Child,
                                                         IsActive = true,
                                                         IsNewest = true
                                                     };
                        this.entities.GroupMemberships.Add(member);
                    }

                    // Update Groups - Child
                    var childOf = lastActive.ChildOf.AsQueryable();
                    foreach (var child in childOf)
                    {
                        child.IsActive = false;
                        child.IsNewest = false;

                        GroupMembership member = new GroupMembership()
                                                     {
                                                         Parent = child.Parent,
                                                         Child = entity,
                                                         IsActive = true,
                                                         IsNewest = true
                                                     };
                        this.entities.GroupMemberships.Add(member);
                    }

                    // Save result
                    this.entities.SaveChanges();
                    return c;
                }
                return null;
            }
            else
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.NotFound);
                return null;
            }
        }

        public void DeleteEntity(string entityId)
        {
            int id;
            if (!int.TryParse(entityId, out id))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.CheckableEntities.Any(ch => ch.EntityID == id))
            {
                // Set last active to non active
                var lastActive = this.entities.CheckableEntities.SingleOrDefault(ch => ch.EntityID == id && ch.IsActive && ch.IsActive);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;

                    // Update Assignments
                    var allRelatedAssignments = this.entities.CheckAssignments.Where(ch => ch.IsActive && ch.CheckID == id);
                    foreach (var ca in allRelatedAssignments)
                    {
                        ca.IsActive = false;
                    }
                    this.entities.SaveChanges();
                }
            }

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.OK);
        }
    }
}

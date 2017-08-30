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
        /// <summary>
        /// Get the last revision of every assignment.
        /// </summary>
        /// <returns>Array of <see cref="CheckAssignmentDTO"/> objects</returns>
        public CheckAssignmentDTO[] GetAssignmentList()
        {
            var lastRevisionAssignments = this.entities.CheckAssignments.Where(ca => ca.IsNewest);
            var assignmentListMapped = Mapper.Map<CheckAssignment[], CheckAssignmentDTO[]>(lastRevisionAssignments.ToArray());
            return assignmentListMapped;
        }

        /// <summary>
        /// Get all assignments including the revisions.
        /// </summary>
        /// <returns>Array of <see cref="CheckAssignmentDTO"/> objects</returns>
        public CheckAssignmentDTO[] GetAssignmentRevisionList()
        {
            var assignmentList = this.entities.CheckAssignments.ToArray();
            var assignmentListMapped = Mapper.Map<CheckAssignment[], CheckAssignmentDTO[]>(assignmentList);
            return assignmentListMapped;
        }

        /// <summary>
        /// Get the active assignment for a specific checkId and entityId.
        /// </summary>
        /// <param name="checkId">Id of a check</param>
        /// <param name="entityId">Id of an entity</param>
        /// <returns><see cref="CheckAssignmentDTO"/> object</returns>
        public CheckAssignmentDTO GetAssignmentById(string checkId, string entityId)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var assignment = this.entities.CheckAssignments.SingleOrDefault(c => c.IsNewest && c.CheckID == cId && c.EntityID == eId);
            return Mapper.Map<CheckAssignment, CheckAssignmentDTO>(assignment);
        }

        /// <summary>
        /// Get all assignments and revisions for a specific checkId and entityId.
        /// </summary>
        /// <param name="checkId">Id of a check</param>
        /// <param name="entityId">id of a entity</param>
        /// <returns>Array of <see cref="CheckAssignmentDTO"/> objects</returns>
        public CheckAssignmentDTO[] GetAllAssignmentRevisionsById(string checkId, string entityId)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var assignment = this.entities.CheckAssignments.Where(c => c.CheckID == cId && c.EntityID == eId).ToArray();
            return Mapper.Map<CheckAssignment[], CheckAssignmentDTO[]>(assignment);
        }

        /// <summary>
        /// Get all related executions of an assignment specified by checkId and entityId.
        /// </summary>
        /// <param name="checkId">Id of a check</param>
        /// <param name="entityId">Id of an entity</param>
        /// <returns>Array of <see cref="CheckAssignmentDTO"/> objects</returns>
        public CheckExecutionDTO[] GetExecutionListOfAssignmentById(string checkId, string entityId)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var executions = this.entities.CheckAssignments.SingleOrDefault(c => c.IsActive && c.CheckID == cId && c.EntityID == eId)?.CheckExecutions.ToArray();
            var executionListMapped = Mapper.Map<CheckExecution[], CheckExecutionDTO[]>(executions);
            return executionListMapped;
        }

        /// <summary>
        /// Add assignment to database.
        /// RevisionNRs, IsActive and the FromDate set the method automatically.
        /// </summary>
        /// <param name="c"><see cref="CheckAssignmentDTO"/> object</param>
        public void AddAssignment(CheckAssignmentDTO c)
        {
            if (this.entities.Checks.Any(ch => ch.CheckID == c.CheckID && ch.IsActive)
                && this.entities.CheckableEntities.Any(ce => ce.EntityID == c.EntityID && ce.IsActive))
            {
                c.CheckRevisionNR = this.entities.Checks.Where(ch => ch.CheckID == c.CheckID && ch.IsActive).Max(ch => ch.RevisionNR);
                c.EntityRevisionNR = this.entities.CheckableEntities.Where(ce => ce.EntityID == c.EntityID && ce.IsActive).Max(ce => ce.RevisionNR);
                c.RevisionNR = 1;
                c.IsActive = true;
                c.FromDate = DateTime.Now;

                var assignment = Mapper.Map<CheckAssignmentDTO, CheckAssignment>(c);

                assignment.IsNewest = true;

                this.entities.CheckAssignments.Add(assignment);
                this.entities.SaveChanges();

                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.Created);
            }

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Update an existing assignment.
        /// </summary>
        /// <param name="checkId">Id of the check</param>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="c"><see cref="CheckAssignmentDTO"/></param>
        /// <returns>Return the updated <see cref="CheckAssignmentDTO"/> object</returns>
        public CheckAssignmentDTO UpdateAssignment(string checkId, string entityId, CheckAssignmentDTO c)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.CheckAssignments.Any(ca => ca.CheckID == cId && ca.EntityID == eId))
            {
                // Set last active to non active
                var last = this.entities.CheckAssignments.SingleOrDefault(ca => ca.CheckID == cId && ca.EntityID == eId && ca.IsNewest);
                if (last != null)
                {
                    last.IsActive = false;
                    last.IsNewest = false;

                    // Do the changes
                    var singleOrDefaultCheck =
                        this.entities.Checks.SingleOrDefault(ch => ch.CheckID == cId && ch.IsNewest);
                    var singleOrDefaultEntity =
                        this.entities.CheckableEntities.SingleOrDefault(ce => ce.EntityID == eId && ce.IsNewest);
                    if (singleOrDefaultCheck != null && singleOrDefaultEntity != null)
                    {
                        c.Check = null;
                        c.CheckableEntity = null;

                        var assignment = Mapper.Map<CheckAssignmentDTO, CheckAssignment>(c);

                        assignment.CheckRevisionNR = singleOrDefaultCheck.RevisionNR;
                        assignment.EntityRevisionNR = singleOrDefaultEntity.RevisionNR;

                        assignment.RevisionNR = last.RevisionNR + 1;

                        assignment.IsActive = true;
                        assignment.IsNewest = true;
                        assignment.FromDate = DateTime.Now;

                        // Insert into DB
                        this.entities.CheckAssignments.Add(assignment);

                        this.entities.SaveChanges();

                        return Mapper.Map<CheckAssignment, CheckAssignmentDTO>(assignment);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.NotFound);
                return null;
            }
        }

        /// <summary>
        /// Delete a existing assignment.
        /// </summary>
        /// <param name="checkId">Id of a check</param>
        /// <param name="entityId">id of an entity</param>
        public void DeleteAssignment(string checkId, string entityId)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.CheckAssignments.Any(ca => ca.CheckID == cId && ca.EntityID == eId))
            {
                // Set last active to non active
                var lastActive = this.entities.CheckAssignments.SingleOrDefault(ca => ca.CheckID == cId && ca.EntityID == eId && ca.IsActive);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;
                }

                this.entities.SaveChanges();
            }

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.OK);
        }
    }
}

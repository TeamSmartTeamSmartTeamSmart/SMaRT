namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using AutoMapper;

    using SMaRT.Shared.ConnectionObjects;

    public partial class DashboardService : IDashboardService
    {
        /// <summary>
        /// Get the last revision of every check.
        /// </summary>
        /// <returns>Array of <see cref="CheckDTO"/> objects</returns>
        public CheckDTO[] GetCheckList()
        {
            var lastRevisionChecks = this.entities.Checks.Where(c => c.IsNewest);
            var checkListMapped = Mapper.Map<Check[], CheckDTO[]>(lastRevisionChecks.ToArray());
            return checkListMapped;
        }

        /// <summary>
        /// Get all checks including the revisions.
        /// </summary>
        /// <returns>Array of <see cref="CheckDTO"/> objects</returns>
        public CheckDTO[] GetCheckRevisionList()
        {
            var checkList = this.entities.Checks.ToArray();
            var checkListMapped = Mapper.Map<Check[], CheckDTO[]>(checkList);
            return checkListMapped;
        }

        /// <summary>
        /// Get the active check for a specific checkId.
        /// </summary>
        /// <param name="checkId">Id of a check</param>
        /// <returns><see cref="CheckDTO"/> object</returns>
        public CheckDTO GetCheckById(string checkId)
        {
            int cId;
            if (!int.TryParse(checkId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            var check = this.entities.Checks.SingleOrDefault(c => c.IsNewest && c.CheckID == cId);
            return Mapper.Map<Check, CheckDTO>(check);
        }

        /// <summary>
        /// Get all checks and revisions for a specific checkId.
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public CheckDTO[] GetAllCheckRevisionsById(string checkId)
        {
            int cId;
            if (!int.TryParse(checkId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            var checkList = this.entities.Checks.Where(c => c.CheckID == cId).ToArray();
            var checkListMapped = Mapper.Map<Check[], CheckDTO[]>(checkList);
            return checkListMapped;
        }

        public CheckAssignmentDTO[] GetAssignmentListOfCheckById(string checkId)
        {
            int cId;
            if (!int.TryParse(checkId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            var assignments = this.entities.Checks.SingleOrDefault(c => c.IsNewest && c.CheckID == cId)?.CheckAssignments.Where(ca => ca.IsNewest).ToArray();
            var assignmentListMapped = Mapper.Map<CheckAssignment[], CheckAssignmentDTO[]>(assignments);
            return assignmentListMapped;
        }

        public void AddCheck(CheckDTO c)
        {
            c.CheckID = this.entities.Checks.Max(ch => ch.CheckID) + 1;
            c.RevisionNR = 1;
            c.IsActive = true;
            c.FromDate = DateTime.Now;

            var check = Mapper.Map<CheckDTO, Check>(c);

            check.IsNewest = true;

            this.entities.Checks.Add(check);
            this.entities.SaveChanges();

            HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.Created);
        }

        public CheckDTO UpdateCheck(string checkId, CheckDTO c)
        {
            int cId;
            if (!int.TryParse(checkId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.Checks.Any(ch => ch.CheckID == cId))
            {
                // Set last active to non active
                var lastActive = this.entities.Checks.SingleOrDefault(ch => ch.CheckID == cId && ch.IsNewest);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;
                    lastActive.IsNewest = false;

                    // Do the changes
                    c.CheckID = cId;
                    c.RevisionNR = lastActive.RevisionNR + 1;
                    c.IsActive = true;
                    c.FromDate = DateTime.Now;

                    // Insert into DB
                    var check = Mapper.Map<CheckDTO, Check>(c);

                    check.IsNewest = true;

                    this.entities.Checks.Add(check);

                    // Update Assignments
                    var allRelatedAssignments = this.entities.CheckAssignments.Where(ch => ch.IsActive && ch.CheckID == cId);
                    foreach (var ca in allRelatedAssignments)
                    {
                        ca.IsActive = false;
                        ca.IsNewest = false;

                        CheckAssignment newAssignment = new CheckAssignment()
                                                            {
                                                                CheckID = ca.CheckID,
                                                                CheckRevisionNR = lastActive.RevisionNR + 1,
                                                                CheckableEntity = ca.CheckableEntity,
                                                                RevisionNR = ca.RevisionNR  + 1,
                                                                Interval = ca.Interval,
                                                                FromDate = ca.FromDate,
                                                                Parameters = ca.Parameters,
                                                                IsActive = ca.IsActive,
                                                                IsNewest = true
                                                            };
                        this.entities.CheckAssignments.Add(newAssignment);
                    }
                }
                this.entities.SaveChanges();

                return c;
            }
            else
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.NotFound);
                return null;
            }
        }

        public void DeleteCheck(string checkId)
        {
            int cId;
            if (!int.TryParse(checkId, out cId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }

            if (this.entities.Checks.Any(ch => ch.CheckID == cId))
            {
                // Set last active to non active
                var lastActive = this.entities.Checks.SingleOrDefault(ch => ch.CheckID == cId && ch.IsActive);
                if (lastActive != null)
                {
                    lastActive.IsActive = false;

                    // Update Assignments
                    var allRelatedAssignments = this.entities.CheckAssignments.Where(ch => ch.IsActive && ch.CheckID == cId);
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

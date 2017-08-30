namespace SMaRT.Master.ServiceAgent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using AutoMapper;

    using log4net;

    using SMaRT.Shared.ConnectionObjects;

    [CheckServiceBehavior]
    public class CheckService : ICheckService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MasterService));
        private readonly SMaRTModel entities;

        /// <summary>
        /// Initializes a new instanze of the CheckService class.
        /// </summary>
        public CheckService()
        {
            this.entities = new SMaRTModel();
        }

        /// <summary>
        /// Searches for all checks, which have to been done by the provided agent.
        /// </summary>
        /// <param name="agentId">ID from an Agent</param>
        /// <returns>
        /// Returns the IDs from the founded checks as IEnumerable
        /// </returns>
        public IEnumerable<CheckInstruction> PullInstructions(int agentId)
        {
            Log.InfoFormat("Agent with ID {0} wants to pull its instructions.", agentId);
            // Stores instructions which have to be done
            List<CheckInstruction> instructions = new List<CheckInstruction>();

            var agent = this.entities.CheckableEntities.SingleOrDefault(a => a.EntityID == agentId && a.IsActive);

            if (agent == null)
            {
                Log.InfoFormat("No (active) agent for provided id {0} found.", agentId);
                throw new FaultException<SMaRTServiceFault>(
                    new SMaRTServiceFault(FaultCodes.NotFound, "No (active) agent for provided ID found"), "No (active) agent for provided ID found");
            }

            //All assignments - Agent or Groups
            var assignments =
                this.entities.CheckAssignments.Where(
                    ca =>
                        ca.IsActive
                        && ((ca.EntityID == agent.EntityID && ca.EntityRevisionNR == agent.RevisionNR)
                            || ca.CheckableEntity.ParentOf.Any(
                                gm =>
                                    gm.ChildID == agent.EntityID && gm.ChildRevisionNR == agent.RevisionNR
                                    && gm.IsActive))).ToList();

            DateTime now = DateTime.Now;
            foreach (var assignment in assignments)
            {
                if (HelpServiceMethods.HasToExecute(assignment))
                {
                    // Save Execution to DB
                    CheckExecution tmpExe = new CheckExecution();
                    tmpExe.CheckAssignment = assignment;
                    tmpExe.CheckableEntity = agent;
                    tmpExe.InstructionTime = now;
                    this.entities.CheckExecutions.Add(tmpExe);
                    this.entities.SaveChanges();

                    // add instruction to list
                    var checkInstruction = Mapper.Map<CheckInstruction>(assignment);
                    checkInstruction.InstructionDate = now;
                    instructions.Add(checkInstruction);

                    Log.InfoFormat("Agent with ID {0} gets Check with ID {1}", agentId, checkInstruction.CheckID);
                }
            }
            return instructions.Distinct();
        }

        /// <summary>
        /// Searches for the check which is specified from the parameter.
        /// </summary>
        /// <param name="checkId">ID from a Check</param>
        /// <returns>Returns an object of CheckCode</returns>
        public CheckCode PullInstructionCode(int checkId)
        {
            if (this.entities.Checks.Any(ch => ch.CheckID == checkId && ch.IsActive))
            {
                var code =
                    this.entities.Checks.Single(
                        ch =>
                            ch.IsActive &&
                            ch.CheckID == checkId);

                Log.InfoFormat("Code for Check with ID {0} is returned to agent.", checkId);

                return Mapper.Map<CheckCode>(code);
            }
            Log.InfoFormat("No (active) Check for provided Id {0} found", checkId);

            throw new FaultException<SMaRTServiceFault>(
                new SMaRTServiceFault(FaultCodes.NotFound, "No (active) Check for provided Id found"));
        }

        /// <summary>
        /// Saves the 
        /// </summary>
        /// <param name="checkId">ID from a Check</param>
        /// <returns>Returns an object of CheckCode</returns>
        public void PushReturn(int agentId, CheckReturn ret)
        {
            if (this.entities.CheckableEntities.Any(ce => ce.EntityID == agentId && ce.IsActive))
            {
                Log.InfoFormat("Agent with ID {0} push result(s) for Check with ID {1} (Return Code: {2})", agentId, ret.CheckID, ret.ReturnCode);

                var assignments =
                    this.entities.CheckAssignments.Where(
                        ca =>
                            ((ca.EntityID == agentId && ca.IsActive)
                             || ca.CheckableEntity.ParentOf.Any(gm => gm.ChildID == agentId && gm.IsActive))
                            && (ca.CheckID == ret.CheckID && ca.IsActive));

                foreach (var assignment in assignments)
                {
                    int agentRevisionNR =
                        this.entities.CheckableEntities.Single(ce => ce.IsActive && ce.EntityID == assignment.EntityID)
                            .RevisionNR;

                    var tmpExe = this.entities.CheckExecutions.Single(
                        ce => ce.CheckID == assignment.CheckID
                        && ce.CheckRevisionNR == assignment.CheckRevisionNR
                        && ce.AssignedEntityID == assignment.EntityID
                        && ce.AssignedEntityRevisionNR == assignment.EntityRevisionNR
                        && ce.AssignmentRevisionNR == assignment.RevisionNR
                        && ce.ExecutedEntityID == agentId
                        && ce.ExecutedEntityRevisionNR == agentRevisionNR
                        && ce.InstructionTime == ret.InstructionDate);

                    tmpExe.StartTime = ret.StartTime;
                    tmpExe.EndTime = ret.EndTime;
                    tmpExe.ReturnCode = ret.ReturnCode;
                    tmpExe.Output = ret.Output;
                    tmpExe.Error = ret.Error;

                    Log.InfoFormat("Agent with ID {0} push Result for Check with ID {1} to Entity with ID {2}", agentId, ret.CheckID, assignment.EntityID);
                }
                this.entities.SaveChanges();
            }
            else
            {
                Log.InfoFormat($"No (active) Agent for provided Id found", agentId);
                throw new FaultException<SMaRTServiceFault>(
                    new SMaRTServiceFault(FaultCodes.NotFound, "No (active) Agent for provided ID found"));
            }
        }
    }
}
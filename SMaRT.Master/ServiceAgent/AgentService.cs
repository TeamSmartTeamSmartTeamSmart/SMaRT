namespace SMaRT.Master.ServiceAgent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using AutoMapper;
    using SMaRT.Shared.ConnectionObjects;

    public class AgentService : IAgentService
    {
        private readonly SMaRTEntities entities;

        /// <summary>
        /// Initializes a new instanze of the CheckService class.
        /// </summary>
        public AgentService()
        {
            this.entities = new SMaRTEntities();
        }

        /// <summary>
        /// Searches for all checks, which have to been done by the provided agent.
        /// </summary>
        /// <param name="agentId">ID from a Agent</param>
        /// <returns>
        /// Returns the IDs from the founded checks as IEnumerable
        /// </returns>
        public IEnumerable<CheckInstruction> PullInstructions(int agentId)
        {
            // Stores instructions which have to be done
            List<CheckInstruction> instructions = new List<CheckInstruction>();

            var agent =
                this.entities.CheckableEntities.SingleOrDefault(
                    a =>
                        a.EntityID == agentId &&
                        a.IsActive);

            if (agent == null)
            {
                throw new FaultException<SMaRTServiceFault>(
                    new SMaRTServiceFault(FaultCodes.NotFound, "No (active) agent for provided id found"));
            }

            // Assignments to Agent
            var assignmentsAgent =
                agent.CheckAssignments.Where(
                    ca =>
                        ca.IsActive);

            foreach (var assignment in assignmentsAgent)
            {
                if (HelpServiceMethods.HasToExecute(assignment))
                {
                    instructions.Add(Mapper.Map<CheckInstruction>(assignment));
                }
            }

            // Assignment to Groupmemberships
            var memberships =
                agent.ChildOf.Where(
                    gm =>
                        gm.IsActive);

            foreach (var member in memberships)
            {
                var assignmentsGroup =
                    member.Parent.CheckAssignments.Where(
                        ca =>
                            ca.IsActive);

                foreach (var assignment in assignmentsGroup)
                {
                    if (HelpServiceMethods.HasToExecute(assignment))
                    {
                        instructions.Add(Mapper.Map<CheckInstruction>(assignment));
                    }
                }
            }

            return instructions;
        }

        /// <summary>
        /// Searches for the check which ist specified from the parameter.
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
                            ch.IsActive);

                return Mapper.Map<CheckCode>(code);
            }

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
                var checkReturn = Mapper.Map<CheckExecution>(ret);

                var assignments =
                    this.entities.CheckAssignments.Where(
                        ca =>
                            (ca.EntityID == agentId && ca.IsActive) ||
                            ca.CheckableEntity.ParentsFrom.Any(gm => gm.ChildID == agentId && gm.IsActive));

                foreach (var assignment in assignments)
                {
                    checkReturn.CheckAssignment = assignment;

                    checkReturn.ExecutedEntityID = agentId;
                    checkReturn.ExecutedEntityRevisionNR = this.entities.CheckableEntities.Single(
                                                                ce =>
                                                                    ce.IsActive).RevisionNR;
                }
            }

            throw new FaultException<SMaRTServiceFault>(
                new SMaRTServiceFault(FaultCodes.NotFound, "No (active) Agent for provided Id found"));
        }
    }
}
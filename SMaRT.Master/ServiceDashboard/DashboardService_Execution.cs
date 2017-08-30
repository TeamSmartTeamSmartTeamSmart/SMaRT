namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Linq;
    using System.Net;

    using AutoMapper;

    using SMaRT.Shared.ConnectionObjects;

    public partial class DashboardService
    {
        public CheckExecutionDTO GetExecutionById(string checkId, string entityId, string instructionTime)
        {
            DateTime instruction;
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId) & !DateTime.TryParse(instructionTime, out instruction))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var execution = this.entities.CheckExecutions.SingleOrDefault(c => c.CheckID == cId && (c.ExecutedEntityID == eId || c.AssignedEntityID == eId) && c.InstructionTime == instruction);
            return Mapper.Map<CheckExecution, CheckExecutionDTO>(execution);
        }

        public CheckExecutionDTO[] GetExecutionListById(string checkId, string entityId)
        {
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var execution = this.entities.CheckExecutions.Where(c => c.CheckID == cId && (c.ExecutedEntityID == eId || c.AssignedEntityID == eId)).ToArray();
            return Mapper.Map<CheckExecution[], CheckExecutionDTO[]>(execution);
        }

        public CheckExecutionDTO GetNewestExecutionById(string checkId, string entityId)
        {
            return this.GetExecutionListByIdCount(checkId, entityId, "1").FirstOrDefault();
        }

        public CheckExecutionDTO[] GetExecutionListByIdInterval(string checkId, string entityId, string fromTime, string toTime)
        {
            DateTime from, to;
            int cId, eId;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId) & !DateTime.TryParse(fromTime, out from) & !DateTime.TryParse(toTime, out to))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var executionList = this.entities.CheckExecutions.Where(c => c.CheckID == cId && (c.ExecutedEntityID == eId || c.AssignedEntityID == eId) && c.StartTime >= from && c.StartTime <= to).ToArray();
            return Mapper.Map<CheckExecution[], CheckExecutionDTO[]>(executionList);
        }

        public CheckExecutionDTO[] GetExecutionListByIdCount(string checkId, string entityId, string count)
        {
            int cId, eId, amount;
            if (!int.TryParse(checkId, out cId) & !int.TryParse(entityId, out eId) & !int.TryParse(count, out amount))
            {
                HelpServiceMethods.SetResponseHttpStatus(HttpStatusCode.BadRequest);
            }
            var executionList = this.entities.CheckExecutions.Where(c => c.CheckID == cId && (c.ExecutedEntityID == eId || c.AssignedEntityID == eId)).OrderByDescending(c => c.StartTime).Take(amount).ToArray();
            return Mapper.Map<CheckExecution[], CheckExecutionDTO[]>(executionList);
        }
    }
}

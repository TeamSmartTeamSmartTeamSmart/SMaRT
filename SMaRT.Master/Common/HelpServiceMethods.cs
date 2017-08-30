namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;

    public static class HelpServiceMethods
    {
        public static bool HasToExecute(CheckAssignment assignment)
        {
            if (assignment.CheckExecutions.Any())
            {
                DateTime lastStartTime = assignment.CheckExecutions.OrderBy(ce => ce.StartTime).Last().InstructionTime;

                if (lastStartTime.AddSeconds(assignment.Interval) <= DateTime.Now)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public static void SetResponseHttpStatus(HttpStatusCode statusCode)
        {
            var context = WebOperationContext.Current;
            if (context != null)
            {
                context.OutgoingResponse.StatusCode = statusCode;
            }
        }
    }
}

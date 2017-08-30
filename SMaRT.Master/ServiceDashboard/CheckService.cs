namespace SMaRT.Master.ServiceDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Text;

    using AutoMapper;

    using SMaRT.Shared.ConnectionObjects;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CheckService" in both code and config file together.
    public class CheckService : ICheckService
    {
        private readonly SMaRTEntities entities;

        public CheckService()
        {
            this.entities = new SMaRTEntities();
        }

        public List<Check> GetCheckList()
        {
            // var checkList = this.entities.Checks.Where(c => c.IsActive).ToList();
            return this.entities.Checks.Where(c => c.IsActive).ToList();
        }

        public CheckDTO GetCheckById(string checkId)
        {
            throw new NotImplementedException();
        }

        public void AddCheck(CheckDTO c)
        {
            throw new NotImplementedException();
        }

        public CheckDTO UpdateCheck(string checkId, CheckDTO c)
        {
            throw new NotImplementedException();
        }

        public void DeleteCheck(string checkId)
        {
            throw new NotImplementedException();
        }

        public List<CheckDTO> GetCheckAssignmentList(string checkId)
        {
            throw new NotImplementedException();
        }
    }
}

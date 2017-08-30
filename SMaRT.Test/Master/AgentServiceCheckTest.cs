namespace SMaRT.Test.Master.Test
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using EntityFramework.FakeItEasy;

    using FakeItEasy;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NUnit.Framework;
    using SMaRT.Master;
    using SMaRT.Master.ServiceAgent;
    using SMaRT.Master.ServiceDashboard;
    using SMaRT.Shared.ConnectionObjects;
    using SMaRT.Shared.Util;

    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class AgentServiceCheckTest : AgentServiceGeneralTest
    {
        [Test]
        public void GetCheckRevisionListTest()
        {
            var c = this.dashboardService.GetCheckRevisionList().ToList();

            Assert.AreEqual(4, c.Count, "Should have 4");
        }

        [Test]
        public void GetCheckListTest()
        {
            var c = this.dashboardService.GetCheckList().ToList();

            Assert.AreEqual(3, c.Count, "Should have 3 Checks where IsNewest is true");
        }

        [Test]
        public void GetCheckByIdTestValidArgument()
        {
            var c = this.dashboardService.GetCheckById("1");

            Assert.AreEqual(1, c.CheckID, "Check should have ID 1");
        }

        [Test]
        public void GetCheckByIdTestInvalidArgument()
        {
            var c = this.dashboardService.GetCheckById("a");

            Assert.IsNull(c, "Check should be null");
        }

        [Test]
        public void GetAllCheckRevisionsByIdValidArgument()
        {
            var c = this.dashboardService.GetAllCheckRevisionsById("2");

            Assert.AreEqual(2, c.Length, "Should have 2");
        }

        [Test]
        public void GetAllCheckRevisionsByIdInvalidArgument()
        {
            var c = this.dashboardService.GetAllCheckRevisionsById("a");

            Assert.AreEqual(0, c.Length, "Should have 0");
        }

        [Test]
        public void AddCheckValidArgument()
        {
            var newCheck = new CheckDTO()
                               {
                                   CheckID = 4,
                                   Name = "Check 4",
                                   Code = "Check Code 4",
                                   Description = "Check 4 Description",
                                   FromDate = new DateTime(2017, 3, 24),
                                   IsActive = true,
                                   RevisionNR = 1
                               };
            this.dashboardService.AddCheck(newCheck);

            Assert.AreEqual(5, this.context.Checks.Count(), "Should have 5");
        }

        [Test]
        public void UpdateCheckInvalidArgument()
        {
            var newCheck = new CheckDTO()
            {
                CheckID = 1,
                Name = "Check 1 Updated",
                Code = "Check Code 1 Updated",
                Description = "Check 1 Description Updated",
                FromDate = new DateTime(2017, 3, 24),
                IsActive = true
            };
            var updatedCheck = this.dashboardService.UpdateCheck("a", newCheck);

            Assert.AreEqual(4, this.context.Checks.Count(), "Should have 4");
        }

        [Test]
        public void DeleteCheckInvalidArgument()
        {
            this.dashboardService.DeleteCheck("a");

            Assert.AreEqual(4, this.context.Checks.Count(), "Should have 4");
        }
    }
}
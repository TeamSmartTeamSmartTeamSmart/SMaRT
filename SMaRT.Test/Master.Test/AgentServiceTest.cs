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
    using SMaRT.Shared.Util;

    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class AgentServiceTest
    {
        //private DbSet<Check> fakeDbSet;

        //private SMaRTEntities entities;

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            //this.fakeDbSet =
            //    Aef.FakeDbSet(
            //        new List<Check>
            //            {
            //                new Check()
            //                    {
            //                        CheckID = 1,
            //                        Name = "Check 1",
            //                        Code = "Check Code 1",
            //                        Description = "Check 1 Description",
            //                        FromDate = new DateTime(2017, 3, 24),
            //                        IsActive = true,
            //                        IsNewest = true,
            //                        RevisionNR = 1
            //                    }
            //            });

            ////this.fakeDbSet = Aef.FakeDbSet<Check>(10);

            //entities  = A.Fake<SMaRTEntities>();
            //A.CallTo(() => entities.Checks).Returns(this.fakeDbSet);
        }

        [Test]
        public void TestMethod()
        {
            // Create test data
            var testData = new List<Check>
            {
                new Check()
                                {
                                    CheckID = 1,
                                    Name = "Check 1",
                                    Code = "Check Code 1",
                                    Description = "Check 1 Description",
                                    FromDate = new DateTime(2017, 3, 24),
                                    IsActive = true,
                                    IsNewest = true,
                                    RevisionNR = 1
                                }
                };

            var set =
                Aef.FakeDbSet(testData);

            var context = A.Fake<SMaRTEntities>();
            A.CallTo(() => context.Checks).Returns(set);

            var dashboardService = new DashboardService(context);

            // Act
            var products = dashboardService.GetCheckRevisionList().ToList();

            // Assert
            Assert.AreEqual(1, products.Count, "Should have 1");
            Assert.AreEqual(1, products.First().CheckID, "Should be 1");
        }
    }
}
namespace SMaRT.Test.Master.Test
{
    using System;
    using System.Collections.Generic;

    using EntityFramework.FakeItEasy;

    using FakeItEasy;

    using NUnit.Framework;

    using SMaRT.Master;
    using SMaRT.Master.ServiceDashboard;

    [TestFixture]
    public abstract class AgentServiceGeneralTest
    {
        protected DashboardService dashboardService;

        protected SMaRTModel context;

        [SetUp]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            // Add checks
            var testDataCheck = new List<Check>
                                    {
                                        new Check
                                            {
                                                CheckID = 1,
                                                Name = "Check 1",
                                                Code = "Check Code 1",
                                                Description = "Check 1 Description",
                                                FromDate = new DateTime(2017, 3, 24),
                                                IsActive = true,
                                                IsNewest = true,
                                                RevisionNR = 1
                                            },
                                        new Check
                                            {
                                                CheckID = 2,
                                                Name = "Check 2",
                                                Code = "Check Code 2",
                                                Description = "Check 2 Description",
                                                FromDate = new DateTime(2017, 3, 24),
                                                IsActive = false,
                                                IsNewest = false,
                                                RevisionNR = 1
                                            },
                                        new Check
                                            {
                                                CheckID = 2,
                                                Name = "Check 2 Update",
                                                Code = "Check Code 2 Update",
                                                Description = "Check 2 Description Update",
                                                FromDate = new DateTime(2017, 3, 25),
                                                IsActive = true,
                                                IsNewest = true,
                                                RevisionNR = 2
                                            },
                                        new Check
                                            {
                                                CheckID = 3,
                                                Name = "Check 3",
                                                Code = "Check Code 3",
                                                Description = "Check 3 Description",
                                                FromDate = new DateTime(2017, 3, 24),
                                                IsActive = false,
                                                IsNewest = true,
                                                RevisionNR = 1
                                            }
                                    };

            // Add CheckableEntities
            var testDataCheckableEntity = new List<CheckableEntity> { new CheckableEntity
                                                                          {
                                                                              EntityID = 1,
                                                                              Name = "Client 1",
                                                                              Description = "Client 1 Description",
                                                                              FromDate = new DateTime(2017, 3, 25),
                                                                              IsNewest = true,
                                                                              IsActive = true,
                                                                              RevisionNR = 1
                                                                          }, new CheckableEntity
                                                                          {
                                                                              EntityID = 2,
                                                                              Name = "Client 2",
                                                                              Description = "Client 2 Description",
                                                                              FromDate = new DateTime(2017, 3, 25),
                                                                              IsNewest = false,
                                                                              IsActive = false,
                                                                              RevisionNR = 1
                                                                          }, new CheckableEntity
                                                                          {
                                                                              EntityID = 2,
                                                                              Name = "Client 2 Updated",
                                                                              Description = "Client 2 Description Updated",
                                                                              FromDate = new DateTime(2017, 3, 25),
                                                                              IsNewest = true,
                                                                              IsActive = true,
                                                                              RevisionNR = 2
                                                                          }, new CheckableEntity
                                                                          {
                                                                              EntityID = 3,
                                                                              Name = "Client 3",
                                                                              Description = "Client 3 Description",
                                                                              FromDate = new DateTime(2017, 3, 25),
                                                                              IsNewest = true,
                                                                              IsActive = false,
                                                                              RevisionNR = 1
                                                                          }
            };

            var setCheck = Aef.FakeDbSet(testDataCheck);
            var setCheckableEntity = Aef.FakeDbSet(testDataCheckableEntity);

            context = A.Fake<SMaRTModel>();
            A.CallTo(() => context.Checks).Returns(setCheck);
            A.CallTo(() => context.CheckableEntities).Returns(setCheckableEntity);

            this.dashboardService = new DashboardService(context);
        }
    }
}
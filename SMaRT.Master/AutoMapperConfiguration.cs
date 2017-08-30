namespace SMaRT.Master
{
    using System.Xml.Linq;
    using AutoMapper;
    using SMaRT.Shared.ConnectionObjects;

    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                // CheckService
                cfg.CreateMap<CheckReturn, CheckExecution>();
                cfg.CreateMap<CheckAssignment, CheckInstruction>()
                    .ForMember(
                        instruction => instruction.Parameters,
                        assignment => assignment.MapFrom(a => XDocument.Parse(a.Parameters).Root));
                cfg.CreateMap<Check, CheckCode>();

                // DashboardService
                cfg.CreateMap<Check, CheckDTO>();
                cfg.CreateMap<CheckDTO, Check>();

                cfg.CreateMap<CheckAssignment, CheckAssignmentDTO>();
                cfg.CreateMap<CheckAssignmentDTO, CheckAssignment>();

                cfg.CreateMap<CheckableEntity, CheckableEntityDTO>();
                cfg.CreateMap<CheckableEntityDTO, CheckableEntity>();

                cfg.CreateMap<GroupMembership, GroupMembershipDTO>();
                cfg.CreateMap<GroupMembershipDTO, GroupMembership>();

                cfg.CreateMap<CheckExecution, CheckExecutionDTO>();
                cfg.CreateMap<CheckExecutionDTO, CheckExecution>();
            });
        }
    }
}

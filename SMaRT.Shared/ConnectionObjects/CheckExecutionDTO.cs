namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract, Serializable]
    public class CheckExecutionDTO
    {
        [DataMember]
        public int CheckID { get; set; }

        [DataMember]
        public int CheckRevisionNR { get; set; }

        [DataMember]
        public int AssignedEntityID { get; set; }

        [DataMember]
        public int AssignedEntityRevisionNR { get; set; }

        [DataMember]
        public int AssignmentRevisionNR { get; set; }

        [DataMember]
        public int ExecutedEntityID { get; set; }

        [DataMember]
        public int ExecutedEntityRevisionNR { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime? EndTime { get; set; }

        [DataMember]
        public int ReturnCode { get; set; }

        [DataMember]
        public string Output { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public virtual CheckableEntityDTO CheckableEntity { get; set; }

        [DataMember]
        public virtual CheckAssignmentDTO CheckAssignment { get; set; }
    }
}

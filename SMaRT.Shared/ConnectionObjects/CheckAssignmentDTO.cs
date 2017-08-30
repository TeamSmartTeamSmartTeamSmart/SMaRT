namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CheckAssignmentDTO
    {
        [DataMember]
        public int CheckID { get; set; }

        [DataMember]
        public int CheckRevisionNR { get; set; }

        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int EntityRevisionNR { get; set; }

        [DataMember]
        public int RevisionNR { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public System.DateTime FromDate { get; set; }

        [DataMember]
        public string Parameters { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual CheckDTO Check { get; set; }

        [DataMember]
        public virtual CheckableEntityDTO CheckableEntity { get; set; }
    }
}

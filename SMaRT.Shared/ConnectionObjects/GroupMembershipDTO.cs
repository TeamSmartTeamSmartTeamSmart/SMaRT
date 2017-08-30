namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract, Serializable]
    public class GroupMembershipDTO
    {
        [DataMember]
        public int ParentID { get; set; }

        [DataMember]
        public int ParentRevisionNR { get; set; }

        [DataMember]
        public int ChildID { get; set; }

        [DataMember]
        public int ChildRevisionNR { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual CheckableEntityDTO Child { get; set; }

        [DataMember]
        public virtual CheckableEntityDTO Parent { get; set; }
    }
}

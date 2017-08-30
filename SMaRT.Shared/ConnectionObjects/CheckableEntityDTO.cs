namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract, Serializable]
    public class CheckableEntityDTO
    {
        [DataMember]
        public int EntityID { get; set; }

        [DataMember]
        public int RevisionNR { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public System.DateTime FromDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}

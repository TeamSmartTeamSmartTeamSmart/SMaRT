namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CheckReturn
    {
        [DataMember]
        public int CheckID { get; set; }

        [DataMember]
        public int CheckRevisionNR { get; set; }

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
        public DateTime InstructionDate { get; set; }
    }
}

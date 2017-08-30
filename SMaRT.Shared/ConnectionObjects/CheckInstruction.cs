namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Linq;

    [DataContract]
    public class CheckInstruction
    {
        [DataMember]
        public int CheckID { get; set; }

        [DataMember]
        public int CheckRevisionNR { get; set; }

        [DataMember]
        public XElement Parameters { get; set; }

        [DataMember]
        public DateTime InstructionDate { get; set; } 
    }
}

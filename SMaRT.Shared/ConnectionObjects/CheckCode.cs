namespace SMaRT.Shared.ConnectionObjects
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CheckCode
    {
        [DataMember]
        public int CheckID { get; set; }

        [DataMember]
        public int CheckRevisionID { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}

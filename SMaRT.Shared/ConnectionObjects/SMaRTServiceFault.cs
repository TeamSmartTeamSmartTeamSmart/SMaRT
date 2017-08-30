namespace SMaRT.Shared.ConnectionObjects
{
    using System.Runtime.Serialization;

    public enum FaultCodes
    {
        NotFound = 0,
        NotActive = 1,
        Exception = 2
    }

    [DataContract]
    public class SMaRTServiceFault
    {
        public SMaRTServiceFault(FaultCodes faultCode, string message)
        {
            this.FaultCode = faultCode;
            this.Message = message;
        }

        [DataMember]
        public FaultCodes FaultCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

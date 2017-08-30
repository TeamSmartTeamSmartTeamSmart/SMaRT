namespace SMaRT.Master.ServiceAgent
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using SMaRT.Shared.ConnectionObjects;

    [ServiceContract]
    public interface ICheckService
    {
        [OperationContract]
        [FaultContract(typeof(SMaRTServiceFault))]
        IEnumerable<CheckInstruction> PullInstructions(int agentId);

        [OperationContract]
        [FaultContract(typeof(SMaRTServiceFault))]
        CheckCode PullInstructionCode(int checkId);

        [OperationContract]
        [FaultContract(typeof(SMaRTServiceFault))]
        void PushReturn(int agentId, CheckReturn returnValues);
    }
}

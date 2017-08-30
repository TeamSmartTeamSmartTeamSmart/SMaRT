namespace SMaRT.Master.ServiceAgent
{
    using System;
    using System.ServiceModel.Configuration;

    public class CheckServiceBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(CheckServiceBehavior);

        protected override object CreateBehavior()
        {
            return new CheckServiceBehavior();
        }
    }
}

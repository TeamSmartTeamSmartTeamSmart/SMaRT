namespace SMaRT.Master.ServiceAgent
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using SMaRT.Shared.ConnectionObjects;

    public class CheckServiceBehavior : Attribute, IErrorHandler, IServiceBehavior
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion ver, ref Message msg)
        {
            if (error is FaultException)
            {
                return;
            }

            var fe  = new FaultException<SMaRTServiceFault>(new SMaRTServiceFault(FaultCodes.Exception, error.Message));
            var fault = fe.CreateMessageFault();
            msg = Message.CreateMessage(ver, fault, string.Empty);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Adds a CheckServiceErrorHandler to each ChannelDispatcher
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher?.ErrorHandlers.Add(new CheckServiceBehavior());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace Client
{
    public class ServiceInvoker : IServiceInvoker
    {
        private static readonly ChannelFactoryManager _factoryManager = new ChannelFactoryManager();

        private static readonly ClientSection _clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

        public void InvokeService<TService>(Action<TService> serviceMethod) where TService : class
        {
            var channel = CreateChannel<TService>();
            try
            {
                serviceMethod(channel);
            }
            finally
            {
                DisposeChannel(channel);
            }
        }

        public TResult InvokeService<TService, TResult>(Func<TService, TResult> serviceMethod) where TService : class
        {
            var channel = CreateChannel<TService>();
            try
            {
                return serviceMethod(channel);
            }
            finally
            {
                DisposeChannel(channel);
            }
        }

        private static TService CreateChannel<TService>() where TService : class
        {
            var endpointNameAddressPair = GetEndpointNameAddressPair(typeof(TService));
            return _factoryManager.CreateChannel<TService>(endpointNameAddressPair.Key, endpointNameAddressPair.Value);
        }

        private static KeyValuePair<string, string> GetEndpointNameAddressPair(Type serviceContractType)
        {
            if (_clientSection?.Endpoints != null && _clientSection.Endpoints.Count > 0)
            {
                foreach (var endpoint in _clientSection.Endpoints.Cast<ChannelEndpointElement>().Where(x => x.Contract == serviceContractType.ToString()))
                {
                    return new KeyValuePair<string, string>(endpoint.Name, endpoint.Address.AbsoluteUri);
                }
            }

            throw new ConfigurationErrorsException($"No client endpoint found for type {serviceContractType}.");
        }

        private static void DisposeChannel<TService>(TService channel) where TService : class
        {
            var commObj = (ICommunicationObject)channel;
            try
            {
                if (commObj.State != CommunicationState.Faulted)
                {
                    commObj.Close();
                }
            }
            catch
            {
                commObj.Abort();
            }
        }
    }
}

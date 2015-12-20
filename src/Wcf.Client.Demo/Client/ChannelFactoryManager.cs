using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Client
{
    public class ChannelFactoryManager : IDisposable
    {
        private static readonly Dictionary<Type, ChannelFactory> _channelFactoryCache = new Dictionary<Type, ChannelFactory>();

        private static readonly object _syncRoot = new object();

        public virtual T CreateChannel<T>(string endpointConfigurationName, string endpointAddress) where T : class
        {
            T local = GetFactory<T>(endpointConfigurationName, endpointAddress).CreateChannel();
            ((IClientChannel)local).Faulted += ChannelFaulted;
            return local;
        }

        protected virtual ChannelFactory<T> GetFactory<T>(string endpointConfigurationName, string endpointAddress) where T : class
        {
            lock (_syncRoot)
            {
                ChannelFactory factory;
                if (!_channelFactoryCache.TryGetValue(typeof(T), out factory))
                {
                    factory = CreateFactoryInstance<T>(endpointConfigurationName, endpointAddress);
                    _channelFactoryCache.Add(typeof(T), factory);
                }
                return factory as ChannelFactory<T>;
            }
        }

        private ChannelFactory CreateFactoryInstance<T>(string endpointConfigurationName, string endpointAddress)
        {
            ChannelFactory factory;
            if (!string.IsNullOrEmpty(endpointAddress))
            {
                factory = new ChannelFactory<T>(endpointConfigurationName, new EndpointAddress(endpointAddress));
            }
            else
            {
                factory = new ChannelFactory<T>(endpointConfigurationName);
            }
            factory.Faulted += FactoryFaulted;
            factory.Open();
            return factory;
        }

        private static void ChannelFaulted(object sender, EventArgs e)
        {
            var channel = (IClientChannel)sender;
            try
            {
                channel.Close();
            }
            catch
            {
                channel.Abort();
            }
            throw new CommunicationObjectFaultedException($"A channel {channel.GetType().FullName} is faulted");
        }

        private static void FactoryFaulted(object sender, EventArgs args)
        {
            var factory = (ChannelFactory)sender;
            try
            {
                factory.Close();
            }
            catch
            {
                factory.Abort();
            }
            var genericArguments = factory.GetType().GetGenericArguments();
            if (genericArguments.Length == 1)
            {
                Type key = genericArguments[0];
                if (_channelFactoryCache.ContainsKey(key))
                {
                    _channelFactoryCache.Remove(key);
                }
            }
            throw new CommunicationObjectFaultedException($"A factory {factory.GetType().FullName} is faulted");
        }

        public void Dispose()
        {
            DisposeManagedResources();
        }

        protected virtual void DisposeManagedResources()
        {
            lock (_syncRoot)
            {
                foreach (var factory in _channelFactoryCache.Values)
                {
                    try
                    {
                        factory.Close();
                    }
                    catch
                    {
                        factory.Abort();
                    }
                }
                _channelFactoryCache.Clear();
            }
        }
    }
}

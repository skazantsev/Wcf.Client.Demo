using System;

namespace Client
{
    public interface IServiceInvoker
    {
        void InvokeService<TService>(Action<TService> serviceMethod) where TService : class;

        TResult InvokeService<TService, TResult>(Func<TService, TResult> serviceMethod) where TService : class;
    }
}

using System;

namespace Client
{
    public interface IServiceInvoker
    {
        TResult InvokeService<TService, TResult>(Func<TService, TResult> serviceMethod)
            where TService : class;
    }
}

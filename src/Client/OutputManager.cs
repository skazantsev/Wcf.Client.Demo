using System.Threading.Tasks;
using Contracts;

namespace Client
{
    public class OutputManager
    {
        private readonly IServiceInvoker _serviceInvoker;

        public OutputManager(IServiceInvoker serviceInvoker)
        {
            _serviceInvoker = serviceInvoker;
        }

        public async Task<string> GetOutput()
        {
            var result = await _serviceInvoker.InvokeService((ICalculatorService x) => x.Multiply(5, 6));
            return string.Format($"Result is: {result}");
        }
    }
}

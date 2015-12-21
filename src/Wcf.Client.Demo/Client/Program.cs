using System;
using System.Threading.Tasks;
using Contracts;

namespace Client
{
    public class Program
    {
        private static IServiceInvoker _invoker;
        public static void Main()
        {
            _invoker = new ServiceInvoker();
            var task = MakeCall();
            Console.WriteLine($"Inocation of {nameof(ICalculatorService)}. Result: {task.Result}");
        }

        private static async Task<double> MakeCall()
        {
            var result = await _invoker.InvokeService<ICalculatorService, Task<double>>(async x => await x.Multiply(5, 6));
            return result;
        }
    }
}

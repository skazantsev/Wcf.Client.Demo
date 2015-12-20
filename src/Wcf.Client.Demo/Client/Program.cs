using System;
using System.Threading.Tasks;
using Contracts;

namespace Client
{
    public class Program
    {
        static void Main()
        {
            var invoker = new ServiceInvoker();
            var task = invoker.InvokeService<ICalculatorService, Task<double>>(async x => await x.Multiply(5, 6));
            Console.WriteLine($"Inocation of {nameof(ICalculatorService)}. Result: {task.Result}");
        }
    }
}

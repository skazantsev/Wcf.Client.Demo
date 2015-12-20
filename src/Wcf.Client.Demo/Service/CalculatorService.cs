using System.Threading.Tasks;
using Contracts;

namespace Service
{
    internal class CalculatorService : ICalculatorService
    {
        public Task<double> Multiply(double num1, double num2)
        {
            return Task.FromResult(num1 * num2);
        }
    }
}

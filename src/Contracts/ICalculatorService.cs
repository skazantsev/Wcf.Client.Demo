using System.ServiceModel;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        Task<double> Multiply(double num1, double num2);
    }
}

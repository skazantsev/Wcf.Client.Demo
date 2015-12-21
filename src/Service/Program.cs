using System;
using System.ServiceModel;

namespace Service
{
    public class Program
    {
        static void Main()
        {
            using (var host = new ServiceHost(typeof(CalculatorService)))
            {
                host.Open();
                Console.WriteLine("Service is started!");
                Console.ReadLine();
            }
        }
    }
}

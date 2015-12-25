using System;

namespace Client
{
    public class Program
    {
        public static void Main()
        {
            var output = new OutputManager(new ServiceInvoker()).GetOutput().Result;
            Console.WriteLine(output);
        }
    }
}

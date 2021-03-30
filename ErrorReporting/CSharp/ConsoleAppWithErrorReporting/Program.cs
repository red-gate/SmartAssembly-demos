using System;

namespace ConsoleAppWithErrorReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void DoSomething()
        {
            // The following will throw an exception.
            // It will be handled by SmartAssembly and
            // error report will be automatically sent.
            Console.WriteLine($"5 divided by 0 is: {Divide(5, 0)}");
        }

        private static int Divide(int a, int b)
        {
            return a / b;
        }
    }
}

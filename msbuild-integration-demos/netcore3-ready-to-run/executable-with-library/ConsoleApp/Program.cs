using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new ThisClassWillBeObfuscated();
            c.PrintClassName();

            Console.WriteLine("Calling external library...");
            var c2 = new ExternalLibrary.PublicClass();
            c2.PrintClassName();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

using System;

namespace ConsoleApp
{
    class ThisIsNotObfuscated { }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is single-file executable!");
            Console.WriteLine("This name should be obfuscated:");
            Console.WriteLine(typeof(ThisIsNotObfuscated).FullName);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new ThisClassWillBeObfuscated();
            c.PrintClassName();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    class ThisClassWillBeObfuscated
    {
        internal void PrintClassName()
        {
            Console.WriteLine("After obfuscation, the name below will be obfuscated:");
            Console.WriteLine(this.GetType().FullName);
            Console.WriteLine();
        }
    }
}

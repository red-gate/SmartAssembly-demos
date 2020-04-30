using System;

namespace ExternalLibrary
{
    public class PublicClass
    {
        public void PrintClassName()
        {
            Console.WriteLine("After obfuscation, the name below will NOT be obfuscated (it's public):");
            Console.WriteLine(this.GetType().FullName);
            Console.WriteLine();

            Console.WriteLine("Calling internal class from external library...");
            var c = new ShouldBeObfuscated();
            c.PrintClassName();
        }
    }
}
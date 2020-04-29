using System;

namespace ExternalLibrary
{
    internal class ShouldBeObfuscated
    {
        public void PrintClassName()
        {
            Console.WriteLine("After obfuscation, the name below will be obfuscated:");
            Console.WriteLine(this.GetType().FullName);
            Console.WriteLine();
        }
    }
}

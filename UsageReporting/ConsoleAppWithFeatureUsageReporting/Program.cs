using SmartAssembly.ReportUsage;
using System;

namespace ConsoleAppWithFeatureUsageReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething();
            DoSomethingElse();
            ReportManually();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        [ReportUsage]
        private static void DoSomething()
        {
            Console.WriteLine($"Usage of `{nameof(DoSomething)}` method will be automatically reported when processed by SmartAssembly!");
        }

        [ReportUsage("custom feature name")]
        private static void DoSomethingElse()
        {
            Console.WriteLine($"Usage of `{nameof(DoSomethingElse)}` method will be automatically reported as `custom feature name` when processed by SmartAssembly!");
        }

        private static void ReportManually()
        {
            Console.WriteLine("You can also call `SmartAssembly.ReportUsage.UsageCounter.ReportUsage` method to manually report usage of a feature.");
            UsageCounter.ReportUsage("manually reported feature");

            Console.WriteLine("You can also call `SmartAssembly.ReportUsage.PlatformData.*` methods to report additional information about the user's machine.");
            PlatformData.ReportNumberOfMonitors();
        }
    }
}

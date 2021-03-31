This directory contains:
- a sample console application with SmartAssembly's feature usage reporting applied,
- examples of custom feature usage reporting templates.

Inspect the provided UsageReporting.sln solution and the .saproj file contained inside [`ConsoleAppWithFeatureUsageReporting`](./ConsoleAppWithFeatureUsageReporting) directory.

Execute the [`build_and_run.bat`](./build_and_run.bat) file to:
- Build the solution.
- Build the console application with SmartAssembly to apply feature usage reporting with one of the custom templates.
- Run the original application.
- Run the application processed by SmartAssembly.

After running the processed application, open SmartAssembly to see the feature usage reports.

See:
- https://documentation.red-gate.com/sa8/reporting-feature-usage/feature-usage-reporting-with-the-sdk
- https://www.nuget.org/packages/RedGate.SmartAssembly.SmartUsageCore/
- https://www.nuget.org/packages/RedGate.SmartAssembly.ReportUsage/

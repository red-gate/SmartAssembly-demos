This directory contains:
- a sample console application with SmartAssembly's error reporting applied,
- examples of custom error reporting templates implemented in Visual Basic .NET.

Inspect the provided ErrorReporting.sln solution and the .saproj file contained inside [`ConsoleAppWithErrorReporting`](./ConsoleAppWithErrorReporting) directory.

Execute the [`build_and_run.bat`](./build_and_run.bat) file to:
- Build the solution.
- Build the console application with SmartAssembly to apply error reporting with one of the custom templates.
- Run the original application. Application should throw an exception.
- Run the application processed by SmartAssembly. The exception thrown by the application
  will be handled by SmartAssembly and an error dialog based on the custom template will be shown,
  allowing you to send the error report.

After running the processed application, open SmartAssembly to see the error reports.

See:
- https://documentation.red-gate.com/sa8/setting-up-error-reporting/error-reporting-with-the-sdk
- https://www.nuget.org/packages/RedGate.SmartAssembly.SmartExceptionsCore/
- https://www.nuget.org/packages/RedGate.SmartAssembly.ReportException/

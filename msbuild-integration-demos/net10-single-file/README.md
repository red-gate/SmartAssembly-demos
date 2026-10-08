# Introduction

.NET latest versions supports single-file deployment, allowing applications to be published as a single executable that contains application binaries, configuration files, and dependencies.

This deployment model simplifies distribution by reducing the number of files that need to be deployed. It is especially beneficial for self-contained applications that would otherwise produce a large number of assemblies and runtime files.

Single-file publishing can be used with both framework-dependent and self-contained deployments. A Runtime Identifier (RID) must be specified to target a particular operating system and architecture. Additional deployment optimizations such as trimming and ReadyToRun compilation can also be combined with single-file publishing.

# Running the example

This directory contains an example of integrating SmartAssembly into a .NET 10 single-file executable build:

- simplified-example - .NET 10 console application protected by SmartAssembly and published as a single-file executable.

Execute the `publish-and-run.bat` file from the example above to build, protect, publish, and run the application protected by SmartAssembly.

# More information

Follow our documentation to see how to protect your single-file application by integrating SmartAssembly into your build process.

https://documentation.red-gate.com/sa/building-your-assembly/using-smartassembly-with-single-file-applications-net

For more information about single-file deployment in .NET, see:

https://learn.microsoft.com/dotnet/core/deploying/single-file/overview
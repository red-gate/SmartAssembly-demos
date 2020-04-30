# Introduction
[.NET Core 3.0 introduced the ability to create ReadyToRun (R2R) binaries](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#readytorun-images), aimed at improving the startup performance of applications.

Usually, a regular .NET assembly only contains managed code (Intermediate Language Code, or IL) which is compiled to native code by the just-in-time compiler (JIT) after the application starts. Assembly built as ReadyToRun not only contains managed code, but also parts of it already precompiled to its native form – this greatly reduces the work required by the JIT to load your application, because it was done ahead-of-time.

ReadyToRun is only available when publishing a self-contained .NET Core application, targeting a specific environment and bitness.

# Running the example
This directory contains two examples of integrating SmartAssembly into .NET Core 3 _ReadyToRun_ build:
- [simplified-example](simplified-example) - .NET Core 3.1 console application protected by SmartAssembly, published as ReadyToRun.
- [executable-with-library](executable-with-library) - .NET Core 3.1 console application with reference to .NET Standard 2.0 library, both protected by SmartAssembly, published as ReadyToRun.

Execute the `publish-and-run.bat` file from either example, to build, publish, and run the application protected by SmartAssembly.

# More information
Follow our documentation to see how to protect your ReadyToRun assembly by integrating SmartAssembly into your build process.

https://documentation.red-gate.com/sa7/building-your-assembly/using-smartassembly-with-readytorun-images-net-core-3

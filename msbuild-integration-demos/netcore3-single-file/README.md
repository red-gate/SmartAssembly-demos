# Introduction
[.NET Core 3.0 introduced the ability to create single-file executables](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#single-file-executables). This allows for distribution of only one application file, as all configs and dependencies are included within the binary itself.

The feature provides a native way for dependencies embedding which is most beneficial when publishing self-contained applications generating hundreds of assemblies. It can be used for framework-dependent or self-contained applications, but requires setting a runtime identifier in both cases to target a specific environment and bitness.

# Running the example
This directory contains an example of integrating SmartAssembly into .NET Core 3 single-file executable build:
- [simplified-example](simplified-example) - .NET Core 3.1 console application protected by SmartAssembly, published as single-file.
- [multiple-runtimes](multiple-runtimes) - .NET Core 3.1 WPF application protected by SmartAssembly, published as single-file for **win-x64** and **win-x86** runtimes separately.

Execute the `publish-and-run.bat` file from example above, to build, publish, and run the application protected by SmartAssembly.

# More information
Follow our documentation to see how to protect your single-file application by integrating SmartAssembly into your build process.

- https://documentation.red-gate.com/sa8/building-your-assembly/using-smartassembly-with-single-file-executables-net-core-3
- https://documentation.red-gate.com/sa8/building-your-assembly/using-smartassembly-with-single-file-executables-net-core-3/obfuscating-multiple-runtimes

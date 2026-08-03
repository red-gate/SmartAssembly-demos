# Introduction

ReadyToRun (R2R) compilation enables .NET applications to improve startup performance by precompiling portions of managed assemblies into native code at publish time.

A standard .NET assembly contains Intermediate Language (IL) code, which is compiled to native machine code by the just-in-time (JIT) compiler when the application runs. Assemblies published with ReadyToRun contain both IL and precompiled native code, significantly reducing the amount of JIT compilation required during startup and helping applications start faster.

ReadyToRun is available when publishing a self-contained application for a specific target runtime identifier (RID) and architecture.

# Running the example

This directory contains examples of integrating SmartAssembly into a .NET 10.0 ReadyToRun build:

- [simplified-example](simplified-example) - .NET 10.0 console application protected by SmartAssembly and n.
- executable-with-library - .NET 10.0 console application with a reference to a .NET library, both protected by SmartAssembly and published as ReadyToRun.

Execute the `publish-and-run.bat` file from either example to build, protect, publish, and run the application using SmartAssembly and ReadyToRun compilation.

# More information

For guidance on protecting ReadyToRun assemblies with SmartAssembly as part of your build pipeline, see the SmartAssembly documentation:

https://documentation.red-gate.com/sa7/building-your-assembly/using-smartassembly-with-readytorun-images-net-core-3
REM Building test application and sample error reporting templates...
dotnet build .\ErrorReporting.sln -c Release

REM Running SmartAssembly to add error reporting to test application...
"%PROGRAMFILES%\Red Gate\SmartAssembly 8\SmartAssembly.com" /build .\ConsoleAppWithErrorReporting\ConsoleAppWithErrorReporting_net472_sample02.saproj

REM Running original assembly...
.\ConsoleAppWithErrorReporting\bin\Release\net472\ConsoleAppWithErrorReporting.exe

REM Running assembly built with SmartAssembly error reporting...
.\ConsoleAppWithErrorReporting\bin\protected\ConsoleAppWithErrorReporting.exe

pause
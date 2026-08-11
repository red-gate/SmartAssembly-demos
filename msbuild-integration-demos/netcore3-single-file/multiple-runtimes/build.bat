dotnet publish .\SampleApp\SampleApp.csproj -r win-x64 -p:PublishSingleFile=true -c Release
dotnet publish .\SampleApp\SampleApp.csproj -r win-x86 -p:PublishSingleFile=true -c Release

REM Execute 64-bit assembly
.\SampleApp\bin\Release\netcoreapp3.1\win-x64\publish\SampleApp.exe

REM Execute 32-bit assembly
.\SampleApp\bin\Release\netcoreapp3.1\win-x86\publish\SampleApp.exe

dotnet clean
dotnet restore
REM dotnet test
dotnet build -c Release
dotnet pack -c Release

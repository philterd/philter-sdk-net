#!/bin/bash
dotnet restore
dotnet build -c Release
nuget pack philter-sdk-net.csproj -Prop Configuration=Release

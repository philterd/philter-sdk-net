#!/bin/bash
dotnet restore
dotnet build -c Release
dotnet pack -c Release philter-sdk-net.csproj

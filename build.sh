#!/bin/bash -e

dotnet restore
dotnet test
dotnet build -c Release
dotnet pack -c Release

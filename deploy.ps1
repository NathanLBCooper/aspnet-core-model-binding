$OUTPUT = Join-Path $PSScriptRoot "build"

dotnet nuget push $OUTPUT\nupkgs\*.nupkg -ApiKey $NUGET_API_KEY -Verbosity detailed -Source $NUGET_SOURCE
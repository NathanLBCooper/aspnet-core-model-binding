param (
    [string]$nuget_source,
    [string]$nuget_api_key
)

$OUTPUT = Join-Path $PSScriptRoot "build"

dotnet nuget push $OUTPUT\nupkgs\*.nupkg --source $nuget_source --api-key $nuget_api_key

exit $LASTEXITCODE
param (
    [string]$build_number = "0",
    [string]$branch = "master"
)

$OUTPUT = Join-Path $PSScriptRoot "build"
$VERSION = "0.1." + $build_number
$PACKAGEVERSION = $VERSION
$branch = $branch.Replace("/merge","pr")

if(!$branch.EndsWith("master")) {
    $PACKAGEVERSION += "-" + $branch
}

Write-Host "Building version " $VERSION "for branch" $branch

Get-ChildItem -Path $OUTPUT -Include * | remove-Item -recurse
dotnet clean AliasModelBinder.sln

if(!$LASTEXITCODE) { dotnet restore AliasModelBinder.sln }
if(!$LASTEXITCODE) { dotnet build AliasModelBinder.sln -c Release }

if(!$LASTEXITCODE) { dotnet test --no-build --verbosity normal test\AliasModelBinder.IntegrationTests\AliasModelBinder.IntegrationTests.csproj -c Release }

if(!$LASTEXITCODE) { dotnet pack src\AliasModelBinder.Client\AliasModelBinder.Client.csproj --configuration RELEASE --output $OUTPUT\nupkgs /p:Version=$PACKAGEVERSION  /p:FileVersion=$VERSION /p:AssemblyVersion=$VERSION }
if(!$LASTEXITCODE) { dotnet pack src\AliasModelBinder.Web\AliasModelBinder.Web.csproj --configuration RELEASE --output $OUTPUT\nupkgs /p:Version=$PACKAGEVERSION  /p:FileVersion=$VERSION /p:AssemblyVersion=$VERSION }

exit $LASTEXITCODE
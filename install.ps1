param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
    ,
    [switch] $Rebuild
)
Set-StrictMode -Version Latest
$man = Test-ModuleManifest $PSScriptRoot/PSEverything/PSEverything.psd1

$msbuild = Resolve-Path ${env:ProgramFiles(x86)}\msbuild\*\bin\msbuild.exe | Select-Object -last 1
[string] $sln = Resolve-Path "$PSScriptRoot\$($man.Name).sln"

if($msbuild -eq $Null){
    throw "Cannot find msbuild.exe"
}

$msbuildArgs = @("/p:Configuration=$Configuration")
if ($Rebuild){
    $msbuildArgs += '/target:rebuild'
}
else{
    $msbuildArgs += '/target:build'
}
$msbuildArgs += $sln
"$msbuildArgs" 
& $msbuild.ProviderPath $msbuildArgs

$man = Test-ModuleManifest $PSScriptRoot/PSEverything/PSEverything.psd1

$name = $man.Name
[string]$version = $man.Version
$moduleSourceDir = "$PSScriptRoot/PSEverything/bin/$Configuration/PSEverything"
$moduleDir = "~/documents/WindowsPowerShell/Modules/$name/$version/"

$newLine = [Environment]::NewLine

$ofs = $newLine
[string]$about_content = Get-Content $PSScriptRoot/README.md | ForEach-Object {
    $_ -replace '```.*', ''    
} 

if (-not (Test-Path $moduleDir))
{
    $null = mkdir $moduleDir
}

Get-ChildItem $moduleSourceDir | Copy-Item -Destination $moduleDir -ErrorAction:Continue
Set-Content -Path $moduleDir/about_${name}.help.txt -value $about_content

$cert =Get-ChildItem cert:\CurrentUser\My -CodeSigningCert
if($cert)
{
    Get-ChildItem $moduleDir/*.ps?1,$moduleDir/*.dll | Set-AuthenticodeSignature -Certificate $cert -TimestampServer http://timestamp.verisign.com/scripts/timstamp.dll
} 

param([Switch] $DebugBinaries)

$psmoduleInfo = Test-ModuleManifest "$PSScriptRoot\PSEverything\PSEverything.psd1"
[string]$version = $psmoduleInfo.Version
$name = $psmoduleInfo.Name

$modulePath = "~/documents/WindowsPowerShell/Modules/$name/$version"
if (-not (Test-Path $modulePath))
{
    $null = mkdir $modulePath
}

$config = if($DebugBinaries){'Debug'} else {'Release'}

Copy-Item $PSScriptRoot\PSEverything\bin\$config\PSEverything\* -Destination $modulePath -Verbose

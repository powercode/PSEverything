$man = Import-PowerShellDataFile $PSScriptRoot/PSEverything/PSEverything.psd1

$name = 'PSEverything'
[string]$version = $man.ModuleVersion

Publish-Module -Name $name -RequiredVersion $version -NuGetApiKey $NuGetApiKey -Repository PSGallery

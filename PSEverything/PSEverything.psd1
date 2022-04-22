
@{
    RootModule = 'PSEverything.dll'
    ModuleVersion = '3.3.0'
    GUID = 'f262ec02-4a88-49e5-94da-e25aab9cbf7a'
    Author = 'Staffan Gustafsson'
    CompanyName = 'PowerCode Consulting AB'
    Copyright = '(c) 2016 sgustafsson. All rights reserved.'
    Description = 'Powershell access to Everything - Blazingly fast file system searches'
    PowerShellVersion = '5.1'
    CompatiblePSEditions = "Desktop", "Core"
    FunctionsToExport = ''
    CmdletsToExport = 'Search-Everything', 'Select-EverythingString'
    VariablesToExport = ''
    AliasesToExport = 'se', 'sles'
    FileList = 'Everything32.dll', 'Everything64.dll', 'LICENSE', 'PSEverything.dll', 'PSEverything.dll-Help.xml', 'PSEverything.psd1'
    PrivateData = @{
        PSData = @{
            Tags = @('Search', 'Everything', 'voidtools', 'regex', 'grep')
            LicenseUri = 'https://raw.githubusercontent.com/powercode/PSEverything/master/LICENSE'
            ProjectUri = 'https://github.com/powercode/PSEverything'
            ReleaseNotes = @'
2.3: Bug fixes. Sorted output.
2.1: Upgrading to SDK matching 1.4.1.809b  - Fixing hang when calling from Eleveated powershell
to Everything.
Now works with both eleveated and non-elevated processes as long as Everything is running as admin.
2.0:
Implements tabcompletion.
Quoting filter paths
Requires PowerShell 5.0 or greater
1.3.3:
Really fixing issue where an error was written in Select-EverythingString when Search-Everything did not return any results
1.3.2:
Fixing issue where an error was written in Select-EverythingString when Search-Everything did not return any results
1.3.1:
Bug fix for -Filter that didn't work in combination with non-global searches.
'@
        }
    }
}


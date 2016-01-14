function Select-EverythingString
{
<#
.SYNOPSIS
    Finds text in files selected by Search-Everything.
.DESCRIPTION
    This is the amalgation of Select-String and Search-Everything.
    It is basically just a Select-String -LiteralPath (Search-Everything <params>)
.EXAMPLE
    Select-EverythingString -Pattern myfile\.cs -ext csproj -Global
    
    Searches in all *.csproj files in the computer for the pattern myfile.cs 
.COMPONENT
    Uses Everything by David Carpenter from http://voidtools.com
#>
[CmdletBinding(DefaultParameterSetName='default')]
[Alias('sles')]
param(
     # Select-String Pattern
    [Parameter(Mandatory, Position=1)]
    [String[]] $Pattern,
    
    # Select-String SimpleMatch
    [Parameter()]
    [Switch] $SimpleMatch,
    
    # Select-String Quiet
    [Parameter()]
    [Switch] $Quiet,
    
    # Select-String AllMatches
    [Parameter()]
    [Switch] $AllMatches,
    
    # Select-String List
    [Parameter()]
    [Switch] $List,
    
    # Select-String NotMatch
    [Parameter()]
    [Switch] $NotMatch,
    
    # Select-String Encoding
    [ValidateSet('unicode','utf7','utf8','utf32','ascii','bigendianunicode','default','oem')]
    [ValidateNotNullOrEmpty()]
    [string]
    $Encoding,

    # Select-String Context
    [ValidateCount(1, 2)]
    [ValidateNotNullOrEmpty()]
    [ValidateRange(0, 2147483647)]
    [int[]]
    $Context,
    
    [Parameter(ParameterSetName='default')]
    [string]
    ${Filter},

    [Parameter(ParameterSetName='default', Position=2)]
    [string[]]
    ${Include},

    [Parameter(ParameterSetName='default')]
    [string[]]
    ${Exclude},

    [Parameter(ParameterSetName='default')]
    [string[]]
    ${Extension},

    [Parameter(ParameterSetName='default')]
    [Alias('pi')]
    [string[]]
    ${PathInclude},

    [Parameter(ParameterSetName='default')]
    [Alias('pe')]
    [string[]]
    ${PathExclude},

    [Parameter(ParameterSetName='default')]
    [Alias('fi')]
    [string[]]
    ${FolderInclude},

    [Parameter(ParameterSetName='default')]
    [Alias('fe')]
    [string[]]
    ${FolderExclude},

    [Parameter(ParameterSetName='default')]
    [int]
    ${ParentCount},

    [Parameter(ParameterSetName='default')]
    [string]
    ${ChildFileName},

    [Parameter(ParameterSetName='default')]
    [ValidateCount(1, 2)]
    [int[]]
    ${NameLength},

    [Parameter(ParameterSetName='default')]
    [ValidateCount(1, 2)]
    [long[]]
    ${Size},

    [Parameter(ParameterSetName='regex')]
    [string]
    ${RegularExpression},

    [switch]
    ${CaseSensitive},

    [switch]
    ${Global},

    [Parameter(ParameterSetName='default')]
    [switch]
    ${MatchWholeWord}

    )

begin
{
    try {
        $outBuffer = $null
        if ($PSBoundParameters.TryGetValue('OutBuffer', [ref]$outBuffer))
        {
            $PSBoundParameters['OutBuffer'] = 1
        }
        $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('pseverything\Search-Everything', [System.Management.Automation.CommandTypes]::Cmdlet)
        $selectStringParams =@{}
        foreach($p in 'Pattern','SimpleMatch','Quiet','AllMatches','List','NotMatch','Encoding','Context')
        {
            if($PSBoundParameters.ContainsKey($p))
            {
                $selectStringParams[$p] = $PSBoundParameters[$p]
                $null = $PSBoundParameters.Remove($p)
            }
        }
        $null = $PSBoundParameters.Remove('Pattern')
        $scriptCmd = { 
            Select-String @selectStringParams -LiteralPath (& $wrappedCmd @PSBoundParameters -AsArray) 
        }
        $steppablePipeline = $scriptCmd.GetSteppablePipeline($myInvocation.CommandOrigin)
        $steppablePipeline.Begin($PSCmdlet)
    } 
    catch [System.Management.Automation.ParameterBindingException]
	{
        if($_.Exception.ParameterName -eq 'LiteralPath')
        {
           $PSCmdlet.WriteDebug("Search-Everything returned empty result set")
        }
        else{
            throw
        }
    }
    catch {
        throw
    }
}
process
{
    try {
        $steppablePipeline.Process($_)         
    } catch {
        throw
    }
}

end
{
    try {
        $steppablePipeline.End()
    } catch {
        throw
    }
}
}

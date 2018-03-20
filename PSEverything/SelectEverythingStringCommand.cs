using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using System.Collections.Generic;

namespace PSEverything
{
    [Cmdlet(VerbsCommon.Select, "EverythingString", DefaultParameterSetName = "default")]
    [OutputType("Microsoft.PowerShell.Commands.MatchInfo")]
    [Alias("sles")]
    public class SelectEverythingStringCommand : PSCmdlet
    {
        static readonly string[] SearchParamNames = new[]{
            nameof(CaseSensitiveSearch),
            nameof(ChildFileName),
            nameof(Exclude),
            nameof(Extension),
            nameof(Filter),
            nameof(FolderExclude),
            nameof(FolderInclude),
            nameof(Global),
            nameof(Include),
            nameof(MatchWholeWord),
            nameof(NameLength),
            nameof(ParentCount),
            nameof(PathExclude),
            nameof(PathInclude),
            nameof(RegularExpression)
        };
        [Parameter(Mandatory = true, Position = 1)]
        public string[] Pattern { get; set; }

        [Parameter]
        public SwitchParameter SimpleMatch { get; set; }

        [Parameter]
        public SwitchParameter Quiet { get; set; }

        [Parameter]
        public SwitchParameter AllMatches { get; set; }
        [Parameter]
        public SwitchParameter List { get; set; }

        [Parameter]
        public SwitchParameter CaseSensitivePattern { get; set; }

        [Parameter]
        public SwitchParameter NotMatch { get; set; }

        [ValidateSet("unicode", "utf7", "utf8", "utf32", "ascii", "bigendianunicode", "default", "oem")]
        [ValidateNotNullOrEmpty]
        public string Encoding { get; set; }

        [ValidateCount(1, 2)]
        [ValidateNotNullOrEmpty]
        [ValidateRange(0, 2147483647)]
        public int[] Context { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string Filter { get; set; }

        [Parameter(ParameterSetName = "default", Position = 2)]
        public string[] Include { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string[] Exclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string[] Extension { get; set; }

        [Parameter(ParameterSetName = "default")]
        [Alias("pi")]
        public string[] PathInclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        [Alias("pe")]
        public string[] PathExclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        [Alias("fi")]
        public string[] FolderInclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        [Alias("fe")]
        public string[] FolderExclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        public int ParentCount { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string ChildFileName { get; set; }

        [Parameter(ParameterSetName = "default")]
        [ValidateCount(1, 2)]
        public int[] NameLength { get; set; }

        [Parameter(ParameterSetName = "default")]
        [ValidateCount(1, 2)]
        public long[] Size { get; set; }

        [Parameter(ParameterSetName = "regex")]
        public string RegularExpression { get; set; }

        [Parameter(ParameterSetName = "regex")]
        public SwitchParameter CaseSensitiveSearch;

        [Parameter]
        public SwitchParameter Global { get; set; }

        [Parameter(ParameterSetName = "default")]
        public SwitchParameter MatchWholeWord { get; set; }

        PowerShell _powershell;
        protected override void EndProcessing()
        {
            var searchParams = new Dictionary<string, object>(SearchParamNames.Length)
            {
                { "AsArray", true }
            };
            var bound = MyInvocation.BoundParameters;
            foreach (var sp in SearchParamNames)
            {
                if (bound.TryGetValue(sp, out object val))
                {
                    searchParams.Add(sp == nameof(CaseSensitiveSearch) ? "CaseSenitive" : sp, val);
                    bound.Remove(sp);
                }
            }

            if (bound.TryGetValue(nameof(CaseSensitivePattern), out var cs))
            {
                bound.Add("CaseSensitive", cs);
                bound.Remove(nameof(CaseSensitivePattern));
            }
            var slsParams = bound;
            using (_powershell = PowerShell.Create(RunspaceMode.CurrentRunspace)) {
                _powershell.AddCommand("Search-Everything").AddParameters(searchParams);
                var paths = _powershell.Invoke<string[]>().First();
                if (_powershell.HadErrors)
                    foreach (var e in _powershell.Streams.Error.ReadAll())
                        WriteError(e);


                if (paths.Length == 0)
                {
                    WriteWarning("No files in search result.");
                    return;
                }

                _powershell.Commands.Clear();
                slsParams.Add("LiteralPath", paths);
                _powershell.AddCommand("Microsoft.PowerShell.Utility\\Select-String").AddParameters(slsParams);
                PSDataCollection<PSObject> output = new PSDataCollection<PSObject>();
                output.DataAdded += (sender, args) => { WriteObject(output.ReadAll(), true); };
                _powershell.Invoke(null, output);
            }
            _powershell = null;
        }

        protected override void StopProcessing()
        {
            _powershell?.Stop();
        }
    }
}

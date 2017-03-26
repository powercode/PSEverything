using System.Linq;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;

namespace PSEverything
{
    [Cmdlet(VerbsCommon.Select, "EverythingString", DefaultParameterSetName = "default")]
    [OutputType(typeof(Microsoft.PowerShell.Commands.MatchInfo))]
    [Alias("sles")]
    public class SelectEverythingStringCommand : PSCmdlet
    {
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

        [Parameter(ParameterSetName = "regex")]
        public SwitchParameter Global { get; set; }

        [Parameter(ParameterSetName = "default")]
        public SwitchParameter MatchWholeWord { get; set; }

        
        protected override void EndProcessing()
        {
            var searchEverything = new SearchEverythingCommand()
            {
                AsArray = true,
                CaseSensitive = CaseSensitiveSearch,
                ChildFileName = ChildFileName,
                Exclude = Exclude,
                Extension = Extension,
                Filter = Filter,
                FolderExclude = FolderExclude,
                FolderInclude = FolderExclude,
                Global = Global,
                Include = Include,
                MatchWholeWord = MatchWholeWord,
                NameLength = NameLength,
                ParentCount = ParentCount,
                PathExclude = PathExclude,
                PathInclude = PathInclude,
                RegularExpression = RegularExpression,                                                
            };
            var paths= searchEverything.Invoke<string[]>().FirstOrDefault();
            var selectStringCommand = new SelectStringCommand()
            {
                LiteralPath = paths,
                Pattern = Pattern,
                AllMatches = AllMatches,
                CaseSensitive = CaseSensitivePattern,
                Context = Context,
                Encoding = Encoding,
                List = List,
                NotMatch = NotMatch,
                Quiet = Quiet,
                SimpleMatch = SimpleMatch                
            };
            WriteObject(selectStringCommand.Invoke<MatchInfo>(), true);
                        
        }
    }
}

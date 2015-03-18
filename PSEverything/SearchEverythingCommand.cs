using System;
using System.Management.Automation;
using System.Text;

namespace PSEverything
{
    [Cmdlet(VerbsCommon.Search, "Everything", DefaultParameterSetName = "Filter")]
    [OutputType(typeof(string))]
    public class SearchEverythingCommand : PSCmdlet
    {
        [Parameter(Position = 1, ParameterSetName = "Pattern")]
        public string Pattern { get; set; }

        [Parameter(ParameterSetName = "Filter", Position = 1)]
        public string[] FilePattern { get; set; }

        [Parameter(ParameterSetName = "Filter", Position = 2)]
        public string[] Extention { get; set; }

        [Parameter(ParameterSetName = "Filter")]
        public string[] PathFilter { get; set; }

        [Parameter(ParameterSetName = "Folder")]
        public string[] Folder { get; set; }

        [Alias("Regex")]
        [Parameter]
        public SwitchParameter RegularExpression { get; set; }
        [Parameter]
        public SwitchParameter CaseSensitive { get; set; }
        [Parameter]
        public SwitchParameter MatchWholeWord { get; set; }
        [Parameter]
        public SwitchParameter Directory { get; set; }

        [Parameter]
        public int Count { get; set; }

        public string GetSearchString()
        {

            var sb = new StringBuilder();
            if (ParameterSetName == "Filter")
            {
                if (Extention != null)
                {
                    sb.Append("ext:");
                    foreach (var ext in Extention)
                    {
                        sb.Append(ext);
                        sb.Append(';');
                    }
                    sb[sb.Length - 1] = ' ';
                }
                if (PathFilter != null)
                {
                    sb.Append("path:");
                    foreach (var fld in PathFilter)
                    {
                        sb.Append(fld);
                        sb.Append(';');
                    }
                    sb[sb.Length - 1] = ' ';
                }
                if (FilePattern != null)
                {
                    sb.Append("file:");
                    foreach (var file in FilePattern)
                    {
                        sb.Append(file);
                        sb.Append(';');
                    }
                    sb[sb.Length - 1] = ' ';
                }
            }
            else if (ParameterSetName == "Pattern")
            {
                sb.Append(Pattern);
            }

            else
            {
                sb.Append("folder:");
                foreach (var folder in Folder)
                {
                    sb.Append(folder);
                    sb.Append(';');
                }
                sb[sb.Length - 1] = ' ';

            }
            return sb.ToString();
        }

        protected override void ProcessRecord()
        {
            Everything.SetMatchCase(CaseSensitive);
            Everything.SetMatchWholeWord(MatchWholeWord);
            Everything.SetRegEx(RegularExpression);
            Everything.SetMatchPath(Directory);

            Everything.SetMax(MyInvocation.BoundParameters.ContainsKey("Count") ? Count : Int32.MaxValue);
            var searchPattern = GetSearchString();
            WriteDebug(searchPattern);
            Everything.SetSearch(searchPattern);
            Everything.Query(true);
            Everything.SortResultsByPath();
            foreach (var path in Everything.GetAllResults())
            {
                WriteObject(path);
            }

        }
    }
}
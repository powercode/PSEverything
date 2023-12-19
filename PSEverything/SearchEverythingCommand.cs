using System;
using System.Management.Automation;
using System.Text;
using System.IO;

namespace PSEverything
{
    [Cmdlet(VerbsCommon.Search, "Everything", SupportsPaging = true, DefaultParameterSetName = "default")]
    [OutputType(new Type[] { typeof(string) })]
    [OutputType(new Type[] { typeof(string[]) })]
    [OutputType(new Type[] { typeof(FileSystemInfo) })]
    [OutputType(new Type[] { typeof(FileSystemInfo[]) })]
    public sealed class SearchEverythingCommand : PSCmdlet, IDisposable
    {
        [Parameter(ParameterSetName = "default")]
        public string Filter { get; set; }

        [Parameter(ParameterSetName = "default", Position = 1)]
        public string[] Include { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string[] Exclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string[] Extension { get; set; }

        [Alias(new[] { "pi" })]
        [Parameter(ParameterSetName = "default")]
        public string[] PathInclude { get; set; }

        [Alias(new[] { "pe" })]
        [Parameter(ParameterSetName = "default")]
        public string[] PathExclude { get; set; }

        [Alias(new[] { "foi" })]
        [Parameter(ParameterSetName = "default")]
        public string[] FolderInclude { get; set; }

        [Alias(new[] { "foe" })]
        [Parameter(ParameterSetName = "default")]
        public string[] FolderExclude { get; set; }

        [Parameter(ParameterSetName = "default")]
        public int ParentCount { get; set; }

        [Parameter(ParameterSetName = "default")]
        public string ChildFileName { get; set; }

        [ValidateCount(1, 2)]
        [Parameter(ParameterSetName = "default")]
        public int[] NameLength { get; set; }

        [ArgumentCompleter(typeof(EverythingArgumentCompleter))]
        [ValidateCount(1, 2)]
        [Parameter(ParameterSetName = "default")]
        public string[] Size { get; set; }

        [Parameter(ParameterSetName = "regex")]
        public string RegularExpression { get; set; }

        [Parameter]
        public SwitchParameter CaseSensitive { get; set; }

        [Parameter]
        public SwitchParameter Global { get; set; }

        [Parameter(ParameterSetName = "default")]
        public SwitchParameter MatchWholeWord { get; set; }

        [Parameter()]
        public SwitchParameter AsArray { get; set; }

        [Alias(["AsFS"])]
        [Parameter()]
        public SwitchParameter AsObject { get; set; }

        private string GetSearchString()
        {
            if (ParameterSetName == "regex") { return RegularExpression; }

            var sb = new StringBuilder();
            AddPathFilter(sb);
            AddFileFilter(sb);
            AddFolderFilter(sb);
            AddPatternFilter(sb);
            AddParentCountFilter(sb);
            AddExtensionFilter(sb);
            AddChildFilter(sb);
            AddSizeFilter(sb);
            AddNameLengthFilter(sb);
            return sb.ToString();
        }

        private void AddPatternFilter(StringBuilder searchBuilder)
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                searchBuilder.Append(' ');
                searchBuilder.Append(Filter);
            }
        }

        private static void AddPath(StringBuilder searchBuilder, string path)
        {
            if (path.IndexOf(' ') == -1)
            {
                searchBuilder.Append(path);
            }
            else
            {
                searchBuilder.Append('"');
                searchBuilder.Append(path);
                searchBuilder.Append('"');
            }
        }


        private static void AddListFilter(StringBuilder searchBuilder, string filterName, string[] include, string[] exclude = null, char separator = ' ')
        {
            if (include == null && exclude == null) return;
            searchBuilder.Append(' ');
            if (include != null)
            {
                foreach (var item in include)
                {
                    searchBuilder.Append(filterName);
                    AddPath(searchBuilder, item);
                    searchBuilder.Append(separator);
                }

            }
            if (exclude != null)
            {
                foreach (var item in exclude)
                {
                    searchBuilder.Append(filterName);
                    searchBuilder.Append('!');
                    AddPath(searchBuilder, item);
                    searchBuilder.Append(separator);
                }
            }
            searchBuilder.Length--;
        }

        private void AddPathFilter(StringBuilder searchBuilder)
        {
            AddListFilter(searchBuilder, "path:", PathInclude, PathExclude);
            if (!Global)
            {
                searchBuilder.Append(" path:");
                AddPath(searchBuilder, SessionState.Path.CurrentFileSystemLocation.ProviderPath);
                if (!SessionState.Path.CurrentFileSystemLocation.ProviderPath.EndsWith("\\"))
                {
                    searchBuilder.Append('\\');
                }
            }
        }

        void AddFileFilter(StringBuilder searchBuilder)
        {
            AddListFilter(searchBuilder, "file:", Include, Exclude);
        }

        void AddFolderFilter(StringBuilder searchBuilder)
        {
            AddListFilter(searchBuilder, "folder:", FolderInclude, FolderExclude);
        }

        void AddExtensionFilter(StringBuilder searchBuilder)
        {
            if (Extension == null) return;
            searchBuilder.Append(" ext:");

            foreach (var item in Extension)
            {
                var ext = item.StartsWith(".") ? item.Substring(1) : item;
                searchBuilder.Append(ext);
                searchBuilder.Append(";");
            }
            searchBuilder.Length--;
        }

        void AddParentCountFilter(StringBuilder searchBuilder)
        {
            if (MyInvocation.BoundParameters.ContainsKey("ParentCount"))
            {
                searchBuilder.Append(" parents:");
                searchBuilder.Append(ParentCount);
            }
        }

        void AddChildFilter(StringBuilder searchBuilder)
        {
            if (!string.IsNullOrEmpty(ChildFileName))
            {
                searchBuilder.Append(" child:");
                searchBuilder.Append(ChildFileName);
            }
        }

        void AddSizeFilter(StringBuilder searchBuilder)
        {
            if (Size != null)
            {
                if (Size.Length == 1)
                {
                    searchBuilder.Append(" size:");
                    searchBuilder.Append(Size[0]);
                }
                else
                {
                    searchBuilder.Append(" size:");
                    searchBuilder.Append(Size[0]);
                    searchBuilder.Append("..");
                    searchBuilder.Append(Size[1]);
                }
            }
        }

        void AddNameLengthFilter(StringBuilder searchBuilder)
        {
            if (NameLength != null)
            {
                if (NameLength.Length == 1)
                {
                    searchBuilder.Append(" len:");
                    searchBuilder.Append(NameLength[0]);
                }
                else
                {
                    searchBuilder.Append(" len:");
                    searchBuilder.Append(NameLength[0]);
                    searchBuilder.Append("..");
                    searchBuilder.Append(NameLength[1]);
                }
            }
        }

        protected override void ProcessRecord()
        {
            Everything.Reset();
            Everything.SetMatchCase(CaseSensitive);
            Everything.SetMatchWholeWord(MatchWholeWord);
            Everything.SetRegEx(!String.IsNullOrEmpty(RegularExpression));

            ulong skip = PagingParameters.Skip;
            if (skip > int.MaxValue)
            {
                ThrowTerminatingError(new ErrorRecord(new ParameterBindingException("Cannot skip that many results"), "SkipToLarge", ErrorCategory.InvalidArgument, skip));
            }

            ulong first = PagingParameters.First;

            if (first == ulong.MaxValue)
            {
                first = int.MaxValue;
            }
            if (first > int.MaxValue)
            {
                ThrowTerminatingError(new ErrorRecord(new ParameterBindingException("Cannot take that many results"), "FirstToLarge", ErrorCategory.InvalidArgument, first));
            }
            if (first < Int32.MaxValue)
            {
                Everything.SetMax((int)first);
            }
            if (skip > 0)
            {
                Everything.SetOffset((int)skip);
            }


            var searchPattern = GetSearchString();
            WriteDebug("Search-Everything search pattern:" + searchPattern);
            Everything.SetSearch(searchPattern);

            try
            {
                Everything.Query(true);
                Everything.SortResultsByPath();
                int resCount = Everything.GetTotalNumberOfResults();
                if (PagingParameters.IncludeTotalCount)
                {
                    var total = PagingParameters.NewTotalCount((ulong)resCount, 1.0);
                    WriteObject(total);
                }
                if (AsObject)
                {
                    FileSystemInfo[] res = Everything.GetAllResultsAsFileSystemInfo(Math.Min(resCount, (int)first));
                    WriteObject(res, enumerateCollection: !AsArray);
                }
                else
                {
                    var res = Everything.GetAllResults(Math.Min(resCount, (int)first));
                    WriteObject(res, enumerateCollection: !AsArray);
                }
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(e, e.Message, ErrorCategory.NotSpecified, searchPattern));
            }
        }

        void IDisposable.Dispose()
        {
            Everything.Cleanup();
        }
    }
}

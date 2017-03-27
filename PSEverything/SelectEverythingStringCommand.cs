using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using System.Collections.Generic;

namespace PSEverything
{
	[Cmdlet(VerbsCommon.Select, "EverythingString", DefaultParameterSetName = "default")]
	[OutputType(typeof(Microsoft.PowerShell.Commands.MatchInfo))]
	[Alias("sles")]
	public class SelectEverythingStringCommand : PSCmdlet
	{
		static readonly string[] SearchParamNames = new[]{
			"CaseSensitive",
			"ChildFileName",
			"Exclude",
			"Extension",
			"Filter",
			"FolderExclude",
			"FolderInclude",
			"Global",
			"Include",
			"MatchWholeWord",
			"NameLength",
			"ParentCount",
			"PathExclude",
			"PathInclude",
			"RegularExpression" };
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
		IAsyncResult _asyncResult;
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
					searchParams.Add(sp, val);
					bound.Remove(sp);
				}
			}

			var slsParams = bound;

			var iss = InitialSessionState.CreateDefault2();
			//iss.Providers.Add(new SessionStateProviderEntry("Microsoft.PowerShell.Core\\FileSystem", typeof(FileSystemProvider), null));
			iss.Commands.Add(new SessionStateCmdletEntry("Search-Everything", typeof(SearchEverythingCommand), null));
			iss.Commands.Add(new SessionStateCmdletEntry("Select-String", typeof(SelectStringCommand), null));
			_powershell = PowerShell.Create(iss);
			_powershell.AddCommand("Search-Everything").AddParameters(searchParams);
			var paths = _powershell.Invoke<string[]>().First();
			if (paths.Length == 0)
			{
				return;
			}

			_powershell.Commands.Clear();
			slsParams.Add("LiteralPath", paths);
			_powershell.AddCommand("Select-String").AddParameters(slsParams);
			var output = new PSDataCollection<object>(1000);

			_asyncResult = _powershell.BeginInvoke<object, object>(null, output);
			try
			{
				while (!_asyncResult.IsCompleted)
				{
					WriteObject(output.ReadAll(), true);
				}
			}
			finally
			{
				WriteObject(_powershell.EndInvoke(_asyncResult), true);
			}
		}

		protected override void StopProcessing()
		{
			if (_asyncResult != null)
			{
				_powershell.Stop();
			}
		}
	}
}

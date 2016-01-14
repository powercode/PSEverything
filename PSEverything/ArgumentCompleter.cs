using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace PSEverything
{
	public class EverythingArgumentCompleter : IArgumentCompleter
	{
		public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete,
			CommandAst commandAst,
			IDictionary fakeBoundParameters)
		{
			switch (commandName)
			{
				case "Search-Everything":
					return CompleteSearchEverything(parameterName, wordToComplete, commandAst, fakeBoundParameters);
				default:
					return null;
			}
		}

		private IEnumerable<CompletionResult> CompleteSearchEverything(string parameterName, string wordToComplete,
			CommandAst commandAst, IDictionary fakeBoundParameters)
		{
			switch (parameterName)
			{
				case "Size":
					return CompleteSize(wordToComplete);
				default:
					return null;
			}
		}

		private string[] _sizeConstants;
		private string[] SizeConstants => _sizeConstants ?? ( _sizeConstants = new string[]{
			"empty",
			"tiny",
			"small",
			"medium",
			"large",
			"huge",
			"gigantic"
		});
		private string[] _sizeConstantsToolTips;
		private string[] SizeConstantToolTips => _sizeConstantsToolTips ?? (_sizeConstantsToolTips = new string[]{
			"empty",
			"0 KB < size <= 10 KB",
			"10 KB < size <= 100 KB",
			"100 KB < size <= 1 MB",
			"1 MB < size <= 16 MB",
			"16 MB < size <= 128 MB",
			" size > 128 MB"
		});

		private IEnumerable<CompletionResult> CompleteSize(string wordToComplete)
		{
			var sizeConstants = SizeConstants;
			var count = SizeConstants.Count(c => c.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase));
			var retval = new CompletionResult[count];
			var retIndex = 0;
			for (int i = 0; i < sizeConstants.Length; i++)
			{
				var sizeConstant = sizeConstants[i];
				if (!sizeConstant.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)) continue;
				var tooltip = SizeConstantToolTips[i];
				retval[retIndex] = new CompletionResult(sizeConstant, sizeConstant, CompletionResultType.ParameterValue, tooltip);
				retIndex++;
			}
			return retval;
		}
	}
}

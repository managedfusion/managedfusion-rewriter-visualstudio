using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;


namespace ManagedFusion.Rewriter.VisualStudio
{
	internal static class ApacheClassifierDefinitions
	{
		#region Classification Type Definitions

		[Export]
		[Name("apache.rules.keyword")]
		[BaseDefinition("keyword")]
		internal static ClassificationTypeDefinition RulesKeywordClassificationDefinition;

		[Export]
		[Name("apache.rules.comment")]
		[BaseDefinition("comment")]
		internal static ClassificationTypeDefinition RulesCommentsAddedDefinition;

		#endregion
	}
}

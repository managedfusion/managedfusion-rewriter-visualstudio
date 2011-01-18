using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ManagedFusion.Rewriter.VisualStudio
{
	internal static class ApacheContentType
	{
		internal const string Name = "apache.rules";

		#region Content Type and File Extension Definitions

		[Export]
		[Name(Name)]
		[BaseDefinition("text")]
		internal static ContentTypeDefinition RulesContentTypeDefinition;

		[Export]
		[FileExtension(".rules")]
		[ContentType(Name)]
		internal static FileExtensionToContentTypeDefinition RulesFileExtensionDefinition;

		[Export]
		[FileExtension(".txt")]
		[ContentType(Name)]
		internal static FileExtensionToContentTypeDefinition TxtFileExtensionDefinition;

		#endregion
	}
}
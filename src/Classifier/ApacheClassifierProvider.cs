using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ManagedFusion.Rewriter.VisualStudio
{
	/// <summary>
	/// This class causes a classifier to be added to the set of classifiers. Since 
	/// the content type is set to "text", this classifier applies to all text files
	/// </summary>
	[Export(typeof(IClassifierProvider))]
	[ContentType("apache.rules")]
	internal class ApacheClassifierProvider : IClassifierProvider
	{
		/// <summary>
		/// Import the classification registry to be used for getting a reference
		/// to the custom classification type later.
		/// </summary>
		[Import]
		internal IClassificationTypeRegistryService ClassificationRegistry;

		#region IClassifierProvider Members

		public IClassifier GetClassifier(ITextBuffer textBuffer)
		{
			return textBuffer.Properties.GetOrCreateSingletonProperty<ApacheClassifier>(delegate { return new ApacheClassifier(ClassificationRegistry); });
		}

		#endregion
	}
}

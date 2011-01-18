using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;

namespace ManagedFusion.Rewriter.VisualStudio.Intellisense
{
	[Export(typeof(IIntellisenseControllerProvider))]
	[Name("Apache mod_rewrite Completion Controller")]
	[Order(Before = "Default Completion Controller")]
	[ContentType(ApacheContentType.Name)]
	internal class ApacheIntellisenseControllerProvider : IIntellisenseControllerProvider
	{
		[Import]
		private IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

		[Import]
		private ICompletionBroker CompletionBroker { get; set; }

		#region IIntellisenseControllerProvider Members

		public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
		{
			return new ApacheIntellisenseController(subjectBuffers, textView, CompletionBroker);
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace ManagedFusion.Rewriter.VisualStudio.Intellisense
{
	[Export(typeof(ICompletionSourceProvider))]
    [Name("Apache mod_rewrite Completion Source Provider")]
	[Order(Before = "default")]
	[ContentType(ApacheContentType.Name)]
	internal class ApacheCompletionSourceProvider : ICompletionSourceProvider
	{
		#region ICompletionSourceProvider Members

		public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

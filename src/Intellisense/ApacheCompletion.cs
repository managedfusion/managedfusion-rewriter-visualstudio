using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;

namespace ManagedFusion.Rewriter.VisualStudio.Intellisense
{
	internal class ApacheCompletion : Completion
	{
		internal ApacheCompletion(string displayText, string insertionText, string description)
			: base(displayText)
		{
			// add the icon to the completion
			base.Properties.AddProperty(typeof(IconDescription), new IconDescription(StandardGlyphGroup.GlyphGroupTypedef, StandardGlyphItem.GlyphItemPublic));
		}
	}
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;

namespace ManagedFusion.Rewriter.VisualStudio.Intellisense
{
	internal class ApacheCompletionSource : ICompletionSource
	{
		#region ICompletionSource Members

		public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
		{
			int triggerPointPosition = session.GetTriggerPoint(session.TextView.TextBuffer).GetPosition(session.TextView.TextSnapshot);
			ITrackingSpan trackingSpan = session.TextView.TextSnapshot.CreateTrackingSpan(triggerPointPosition, 0, SpanTrackingMode.EdgeInclusive);

			var rewriteDirectives = new[] {
				new ApacheCompletion("RewriteBase", "RewriteBase", "The RewriteBase directive explicitly sets the base URL for per-directory rewrites."),
				new ApacheCompletion("RewriteCond", "RewriteCond", "The RewriteCond directive defines a rule condition. One or more RewriteCond can precede a RewriteRule directive."),
				new ApacheCompletion("RewriteEngine", "RewriteEngine", "The RewriteEngine directive enables or disables the runtime rewriting engine."),
				new ApacheCompletion("RewriteLock", "RewriteLock", "This directive sets the filename for a synchronization lockfile which mod_rewrite needs to communicate with RewriteMap programs."),
				new ApacheCompletion("RewriteLog", "RewriteLog", "The RewriteLog directive sets the name of the file to which the server logs any rewriting actions it performs."),
				new ApacheCompletion("RewriteLogLevel", "RewriteLogLevel", "The RewriteLogLevel directive sets the verbosity level of the rewriting logfile."),
				new ApacheCompletion("RewriteMap", "RewriteMap", "The RewriteMap directive defines a Rewriting Map which can be used inside rule substitution strings by the mapping-functions to insert/substitute fields through a key lookup."),
				new ApacheCompletion("RewriteOptions", "RewriteOptions", "The RewriteOptions directive sets some special options for the current per-server or per-directory configuration."),
				new ApacheCompletion("RewriteRule", "RewriteRule", "The RewriteRule directive is the real rewriting workhorse. The directive can occur more than once, with each instance defining a single rewrite rule. The order in which these rules are defined is important - this is the order in which they will be applied at run-time.")
			};

			var completionSet = new CompletionSet(
				ApacheContentType.Name,
				"Apache Rewrite Directives",
				trackingSpan,
				rewriteDirectives,
				null
			);

			completionSets.Add(completionSet);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}

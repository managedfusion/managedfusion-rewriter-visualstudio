using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Text.RegularExpressions;

namespace ManagedFusion.Rewriter.VisualStudio
{
	/// <summary>
	/// Classifier that classifies all text as an instance of the OrinaryClassifierType
	/// </summary>
	internal sealed class ApacheClassifier : IClassifier
	{
		// This event gets raised if a non-text change would affect the classification in some way,
		// for example typing /* would cause the classification to change in C# without directly
		// affecting the span.
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

		private IClassificationType _keywordClassificationType;
		private IClassificationType _commentClassificationType;

		internal ApacheClassifier(IClassificationTypeRegistryService registry)
		{
			_keywordClassificationType = registry.GetClassificationType("apache.rules.keyword");
			_commentClassificationType = registry.GetClassificationType("apache.rules.comment");
		}

		/// <summary>
		/// This method scans the given SnapshotSpan for potential matches for this classification.
		/// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
		/// </summary>
		/// <param name="trackingSpan">The span currently being classified</param>
		/// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			List<ClassificationSpan> classifications = new List<ClassificationSpan>();

			/*
			 * Comment Classifications
			 */
			if (span.IsComment())
				classifications.Add(new ClassificationSpan(span, _commentClassificationType));

			/*
			 * Keyword Classifications
			 */
			if (span.IsCommand())
			{
				string command = span.GetCommand();
				classifications.Add(new ClassificationSpan(new SnapshotSpan(span.Start, command.Length), _keywordClassificationType));
			}

			return classifications;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;
using System.Text.RegularExpressions;

namespace ManagedFusion.Rewriter.VisualStudio
{
	internal static class ApacheLineExtensions
	{
		public static readonly string[] RewriteKeywords = new string[] { "RewriteModule", "RewriteBase", "RewriteCond", "RewriteEngine", "RewriteLock", "RewriteLog", "RewriteLogLevel", "RewriteMap", "RewriteOptions", "RewriteRule", "OutRewriteRule", "OutRewriteCond" };
		public static readonly ILookup<int, string> RewriteKeywordsLookup;
		public static readonly int MaxRewriteKeywordLength;

		static ApacheLineExtensions()
		{
			RewriteKeywordsLookup = RewriteKeywords.ToLookup(keyword => keyword.Length);
			MaxRewriteKeywordLength = RewriteKeywords.Max(keyword => keyword.Length);
		}

		public static bool IsComment(this ITextSnapshotLine line)
		{
			SnapshotSpan span = new SnapshotSpan(line.Start, line.End);
			return span.IsComment();
		}

		public static bool IsComment(this SnapshotSpan span)
		{
			return span.Length > 0 && span.Snapshot[span.Start] == '#';
		}

		public static bool IsCommand(this ITextSnapshotLine line)
		{
			SnapshotSpan span = new SnapshotSpan(line.Start, line.End);
			return span.IsCommand();
		}

		public static bool IsCommand(this SnapshotSpan span)
		{
			string test = span.GetCommand();

			if (test == null || test.Length == 0)
				return false;

			var possibleKeywords = RewriteKeywordsLookup[test.Length];
			return possibleKeywords.Contains(test);
		}

		public static string GetCommand(this SnapshotSpan span)
		{
			string test = span.GetText();

			if (test == null || test.Length == 0)
				return String.Empty;

			int endIndex = test.IndexOf(' ');

			if (endIndex > 0 || test.Length > MaxRewriteKeywordLength)
				test = test.Substring(0, endIndex);

			return test;
		}

		public static IEnumerable<ITextSnapshotLine> ApacheCommentLines(this IEnumerable<ITextSnapshotLine> lines)
		{
			foreach (var line in lines)
				if (line.Length > 0 && line.Snapshot[line.Start] == '#')
					yield return line;
		}

		public static IEnumerable<ITextSnapshotLine> ApacheCommandLines(this IEnumerable<ITextSnapshotLine> lines)
		{
			foreach (var line in lines)
				if (line.IsCommand())
					yield return line;
		}
	}
}

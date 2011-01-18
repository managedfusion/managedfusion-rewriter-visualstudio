using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Xaml;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace ManagedFusion.Rewriter.VisualStudio.Intellisense
{
	internal class ApacheIntellisenseController : IIntellisenseController
    {
		private ITextView subjectTextView;
		private IList<ITextBuffer> subjectBuffers;
		private ICompletionBroker completionBroker;
		private ICompletionSession activeSession;

		private int triggerPosition;
		private ITrackingSpan completionSpan;
		private IWpfTextView wpfTextView;

		internal ApacheIntellisenseController(IList<ITextBuffer> subjectBuffers, ITextView subjectTextView, ICompletionBroker completionBroker)
        {
			this.subjectBuffers = subjectBuffers;
			this.subjectTextView = subjectTextView;
			this.completionBroker = completionBroker;

			wpfTextView = subjectTextView as IWpfTextView;
			if (wpfTextView != null)
			{
				wpfTextView.VisualElement.KeyDown += new System.Windows.Input.KeyEventHandler(OnKeyDown);
				wpfTextView.VisualElement.KeyUp += new System.Windows.Input.KeyEventHandler(OnKeyUp);
			}
        }

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			// Make sure that this event happened on the same text view to which we're attached.
			ITextView textView = sender as ITextView;
			if (this.subjectTextView != textView)
				return;

			if (activeSession == null)
				return;

			ITrackingSpan span;
			activeSession.Properties.TryGetProperty<ITrackingSpan>(typeof(ApacheIntellisenseController), out span);
			string str = span.GetText(span.TextBuffer.CurrentSnapshot);

			switch (e.Key)
			{
				case Key.Space:
				case Key.Escape:
					activeSession.Dismiss();
					e.Handled = true;
					return;

				case Key.Left:
					if (textView.Caret.Position.BufferPosition.Position <= triggerPosition)
						// we went too far to the left
						activeSession.Dismiss();
					return;

				case Key.Right:
					if (textView.Caret.Position.BufferPosition.Position > triggerPosition + completionSpan.GetSpan(completionSpan.TextBuffer.CurrentSnapshot).Length)
						// we went too far to the right
						activeSession.Dismiss();
					return;

				case Key.Tab:
				case Key.Enter:
					if (this.activeSession.SelectedCompletionSet.SelectionStatus != null)
						activeSession.Commit();
					else
						activeSession.Dismiss();
					e.Handled = true;
					return;

				default:
					break;
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			// Make sure that this event happened on the same text view to which we're attached.
			ITextView textView = sender as ITextView;
			if (this.subjectTextView != textView)
				return;

			// if there is a session already leave it be
			if (activeSession != null)
				return;

			// determine which subject buffer is affected by looking at the caret position
			SnapshotPoint? caret = textView.Caret.Position.Point.GetPoint(textBuffer => (
					subjectBuffers.Contains(textBuffer)),
					PositionAffinity.Predecessor);

			// return if no suitable buffer found
			if (!caret.HasValue)
				return;

			SnapshotPoint caretPoint = caret.Value;
			ITextSnapshotLine line = caretPoint.GetContainingLine();
			SnapshotSpan span = new SnapshotSpan(line.Start, line.End);

			if (span.IsComment() || span.IsCommand())
				return;

			// the invocation occurred in a subject buffer of interest to us
			triggerPosition = caretPoint.Position;
			ITrackingPoint triggerPoint = caretPoint.Snapshot.CreateTrackingPoint(triggerPosition, PointTrackingMode.Negative);
			completionSpan = caretPoint.Snapshot.CreateTrackingSpan(caretPoint.Position, 0, SpanTrackingMode.EdgeInclusive);

			// Create a completion session
			activeSession = completionBroker.CreateCompletionSession(textView, triggerPoint, true);

			// Put the completion context and original (empty) completion span
			// on the session so that it can be used by the completion source
			activeSession.Properties.AddProperty(typeof(ApacheIntellisenseController), completionSpan);

			// Attach to the session events
			activeSession.Dismissed += new System.EventHandler(OnActiveSessionDismissed);
			activeSession.Committed += new System.EventHandler(OnActiveSessionCommitted);

			// Start the completion session. The intellisense will be triggered.
			activeSession.Start();
		}

		private void OnActiveSessionDismissed(object sender, System.EventArgs e)
		{
			activeSession = null;
		}

		private void OnActiveSessionCommitted(object sender, System.EventArgs e)
		{
			activeSession = null;
		}

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
			wpfTextView = subjectTextView as IWpfTextView;
			if (wpfTextView != null)
			{
				wpfTextView.VisualElement.KeyDown -= new System.Windows.Input.KeyEventHandler(OnKeyDown);
				wpfTextView.VisualElement.KeyUp -= new System.Windows.Input.KeyEventHandler(OnKeyUp);
			}
        }

        /// <summary>
        /// Detaches the events
        /// </summary>
        /// <param name="textView"></param>
        public void Detach(ITextView textView)
        {
            if (textView == null)
                throw new InvalidOperationException("Already detached from text view");

            if (this.subjectTextView != textView)
                throw new ArgumentException("Not attached to specified text view", "textView");
        }
    }
}

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Framework.Components;

namespace Subtext.Extensibility.Plugins
{
	public class FeedbackEventArgs: SubtextEventArgs
	{
		private FeedbackItem _feedback;
		/// <summary>
		/// The Feedback being manipulated
		/// </summary>
		public FeedbackItem Entry
		{
			get { return _feedback; }
		}


		public FeedbackEventArgs(FeedbackItem feedback, ObjectState state)
			: base(state)
		{
			_feedback = feedback;
		}

		public FeedbackEventArgs(FeedbackItem feedback) : this(feedback, ObjectState.None) { }

	}
}

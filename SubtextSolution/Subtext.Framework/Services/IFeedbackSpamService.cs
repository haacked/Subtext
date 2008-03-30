using System;
using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
	/// <summary>
	/// Base interface for comment spam services such as Akismet.
	/// </summary>
	public interface IFeedbackSpamService
	{
		/// <summary>
		/// Examines the item and determines whether or not it is spam.
		/// </summary>
		/// <param name="feedback"></param>
		/// <returns></returns>
		bool IsSpam(FeedbackItem feedback);

		/// <summary>
		/// Submits the item to the service as a false positive. 
		/// Something that should not have been marked as spam.
		/// </summary>
		/// <param name="feedback"></param>
		void SubmitGoodFeedback(FeedbackItem feedback);

		/// <summary>
		/// Submits the item to the service as a piece of SPAM that got through 
		/// the filter. Something that should've been marked as SPAM.
		/// </summary>
		/// <param name="feedback"></param>
		void SubmitSpam(FeedbackItem feedback);
	}
}

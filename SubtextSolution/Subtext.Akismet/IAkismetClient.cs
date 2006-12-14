using System;
using System.Net;

namespace Subtext.Akismet
{
	/// <summary>
	/// The client class used to communicate with the 
	/// <see href="http://akismet.com/">Akismet</see> service.
	/// </summary>
	public interface IAkismetClient
	{
		/// <summary>
		/// Gets or sets the Akismet API key.
		/// </summary>
		/// <value>The API key.</value>
		string ApiKey { get; set; }

		/// <summary>
		/// Gets or sets the root URL to the blog.
		/// </summary>
		/// <value>The blog URL.</value>
		Uri BlogUrl { get; set; }

		/// <summary>
		/// Checks the comment and returns true if it is spam, otherwise false.
		/// </summary>
		/// <param name="comment"></param>
		/// <returns></returns>
		bool CheckCommentForSpam(IComment comment);

		/// <summary>
		/// Gets or sets the proxy to use.
		/// </summary>
		/// <value>The proxy.</value>
		IWebProxy Proxy { get; set; }

		/// <summary>
		/// Submits a comment to Akismet that should not have been 
		/// flagged as SPAM (a false positive).
		/// </summary>
		/// <param name="comment"></param>
		/// <returns></returns>
		void SubmitHam(IComment comment);

		/// <summary>
		/// Submits a comment to Akismet that should have been 
		/// flagged as SPAM, but was not flagged by Akismet.
		/// </summary>
		/// <param name="comment"></param>
		/// <returns></returns>
		void SubmitSpam(IComment comment);

		/// <summary>
		/// Gets or sets the timeout in milliseconds for the http request to Akismet. 
		/// </summary>
		/// <value>The timeout.</value>
		int Timeout { get; set; }

		/// <summary>
		/// Gets or sets the Usera Agent for the Akismet Client.  
		/// Do not confuse this with the user agent for the comment 
		/// being checked.
		/// </summary>
		string UserAgent { get; set; }

		/// <summary>
		/// Verifies the API key.  You really only need to
		/// call this once, perhaps at startup.
		/// </summary>
		/// <returns></returns>
		bool VerifyApiKey();
	}
}

using System;
using System.Collections.Specialized;
using System.Net;

namespace Subtext.Akismet
{
	public class Comment : IComment
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Comment"/> class.
		/// </summary>
		/// <param name="authorIpAddress">The author ip address.</param>
		/// <param name="authorUserAgent">The author user agent.</param>
		public Comment(IPAddress authorIpAddress, string authorUserAgent)
		{
			IpAddress = authorIpAddress;
			UserAgent = authorUserAgent;
		}
		
		/// <summary>
		/// The name submitted with the comment.
		/// </summary>
		public string Author
		{
			get;
			set;
		}

		/// <summary>
		/// The email submitted with the comment.
		/// </summary>
		public string AuthorEmail
		{
			get;
			set;
		}

		/// <summary>
		/// The url submitted if provided.
		/// </summary>
		public Uri AuthorUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Content of the comment
		/// </summary>
		public string Content
		{
			get;
			set;
		}

		/// <summary>
		/// The HTTP_REFERER header value of the 
		/// originating comment.
		/// </summary>
		public string Referer
		{
			get;
			set;
		}

		/// <summary>
		/// Permanent location of the entry the comment was 
		/// submitted to.
		/// </summary>
		public Uri Permalink
		{
			get;
			set;
		}

		/// <summary>
		/// User agent of the requester. (Required)
		/// </summary>
		public string UserAgent
		{
			get;
            private set;
		}

		/// <summary>
		/// May be one of the following: {blank}, "comment", "trackback", "pingback", or a made-up value 
		/// like "registration".
		/// </summary>
		public string CommentType
		{
			get;
			set;
		}

		/// <summary>
		/// IPAddress of the submitter
		/// </summary>
        public IPAddress IpAddress
        {
            get;
            private set;
        }

		/// <summary>
		/// Optional collection of various server environment variables. 
		/// </summary>
        public NameValueCollection ServerEnvironmentVariables
        {
            get {
                _serverEnvironmentVariables = _serverEnvironmentVariables ?? new NameValueCollection();
                return _serverEnvironmentVariables;
            }
        }
        NameValueCollection _serverEnvironmentVariables;

    }
}
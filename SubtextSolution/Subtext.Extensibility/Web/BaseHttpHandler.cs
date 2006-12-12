using System;
using System.Net;
using System.Web;
using Subtext.Extensibility.Properties;

namespace Subtext.Extensibility.Web
{
	/// <summary>
	/// An abstract base Http Handler for all your
	/// <see cref="IHttpHandler"/> needs.
	/// </summary>
	/// <remarks>
	/// <p>
	/// For the most part, classes that inherit from this
	/// class do not need to override <see cref="ProcessRequest"/>.
	/// Instead implement the abstract methods and
	/// properties and put the main business logic
	/// in the <see cref="HandleRequest"/>.
	/// </p>
	/// <p>
	/// HandleRequest should respond with a StatusCode of
	/// 200 if everything goes well, otherwise use one of
	/// the various "Respond" methods to generate an appropriate
	/// response code.  Or use the HttpStatusCode enumeration
	/// if none of these apply.
	/// </p>
	/// </remarks>
	public abstract class BaseHttpHandler : IHttpHandler
	{
		/// <summary>
		/// Processs the incoming HTTP request.
		/// </summary>
		/// <param name="context">Context.</param>
		public void ProcessRequest(HttpContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }

			SetResponseCachePolicy(context.Response.Cache);

			if (!ValidateParameters(context))
			{
				RespondInternalError(context);
				return;
			}

			if (RequiresAuthentication
				&& (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated))
			{
				RespondForbidden(context);
				return;
			}

			context.Response.ContentType = ContentMimeType;

			HandleRequest(context);
		}

		/// <summary>
		/// Indicates whether or not this handler can be
		/// reused between successive requests.
		/// </summary>
		/// <remarks>
		/// Return true if this handler does not maintain
		/// any state (generally a good practice).  Otherwise
		/// returns false.
		/// </remarks>
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Handles the request.  This is where you put your
		/// business logic.
		/// </summary>
		/// <remarks>
		/// <p>This method should result in a call to one 
		/// (or more) of the following methods:</p>
		/// <p><code>context.Response.BinaryWrite();</code></p>
		/// <p><code>context.Response.Write();</code></p>
		/// <p><code>context.Response.WriteFile();</code></p>
		/// <p>
		/// <code>
		/// someStream.Save(context.Response.OutputStream);
		/// </code>
		/// </p>
		/// <p>etc...</p>
		/// <p>
		/// If you want a download box to show up with a 
		/// pre-populated filename, add this call here 
		/// (supplying a real filename).
		/// </p>
		/// <p>
		/// </p>
		/// <code>Response.AddHeader("Content-Disposition"
		/// , "attachment; filename=\"" + Filename + "\"");</code>
		/// </p>
		/// </remarks>
		/// <param name="context">Context.</param>
		public abstract void HandleRequest(HttpContext context);

		/// <summary>
		/// Validates the parameters.  Inheriting classes must
		/// implement this and return true if the parameters are
		/// valid, otherwise false.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns><c>true</c> if the parameters are valid,
		/// otherwise <c>false</c></returns>
		public abstract bool ValidateParameters(HttpContext context);

		/// <summary>
		/// Gets a value indicating whether this handler
		/// requires users to be authenticated.
		/// </summary>
		/// <value>
		///    <c>true</c> if authentication is required
		///    otherwise, <c>false</c>.
		/// </value>
		public abstract bool RequiresAuthentication { get;}

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		public abstract string ContentMimeType { get;}

		/// <summary>
		/// Sets the cache policy.  Unless a handler overrides
		/// this method, handlers will not allow a respons to be
		/// cached.
		/// </summary>
		/// <param name="cache">Cache.</param>
		public virtual void SetResponseCachePolicy
			(HttpCachePolicy cache)
		{
            if (cache == null)
            {
                throw new ArgumentNullException("cache", Resources.ArgumentNull_Generic);
            }
            
            cache.SetCacheability(HttpCacheability.NoCache);
			cache.SetNoStore();
			cache.SetExpires(DateTime.MinValue);
		}

		/// <summary>
		/// Helper method used to Respond to the request
		/// that the file was not found.
		/// </summary>
		/// <param name="context">Context.</param>
		protected static void RespondFileNotFound(HttpContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }
            
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			context.Response.End();
		}

		/// <summary>
		/// Helper method used to Respond to the request
		/// that an error occurred in processing the request.
		/// </summary>
		/// <param name="context">Context.</param>
		protected static void RespondInternalError(HttpContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }

			// It's really too bad that StatusCode property
			// is not of type HttpStatusCode.
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.End();
		}

		/// <summary>
		/// Helper method used to Respond to the request
		/// that the request in attempting to access a resource
		/// that the user does not have access to.
		/// </summary>
		/// <param name="context">Context.</param>
		protected static void RespondForbidden(HttpContext context)
		{
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }
            
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
			context.Response.End();
		}
	}
}

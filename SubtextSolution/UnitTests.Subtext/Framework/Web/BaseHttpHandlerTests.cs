using System;
using System.Net;
using System.Web;
using MbUnit.Framework;
using Subtext.Extensibility.Web;
using Subtext.TestLibrary;

namespace UnitTests.Subtext.Framework.Web
{
	[TestFixture]
	public class BaseHttpHandlerTests
	{
		[Test]
		public void BaseHttpHandlerIsReusable()
		{
			TestHttpHandler handler = new TestHttpHandler(false, false);
			Assert.IsTrue(handler.IsReusable);
		}

		[Test]
		public void ProcessRequestRespondsWithInternalErrorIfParametersInvalid()
		{
			TestHttpHandler handler = new TestHttpHandler(false, false);
			using (new HttpSimulator().SimulateRequest())
			{
				handler.ProcessRequest(HttpContext.Current);
				Assert.AreEqual((int) HttpStatusCode.InternalServerError, HttpContext.Current.Response.StatusCode);
			}
		}

		[Test]
		public void ProcessRequestRespondsWithForbiddenIfRequiresAuthenticationButUserNotAuthenticated()
		{
			TestHttpHandler handler = new TestHttpHandler(true, true);
			using (new HttpSimulator().SimulateRequest())
			{
				handler.ProcessRequest(HttpContext.Current);
				Assert.AreEqual((int) HttpStatusCode.Forbidden, HttpContext.Current.Response.StatusCode);
			}
		}

		[Test]
		public void CanRespondWithFileNotFound()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				TestHttpHandler.Respond404(HttpContext.Current);
				Assert.AreEqual((int) HttpStatusCode.NotFound, HttpContext.Current.Response.StatusCode);
			}
		}

		#region Exception Tests
		[Test]
		[ExpectedArgumentNullException]
		public void ProcessRequestThrowsArgumentNullException()
		{
			TestHttpHandler handler = new TestHttpHandler(true, false);
			handler.ProcessRequest(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void SetResponseCachePolicyThrowsArgumentNullException()
		{
			TestHttpHandler handler = new TestHttpHandler(true, false);
			handler.SetResponseCachePolicy(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void RespondFileNotFoundThrowsArgumentNullException()
		{
			TestHttpHandler.Respond404(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void RespondInternalErrorThrowsArgumentNullException()
		{
			TestHttpHandler.Respond500(null);
		}
		
		[Test]
		[ExpectedArgumentNullException]
		public void RespondForbiddenThrowsArgumentNullException()
		{
			TestHttpHandler.Respond403(null);
		}
		#endregion
	}

	internal class TestHttpHandler : BaseHttpHandler
	{
		private bool validParameters;
		private bool requiresAuthentication;

		internal TestHttpHandler(bool validParameters, bool requiresAuthentication)
		{
			this.validParameters = validParameters;
			this.requiresAuthentication = requiresAuthentication;
		}

		public override void HandleRequest(HttpContext context)
		{
		}

		public static void Respond404(HttpContext context)
		{
			RespondFileNotFound(context);
		}

		public static void Respond403(HttpContext context)
		{
			RespondForbidden(context);
		}

		public static void Respond500(HttpContext context)
		{
			RespondInternalError(context);
		}

		/// <summary>
		/// Validates the parameters.  Inheriting classes must
		/// implement this and return true if the parameters are
		/// valid, otherwise false.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns><c>true</c> if the parameters are valid,
		/// otherwise <c>false</c></returns>
		public override bool ValidateParameters(HttpContext context)
		{
			return validParameters;
		}

		/// <summary>
		/// Gets a value indicating whether this handler
		/// requires users to be authenticated.
		/// </summary>
		/// <value>
		///    <c>true</c> if authentication is required
		///    otherwise, <c>false</c>.
		/// </value>
		public override bool RequiresAuthentication
		{
			get { return requiresAuthentication; }
		}

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		public override string ContentMimeType
		{
			get { throw new NotImplementedException(); }
		}
	}
}

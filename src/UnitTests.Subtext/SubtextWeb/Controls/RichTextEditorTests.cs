using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Web.Controls;
using Subtext.Web.Providers.BlogEntryEditor.FTB;
using Subtext.Framework.Web.Handlers;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Routing;
using System.Web.Routing;
using System.Web;
using Subtext.Web.Admin;
using Subtext.Web.Providers.BlogEntryEditor.PlainText;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestFixture]
    public class RichTextEditorTests
    {
        [Test]
        public void RichTextEditor_CreateDefaultProvider()
        {
            using (var httpRequest = new HttpSimulator().SimulateRequest())
            {
                //arrange
                var context = new Mock<ISubtextContext>();
                var httpContext = new Mock<HttpContextBase>();
                httpContext.Setup(c => c.Request.ApplicationPath).Returns("path");
                context.Setup(c => c.UrlHelper).Returns(
                    new BlogUrlHelper(new RequestContext(httpContext.Object, new RouteData()), null));
                context.Setup(c => c.Blog).Returns(new Blog { Host = "host" });
                var page = new SubtextPage { SubtextContext = context.Object };
                var editor = new RichTextEditor { Page = page };

                //act
                editor.InitControls(new EventArgs());

                //post
                var provider = editor.Provider;
                Assert.IsTrue(provider is FtbBlogEntryEditorProvider, "FtbBlogEntryEditorProvider is created by default.");
            }
        }

        [Test]
        public void RichTextEditor_CreatePlainText_IfPreferenceSet()
        {
            using (var httpRequest = new HttpSimulator().SimulateRequest())
            {
                //arrange
                var context = new Mock<ISubtextContext>();
                var httpContext = new Mock<HttpContextBase>();
                httpContext.Setup(c => c.Request.ApplicationPath).Returns("path");
                context.Setup(c => c.UrlHelper).Returns(
                    new BlogUrlHelper(new RequestContext(httpContext.Object, new RouteData()), null));
                context.Setup(c => c.Blog).Returns(new Blog { Host = "host" });
                var page = new SubtextPage { SubtextContext = context.Object };
                var editor = new RichTextEditor { Page = page };
                //set use plain text in user preferences
                Preferences.UsePlainHtmlEditor = true;

                //act
                editor.InitControls(new EventArgs());

                //post
                var provider = editor.Provider;
                Assert.IsTrue(provider is PlainTextBlogEntryEditorProvider, "PlainTextBlogEntryEditorProvider is created if it is selected in user preferences.");
            }

        }
    }
}

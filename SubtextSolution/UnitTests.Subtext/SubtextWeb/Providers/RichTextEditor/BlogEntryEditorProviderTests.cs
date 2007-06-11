using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.UI;
using Subtext.Providers.BlogEntryEditor.FCKeditor;
using Subtext.Web.Providers.BlogEntryEditor.FTB;
using Subtext.Web.Providers.BlogEntryEditor.PlainText;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
	[TypeFixture(typeof(BlogEntryEditorProvider))]
	[ProviderFactory(typeof(BlogEntryEditorProviderFactory), typeof(BlogEntryEditorProvider))]
	public class BlogEntryEditorProviderTests
	{
		[Test]
		public void CanSetControlId(BlogEntryEditorProvider provider)
		{
			string test = "MyTestControlID";
			provider.ControlId = test;
			Assert.AreEqual(test, provider.ControlId);
		}

		[Test]
		[RollBack2]
		public void CanSetText(BlogEntryEditorProvider provider)
		{
			UnitTestHelper.SetupBlog();

			string test = "Lorem Ipsum";
			provider.InitializeControl();
			provider.Text = test;
			Assert.AreEqual(test, provider.Text);
			Assert.AreEqual(test, provider.Xhtml);
		}

		[Test]
		[RollBack2]
		public void SetWidth(BlogEntryEditorProvider provider)
		{
			UnitTestHelper.SetupBlog();

			Unit test = 200;
			provider.InitializeControl();
			provider.Width = test;
			Assert.AreEqual(test, provider.Width);
		}

		[Test]
		[RollBack2]
		public void SetHeight(BlogEntryEditorProvider provider)
		{
			UnitTestHelper.SetupBlog();

			Unit test = 100;
			provider.InitializeControl();
			provider.Height = test;
			Assert.AreEqual(test, provider.Height);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullName(BlogEntryEditorProvider provider)
		{
			provider.Initialize(null, new NameValueCollection());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullConfigValue(BlogEntryEditorProvider provider)
		{
			provider.Initialize("FCKProvider", null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInitializationWithEmptyWebFolder(BlogEntryEditorProvider provider)
		{
			provider.Initialize("FCKProvider", new NameValueCollection());
		}
	}

	internal class BlogEntryEditorProviderFactory
	{
		[Factory]
		public FckBlogEntryEditorProvider FckBlogEntryEditorProvider
		{
			get
			{
				FckBlogEntryEditorProvider provider = new FckBlogEntryEditorProvider();
				NameValueCollection configValues = new NameValueCollection();
				configValues.Add("WebFormFolder", "~/Providers/BlogEntryEditor/FCKeditor/");
				configValues.Add("ImageBrowserURL", "/imagebrowser/default.aspx");
				configValues.Add("LinkBrowserURL", "/LinkBrowser/default.aspx");
				configValues.Add("ImageConnectorURL", "/ImageConnector/default.aspx");
				configValues.Add("LinkConnectorURL", "/LinkConnector/default.aspx");
				configValues.Add("FileAllowedExtensions", ".htm, .txt");
				configValues.Add("ImageAllowedExtensions", ".gif, .jpg");
				configValues.Add("ToolbarSet", "SubText");
				configValues.Add("Skin", "office2003");
				configValues.Add("RemoveServerNamefromUrls", "false");
				provider.Initialize("FCKProvider", configValues);
				return provider;
			}
		}

		[Factory]
		public FtbBlogEntryEditorProvider FtbBlogEntryEditorProvider
		{
			get
			{
				FtbBlogEntryEditorProvider provider = new FtbBlogEntryEditorProvider();
				NameValueCollection configValues = new NameValueCollection(3);
				configValues.Add("WebFormFolder", "~/Providers/RichTextEditor/FTB/");
				configValues.Add("toolbarlayout", "Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|InsertTable|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,Insert,InsertRule|Cut,Copy,Paste;Undo,Redo|ieSpellCheck,WordClean|InsertImage,InsertImageFromGallery");
				configValues.Add("FormatHtmlTagsToXhtml", "true");
				configValues.Add("RemoveServerNamefromUrls", "false");
				provider.Initialize("FTBProvider", configValues);
				return provider;
			}
		}

		[Factory]
		public PlainTextBlogEntryEditorProvider PlainTextBlogEntryEditorProvider
		{
			get
			{
				PlainTextBlogEntryEditorProvider provider = new PlainTextBlogEntryEditorProvider();
				NameValueCollection configValues = new NameValueCollection(3);
				configValues.Add("rows", "10");
				configValues.Add("cols", "70");
				configValues.Add("cssClass", "myCssClass");
				provider.Initialize("PlainTextProvider", configValues);
				return provider;
			}
		}
	}

	[TestFixture]
	public class OtherBlogEntryProviderTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void InitializeThrowsArgumentNullExceptionIfNameNull()
		{
			FakeTextBlogEntryEditorProvider provider = new FakeTextBlogEntryEditorProvider();
			provider.Initialize(null, new NameValueCollection());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InitializeThrowsArgumentNullExceptionIfCollectionNull()
		{
			FakeTextBlogEntryEditorProvider provider = new FakeTextBlogEntryEditorProvider();
			provider.Initialize("Name", null);
		}

		[Test]
		public void InitializeCanSetWidthAndHeight()
		{
			FakeTextBlogEntryEditorProvider provider = new FakeTextBlogEntryEditorProvider();
			NameValueCollection config = new NameValueCollection();
			config.Add("Width", "109");
			config.Add("Height", "231");
			provider.Initialize("Name", config);
			Assert.AreEqual(new Unit(109), provider.Width);
			Assert.AreEqual(new Unit(231), provider.Height);
		}

		[Test]
		public void CanGetProviderAndProviders()
		{
			Assert.IsNotNull(BlogEntryEditor.Provider);
			Assert.AreEqual(3, BlogEntryEditor.Providers.Count);
		}

		[Test]
		public void CanParseUnit()
		{
			Assert.AreEqual(Unit.Empty, FakeTextBlogEntryEditorProvider.ParseTheUnit(null));
			Assert.AreEqual(Unit.Empty, FakeTextBlogEntryEditorProvider.ParseTheUnit("as234s23h4s!#$#"));
			Assert.AreEqual(Unit.Empty, FakeTextBlogEntryEditorProvider.ParseTheUnit(""));
			Assert.AreEqual(new Unit(1), FakeTextBlogEntryEditorProvider.ParseTheUnit("1"));
		}
	}

	internal class FakeTextBlogEntryEditorProvider : BlogEntryEditorProvider
	{
		private string text;

		public static Unit ParseTheUnit(string s)
		{
			return ParseUnit(s);
		}

		/// <summary>
		/// The content of the area
		/// </summary>
		public override string Text
		{
			get { return this.text; }
			set { this.text = value; }
		}

		/// <summary>
		/// The content of the area, but XHTML converted
		/// </summary>
		public override string Xhtml
		{
			get { return null; }
		}

		/// <summary>
		/// Return the RichTextEditorControl to be displayed inside the page
		/// </summary>
		public override Control RichTextEditorControl
		{
			get { return null; }
		}

		/// <summary>
		/// Initializes the Control to be displayed
		/// </summary>
		public override void InitializeControl()
		{
			throw new NotImplementedException();
		}
	}
}

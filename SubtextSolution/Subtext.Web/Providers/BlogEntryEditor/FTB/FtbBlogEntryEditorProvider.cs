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
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Web;

using FreeTextBoxControls;

namespace Subtext.Web.Providers.BlogEntryEditor.FTB
{
	/// <summary>
	/// Summary description for FtbRichTextEditor.
	/// </summary>
    public class FtbBlogEntryEditorProvider : BlogEntryEditorProvider
	{
		FreeTextBox _ftbCtl; //There's a good reason to do this early.
		string _webFormFolder = string.Empty;
		string _toolbarlayout = string.Empty;
		bool _formatHtmlTagsToXhtml;
		bool _removeServerNamefromUrls;

        private static System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Subtext.Web.Providers.BlogEntryEditor.FTB.resources.ErrorMessages", System.Reflection.Assembly.GetExecutingAssembly());

		public override Control RichTextEditorControl
		{
			get
			{
				return _ftbCtl;
			}
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{
			if(name == null)
				throw new ArgumentNullException("name", rm.GetString("nameNeeded"));
			
			if(configValue == null)
				throw new ArgumentNullException("configValue", rm.GetString("configNeeded"));
			
			if(configValue["WebFormFolder"]!=null)
				_webFormFolder=configValue["WebFormFolder"];
			else
				throw new InvalidOperationException(rm.GetString("WebFormFolderNeeded"));

			if(configValue["toolbarlayout"]!=null)
				_toolbarlayout=configValue["toolbarlayout"];
			if(configValue["FormatHtmlTagsToXhtml"]!=null)
				_formatHtmlTagsToXhtml=Boolean.Parse(configValue["FormatHtmlTagsToXhtml"]);
			if(configValue["RemoveServerNamefromUrls"]!=null)
				_removeServerNamefromUrls=Boolean.Parse(configValue["RemoveServerNamefromUrls"]);

			base.Initialize(name, configValue);
		}

		public override void InitializeControl() 
		{
			_ftbCtl = new FreeTextBox();
			_ftbCtl.ID = this.ControlId;
			
			if (this.Width != Unit.Empty)
				_ftbCtl.Width = this.Width;

			if (this.Height != Unit.Empty)
				_ftbCtl.Height = this.Height;
			
			if(_toolbarlayout!=null && _toolbarlayout.Trim().Length!=0)
				_ftbCtl.ToolbarLayout=_toolbarlayout;
			_ftbCtl.FormatHtmlTagsToXhtml=_formatHtmlTagsToXhtml;
			_ftbCtl.RemoveServerNameFromUrls=_removeServerNamefromUrls;

			if(_webFormFolder!=null && _webFormFolder.Length!=0)
				_ftbCtl.ImageGalleryUrl=HttpHelper.ExpandTildePath(_webFormFolder+"ftb.imagegallery.aspx?rif={0}&cif={0}");

			string blogImageRootPath=Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);

			_ftbCtl.ImageGalleryPath=blogImageRootPath;
		}

		public override String Text
		{
			get
			{
				return _ftbCtl.Text;
			}
			set
			{
				_ftbCtl.Text = value;
			}
		}

		public override String Xhtml
		{
			get
			{
				return _ftbCtl.Xhtml;
			}
		}
	}
}

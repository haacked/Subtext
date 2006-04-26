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
using System.IO;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using System.Web;
using Subtext.Web.Controls;

using FreeTextBoxControls;

namespace Subtext.Web.Providers.RichTextEditor.FTB
{
	/// <summary>
	/// Summary description for FtbRichTextEditor.
	/// </summary>
	public class FtbRichTextEditorProvider: RichTextEditorProvider
	{
		FreeTextBox _ftbCtl;
		string _controlID=string.Empty;
		string _name = string.Empty;

		string _webFormFolder=string.Empty;
		string _toolbarlayout=string.Empty;
		bool _formatHtmlTagsToXhtml=false;
		bool _removeServerNamefromUrls=false;


		public override Control RichTextEditorControl
		{
			get
			{
				return _ftbCtl;
			}
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{
			_name=name;
			if(configValue["WebFormFolder"]!=null)
				_webFormFolder=configValue["WebFormFolder"];
			else
				throw new ApplicationException("WebFormFolder must be specified for the FreeTextBox provider to work");
			if(configValue["toolbarlayout"]!=null)
				_toolbarlayout=configValue["toolbarlayout"];
			if(configValue["FormatHtmlTagsToXhtml"]!=null)
				_formatHtmlTagsToXhtml=Boolean.Parse(configValue["FormatHtmlTagsToXhtml"]);
			if(configValue["RemoveServerNamefromUrls"]!=null)
				_removeServerNamefromUrls=Boolean.Parse(configValue["RemoveServerNamefromUrls"]);

		}

		public override void InitializeControl() 
		{
			_ftbCtl=new FreeTextBox();
			_ftbCtl.ID=ControlID;
			if(!_toolbarlayout.Trim().Equals(""))
				_ftbCtl.ToolbarLayout=_toolbarlayout;
			_ftbCtl.FormatHtmlTagsToXhtml=_formatHtmlTagsToXhtml;
			_ftbCtl.RemoveServerNameFromUrls=_removeServerNamefromUrls;

			if(!_webFormFolder.Equals(""))
				_ftbCtl.ImageGalleryUrl=ControlHelper.ExpandTildePath(_webFormFolder+"/ftb.imagegallery.aspx?rif={0}&cif={0}");

			string blogImageRootPath=Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);

			if(!Directory.Exists(HttpContext.Current.Server.MapPath(blogImageRootPath)))
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath(blogImageRootPath));

			_ftbCtl.ImageGalleryPath=blogImageRootPath;
		}

		public override string ControlID
		{
			get
			{
				return _controlID;
			}
			set
			{
				_controlID=value;
			}
		}

		public override Unit Height
		{
			get
			{
				return _ftbCtl.Height;
			}
			set
			{
				_ftbCtl.Height=value;
			}
		}

		public override Unit Width
		{
			get
			{
				return _ftbCtl.Width;
			}
			set
			{
				_ftbCtl.Width=value;
			}
		}

		public override String Text
		{
			get
			{
				return _ftbCtl.Text;
			}
			set
			{
				_ftbCtl.Text=value;
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

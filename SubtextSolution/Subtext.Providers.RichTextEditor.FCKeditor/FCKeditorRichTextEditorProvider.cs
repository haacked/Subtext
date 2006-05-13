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
using System.IO;
using Subtext.Extensibility.Providers;
using System.Web;
using Subtext.Web.Controls;

namespace Subtext.Providers.RichTextEditor.FCKeditor
{
	/// <summary>
	/// Summary description for FCKeditorRichTextEditorProvider.
	/// </summary>
	public class FCKeditorRichTextEditorProvider : RichTextEditorProvider
	{

		FredCK.FCKeditorV2.FCKeditor _fckCtl;
		string _controlID=string.Empty;
		string _webFormFolder=string.Empty;
		string _imageBrowserURL=string.Empty;
		string _linkBrowserURL=string.Empty;
		string _imageConnectorURL=string.Empty;
		string _linkConnectorURL=string.Empty;
		string _toolbarSet=string.Empty;
		string _skin=string.Empty;
		static string _fileAllowedExtensions=string.Empty;
		static string _imageAllowedExtensions=string.Empty;

		public override System.Web.UI.Control RichTextEditorControl
		{
			get
			{
				return _fckCtl;
			}
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

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{
			if(name == null)
				throw new ArgumentNullException("name", "Cannot initialize a provider with a null name.");
			
			if(configValue == null)
				throw new ArgumentNullException("configValue", "Cannot initialize a provider with a null configValue.");
			
			if(configValue["WebFormFolder"]!=null)
				_webFormFolder=configValue["WebFormFolder"];
			else
				throw new InvalidOperationException("WebFormFolder must be specified for the FCKeditor provider to work");

			if(configValue["ImageBrowserURL"]!=null)
				_imageBrowserURL=configValue["ImageBrowserURL"];
			else
				throw new InvalidOperationException("ImageBrowserURL must be specified for the FCKeditor provider to work");

			if(configValue["LinkBrowserURL"]!=null)
				_linkBrowserURL=configValue["LinkBrowserURL"];
			else
				throw new InvalidOperationException("LinkBrowserURL must be specified for the FCKeditor provider to work");

			if(configValue["ImageConnectorURL"]!=null)
				_imageConnectorURL=configValue["ImageConnectorURL"];
			else
				throw new InvalidOperationException("ImageConnectorURL must be specified for the FCKeditor provider to work");

			if(configValue["LinkConnectorURL"]!=null)
				_linkConnectorURL=configValue["LinkConnectorURL"];
			else
				throw new InvalidOperationException("LinkConnectorURL must be specified for the FCKeditor provider to work");


			if(configValue["FileAllowedExtensions"]!=null)
				_fileAllowedExtensions=configValue["FileAllowedExtensions"];
			else
				throw new InvalidOperationException("FileAllowedExtensions must be specified for the FCKeditor provider to work");


			if(configValue["ImageAllowedExtensions"]!=null)
				_imageAllowedExtensions=configValue["ImageAllowedExtensions"];
			else
				throw new InvalidOperationException("ImageAllowedExtensions must be specified for the FCKeditor provider to work");


			if(configValue["ToolbarSet"]!=null)
				_toolbarSet=configValue["ToolbarSet"];

			if(configValue["Skin"]!=null)
				_skin=configValue["Skin"];

			base.Initialize(name, configValue);
		}

		public override void InitializeControl()
		{
			_fckCtl=new FredCK.FCKeditorV2.FCKeditor();
			_fckCtl.ID=ControlID;
			_fckCtl.BasePath=ControlHelper.ExpandTildePath(_webFormFolder);
			if(_toolbarSet.Length!=0)
				_fckCtl.ToolbarSet=_toolbarSet;

			if(_skin.Length!=0)
				_fckCtl.SkinPath=_fckCtl.BasePath+"editor/skins/"+_skin+"/";

			// Compute user image gallery url
			string blogImageRootPath=Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);
			string currentImageConnector=_imageConnectorURL.Replace("~/","~/"+Subtext.Framework.Configuration.Config.CurrentBlog.Subfolder+"/");
			string currentLinkConnector=_linkConnectorURL.Replace("~/","~/"+Subtext.Framework.Configuration.Config.CurrentBlog.Subfolder+"/");

			if(!Directory.Exists(HttpContext.Current.Server.MapPath(blogImageRootPath)))
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath(blogImageRootPath));

			_fckCtl.ImageBrowserURL=String.Format(ControlHelper.ExpandTildePath(_imageBrowserURL),ControlHelper.ExpandTildePath(currentImageConnector));
			_fckCtl.LinkBrowserURL=String.Format(ControlHelper.ExpandTildePath(_linkBrowserURL),ControlHelper.ExpandTildePath(currentLinkConnector));
			

		}

		public override System.Web.UI.WebControls.Unit Height
		{
			get
			{
				return _fckCtl.Height;
			}
			set
			{
				_fckCtl.Height=value;
			}
		}

		public override string Text
		{
			get
			{
				return _fckCtl.Value;
			}
			set
			{
				_fckCtl.Value=value;
			}
		}

		public override System.Web.UI.WebControls.Unit Width
		{
			get
			{
				return _fckCtl.Width;
			}
			set
			{
				_fckCtl.Width=value;
			}
		}

		public override string Xhtml
		{
			get
			{
				return this.Text;
			}
		}

		public static string ImageAllowedExtensions 
		{
			get 
			{
				return _imageAllowedExtensions;
			}
		}

		public static string FileAllowedExtensions 
		{
			get 
			{
				return _fileAllowedExtensions;
			}
		}

	}
}

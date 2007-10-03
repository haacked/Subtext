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
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Web;
using Subtext.Web.Controls;
using System.Collections.Specialized;
using Subtext.Providers.BlogEntryEditor.FCKeditor.Properties;
using System.Globalization;

namespace Subtext.Providers.BlogEntryEditor.FCKeditor
{
	/// <summary>
	/// Summary description for FCKeditorRichTextEditorProvider.
	/// </summary>
	public sealed class FckBlogEntryEditorProvider : BlogEntryEditorProvider, IDisposable
	{
		FredCK.FCKeditorV2.FCKeditor _fckCtl = new FredCK.FCKeditorV2.FCKeditor(); //There's a good reason to do this early.
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

		public override void Initialize(string name, NameValueCollection config)
		{
			if(name == null)
				throw new ArgumentNullException("name", Resources.nameNeeded);
			
			if(config == null)
				throw new ArgumentNullException("configValue", Resources.configNeeded);

			if(config["WebFormFolder"]!=null)
				_webFormFolder=config["WebFormFolder"];
			else
				throw new InvalidOperationException(Resources.WebFormFolderNeeded);

			if(config["ImageBrowserURL"]!=null)
				_imageBrowserURL=config["ImageBrowserURL"];
			else
				throw new InvalidOperationException(Resources.ImageBrowserURLNeeded);

			if(config["LinkBrowserURL"]!=null)
				_linkBrowserURL=config["LinkBrowserURL"];
			else
				throw new InvalidOperationException(Resources.LinkBrowserURLNeeded);

			if(config["ImageConnectorURL"]!=null)
				_imageConnectorURL=config["ImageConnectorURL"];
			else
				throw new InvalidOperationException(Resources.ImageConnectorURLNeeded);

			if(config["LinkConnectorURL"]!=null)
				_linkConnectorURL=config["LinkConnectorURL"];
			else
				throw new InvalidOperationException(Resources.LinkConnectorURLNeeded);


			if(config["FileAllowedExtensions"]!=null)
				_fileAllowedExtensions=config["FileAllowedExtensions"];
			else
				throw new InvalidOperationException(Resources.FileAllowedExtensionsNeeded);


			if(config["ImageAllowedExtensions"]!=null)
				_imageAllowedExtensions=config["ImageAllowedExtensions"];
			else
				throw new InvalidOperationException(Resources.ImageAllowedExtensionsNeeded);

			if(config["ToolbarSet"]!=null)
				_toolbarSet=config["ToolbarSet"];

			if(config["Skin"]!=null)
				_skin=config["Skin"];
			
			base.Initialize(name, config);
		}

		public override void InitializeControl()
		{
			_fckCtl = new FredCK.FCKeditorV2.FCKeditor();
			_fckCtl.ID = this.ControlId;
			_fckCtl.BasePath = HttpHelper.ExpandTildePath(_webFormFolder);

			if(this.Width != Unit.Empty)
				_fckCtl.Width = this.Width;

			if (this.Height != Unit.Empty)
				_fckCtl.Height = this.Height;
			
			if(_toolbarSet.Length!=0)
				_fckCtl.ToolbarSet = _toolbarSet;

			if(_skin.Length != 0)
				_fckCtl.SkinPath = _fckCtl.BasePath + "editor/skins/" + _skin + "/";

			// Compute user image gallery url
			string blogSubFolder = Subtext.Framework.Configuration.Config.CurrentBlog.Subfolder;
			string currentImageConnector = _imageConnectorURL;
			string currentLinkConnector = _linkConnectorURL;
			if (blogSubFolder.Length > 0)
			{
				currentImageConnector = _imageConnectorURL.Replace("~/", "~/" + blogSubFolder + "/");
				currentLinkConnector = _linkConnectorURL.Replace("~/", "~/" + blogSubFolder + "/");
			}

			_fckCtl.ImageBrowserURL = String.Format(CultureInfo.InvariantCulture, ControlHelper.ExpandTildePath(_imageBrowserURL), ControlHelper.ExpandTildePath(currentImageConnector));
			_fckCtl.LinkBrowserURL = String.Format(CultureInfo.InvariantCulture, ControlHelper.ExpandTildePath(_linkBrowserURL), ControlHelper.ExpandTildePath(currentLinkConnector));
			}

		public override string Text
		{
			get
			{
				return _fckCtl.Value;
			}
			set
			{
				_fckCtl.Value = value;
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

		public void Dispose()
		{
			_fckCtl.Dispose();
		}

	}
}

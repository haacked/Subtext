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
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Web.Controls;

using FredCK.FCKeditorV2;

namespace Subtext.Providers.RichTextEditor.FCKeditor
{
	/// <summary>
	/// Summary description for FCKeditorRichTextEditorProvider.
	/// </summary>
	public class FCKeditorRichTextEditorProvider: RichTextEditorProvider
	{

		FredCK.FCKeditorV2.FCKeditor _fckCtl;
		string _controlID=string.Empty;
		string _name = string.Empty;

		string _webFormFolder=string.Empty;

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
			_name=name;
			
			if(configValue["WebFormFolder"]!=null)
				_webFormFolder=configValue["WebFormFolder"];
			else
				throw new ApplicationException("WebFormFolder must be specified for the FCKeditor provider to work");
		}

		public override void InitializeControl()
		{
			_fckCtl=new FredCK.FCKeditorV2.FCKeditor();
			_fckCtl.ID=ControlID;
			_fckCtl.BasePath=ControlHelper.ExpandTildePath(_webFormFolder);

			// Compute user image gallery url
			string blogImageRootPath=Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);

			if(!Directory.Exists(HttpContext.Current.Server.MapPath(blogImageRootPath)))
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath(blogImageRootPath));

			System.Web.HttpContext.Current.Application["FCKeditor:UserFilesPath"]=blogImageRootPath;

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

	}
}

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

namespace Subtext.Web.Providers.RichTextEditor.PlainText
{
	/// <summary>
	/// Summary description for PlainTextRichTextEditorProvider.
	/// </summary>
	public class PlainTextRichTextEditorProvider : RichTextEditorProvider
	{
		TextBox _txtCtl;
		string _controlID=string.Empty;
		int _rows;
		int _cols;
		string _cssClass=String.Empty;

		private static System.Resources.ResourceManager rm =  new System.Resources.ResourceManager("Subtext.Web.Providers.RichTextEditor.PlainText.resources.ErrorMessages",System.Reflection.Assembly.GetExecutingAssembly());


		public override Control RichTextEditorControl
		{
			get
			{
				return _txtCtl;
			}
		}

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{

			if(name == null)
				throw new ArgumentNullException("name", rm.GetString("nameNeeded"));
			
			if(configValue == null)
				throw new ArgumentNullException("configValue", rm.GetString("configNeeded"));

			if(configValue["rows"]!=null)
				_rows=Convert.ToInt32(configValue["rows"]);
			if(configValue["cols"]!=null)
				_cols=Convert.ToInt32(configValue["cols"]);
			if(configValue["cssClass"]!=null)
				_cssClass=configValue["cssClass"];
			
			base.Initialize(name, configValue);
		}

		public override void InitializeControl() 
		{
			_txtCtl=new TextBox();
			_txtCtl.ID=ControlID;
			if(_cssClass!=null && _cssClass.Trim().Length!=0)
				_txtCtl.CssClass=_cssClass;
			_txtCtl.TextMode=TextBoxMode.MultiLine;
			_txtCtl.Rows=_rows;
			_txtCtl.Columns=_cols;
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
				return _txtCtl.Height;
			}
			set
			{
				_txtCtl.Height=value;
			}
		}

		public override Unit Width
		{
			get
			{
				return _txtCtl.Width;
			}
			set
			{
				_txtCtl.Width=value;
			}
		}

		public override String Text
		{
			get
			{
				return _txtCtl.Text;
			}
			set
			{
				_txtCtl.Text=value;
			}
		}

		public override String Xhtml
		{
			get
			{
				return _txtCtl.Text;
			}
		}
	}
}

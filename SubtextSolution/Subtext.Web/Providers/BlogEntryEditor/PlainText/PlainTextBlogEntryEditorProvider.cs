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

namespace Subtext.Web.Providers.BlogEntryEditor.PlainText
{
	/// <summary>
	/// Summary description for PlainTextRichTextEditorProvider.
	/// </summary>
    public class PlainTextBlogEntryEditorProvider : BlogEntryEditorProvider
	{
		TextBox _txtCtl;
		int _rows;
		int _cols;
		string _cssClass;

        private static System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Subtext.Web.Providers.BlogEntryEditor.PlainText.resources.ErrorMessages", System.Reflection.Assembly.GetExecutingAssembly());

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
				_cssClass = configValue["cssClass"];
			
			base.Initialize(name, configValue);
		}

		public override void InitializeControl() 
		{
			_txtCtl = new TextBox();
			_txtCtl.ID = this.ControlId;
			if(_cssClass != null && _cssClass.Trim().Length != 0)
				_txtCtl.CssClass = _cssClass;

			if (this.Width != Unit.Empty)
				_txtCtl.Width = this.Width;

			if (this.Height != Unit.Empty)
				_txtCtl.Height = this.Height;
			
			_txtCtl.TextMode=TextBoxMode.MultiLine;
			_txtCtl.Rows = _rows;
			_txtCtl.Columns = _cols;
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

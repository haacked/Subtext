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
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Summary description for RichTextEditorCtl.
	/// </summary>
	[ValidationProperty("Text")]
	public class RichTextEditor: WebControl, INamingContainer
	{
		private Control editor;
		private RichTextEditorProvider provider; 

		
		public string Text 
		{
			get 
			{
				return provider.Text;
			}
			set 
			{
				provider.Text=value;
			}
		}

		public string Xhtml 
		{
			get 
			{
				return provider.Xhtml;
			}
		}

		protected override void OnInit(EventArgs e)
		{	
			provider=RichTextEditorProvider.Instance();
			provider.ControlID=this.ID;
			provider.InitializeControl();
			editor=provider.RichTextEditorControl;
			this.Controls.Add(editor);
			base.OnInit (e);
		}
	}
}

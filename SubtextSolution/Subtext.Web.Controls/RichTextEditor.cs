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

		#region EventHandlers
		public delegate void ErrorHandler(object sender, RichTextEditorErrorEventArgs e);
		public event ErrorHandler Error;

		public void OnError(Exception ex) 
		{
			if(Error!=null) 
			{
				Error(this,new RichTextEditorErrorEventArgs(ex));
			}
		}

		public class RichTextEditorErrorEventArgs:EventArgs 
		{
			private Exception _ex;
			public RichTextEditorErrorEventArgs( Exception ex) 
			{
				_ex=ex;
			}

			public Exception Ex 
			{
				get 
				{
					return _ex;
				}
				set 
				{
					_ex=value;
				}
			}
		}
		#endregion

		private Control editor;
		private RichTextEditorProvider provider; 

		private Unit _height=Unit.Empty;
		private Unit _width=Unit.Empty;
		
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

		public override Unit Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height=value;
			}
		}

		public override Unit Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
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
			try 
			{
				provider=RichTextEditorProvider.Instance();
				provider.ControlID=this.ID;
				provider.InitializeControl();
				if(_height!=Unit.Empty)
					provider.Height=_height;
				if(_width!=Unit.Empty)
					provider.Width=_width;
				editor=provider.RichTextEditorControl;
				this.Controls.Add(editor);
				base.OnInit (e);
			}
			catch (Exception ex) 
			{
				OnError(ex);
			}
		}
	}
}

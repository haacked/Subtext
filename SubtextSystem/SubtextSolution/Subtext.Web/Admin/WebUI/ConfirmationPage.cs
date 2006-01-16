using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Will cause the user to confirm navigating away from the current page. This behavior can be overriden.
	/// </summary>
	public class ConfirmationPage : AdminPage
	{
		private bool _IsInEdit = false;
		public bool IsInEdit
		{
			get {return this._IsInEdit;}
			set {this._IsInEdit = value;}
		}

		private string _message = "You will lose any non-saved text";
		public string Message
		{
			get{return _message;}
			set{_message = value;}
		}

		protected override void OnPreRender(EventArgs e)
		{
			//If we are in edit mode, register the script
			if(IsInEdit)
			{
				Page.RegisterClientScriptBlock("ConfirmationBeforeLeaving",string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}{2}",scriptStart,Message,scriptEnd));
			}
			base.OnPreRender (e);
		}



		const string scriptStart = "<script language=\"javascript\" type=\"javascript\">g_blnCheckUnload = true;function RunOnBeforeUnload() {if (g_blnCheckUnload) {window.event.returnValue = '";
		const string scriptEnd = "';    }  }  function bypassCheck() {     g_blnCheckUnload  = false;   }</script>";

		public static readonly string ByPassFuncationName = "bypassCheck()";


		protected override void Render(HtmlTextWriter writer)
		{

			//If we are in edit mode, wire up the onbeforeunload event
			if(IsInEdit)
			{
				TextWriter tempWriter = new StringWriter();
				base.Render(new HtmlTextWriter(tempWriter));
				writer.Write(Regex.Replace(tempWriter.ToString(),"<body","<body onbeforeunload=\"RunOnBeforeUnload()\"",RegexOptions.IgnoreCase));
			}
			else
			{
				base.Render(writer);
			}
		}

		

		
	}
}

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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// This is used to render a client side script tag using the 
	/// the ~/ syntax for the script path.
	/// </summary>
	public class ScriptTag : HtmlControl
	{
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/>
		/// event. Sets the <see cref="Control.EnableViewState"/> property to false.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			this.EnableViewState = false;
			base.OnInit (e);
		}

		/// <summary>
		/// Renders this script tag.
		/// </summary>
		/// <param name="writer">Writer.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			string format = @"<script src=""{0}"" type=""text/{1}""{2}></script>";

			string idText = string.Empty;
			if(IsAttributeDefined("id"))
				idText = " id=\"" + Attributes["id"] + "\"";


			writer.Write(string.Format(CultureInfo.InvariantCulture, format, Src, Language, idText));
		}

		/// <summary>
		/// Gets or sets the SRC, the path to the script file.
		/// </summary>
		/// <value></value>
		public string Src
		{
			get
			{
				if(IsAttributeDefined("src"))
					return ConvertToAppPath(Attributes["src"]);
				else
					return string.Empty;
			}
			set
			{
				if(String.IsNullOrEmpty(value))
					value = null;

				Attributes["src"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the language of the script.
		/// </summary>
		/// <value></value>
		public string Language
		{
			get
			{
				if(IsAttributeDefined("language"))
					return Attributes["language"];
				else
					return "javascript";
			}
			set
			{
				if (String.IsNullOrEmpty(value))
					value = null;

				Attributes["language"] = value;
			}
		}

		static string ConvertToAppPath(string path)
		{
			return ControlHelper.ExpandTildePath(path);
		}

		/// <summary>
		/// Gets the name of the tag.
		/// </summary>
		/// <value></value>
		public override string TagName
		{
			get
			{
				return "script";
			}
		}

		bool IsAttributeDefined(string name)
		{
			return ControlHelper.IsAttributeDefined(this, name);
		}
	}
}

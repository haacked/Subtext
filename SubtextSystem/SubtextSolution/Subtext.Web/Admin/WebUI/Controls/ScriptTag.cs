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

namespace Subtext.Web.Admin.WebUI
{
	/// <summary>
	/// Renders a script tag.
	/// </summary>
	public class ScriptTag : Control
	{
		/// <summary>
		/// Renders a &lt;script&gt; tag.
		/// </summary>
		/// <param name="output">Output.</param>
		protected override void Render(HtmlTextWriter output)
		{	
			output.Write
			(
				string.Format(System.Globalization.CultureInfo.InvariantCulture, "<script src=\"{0}\" type=\"{1}\"></script>", 
				_src, _type));
		}

		/// <summary>
		/// Gets or sets the SRC of the script.
		/// </summary>
		/// <value></value>
		public string Src
		{
			get { return _src; }
			set { _src = value; }
		}

		string _src;

		/// <summary>
		/// Gets or sets the type of the script.
		/// </summary>
		/// <value></value>
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		string _type;
	}
}

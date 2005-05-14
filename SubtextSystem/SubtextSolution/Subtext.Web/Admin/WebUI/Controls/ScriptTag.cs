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
				String.Format("<script src=\"{0}\" type=\"{1}\"></script>", 
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

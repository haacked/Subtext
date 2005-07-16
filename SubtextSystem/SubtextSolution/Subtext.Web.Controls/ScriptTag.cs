using System;
using System.Globalization;
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
		/// Creates a new <see cref="ScriptTag"/> instance.
		/// </summary>
		public ScriptTag() : base()
		{
			this.EnableViewState = false;
		}

		/// <summary>
		/// Renders this script tag.
		/// </summary>
		/// <param name="writer">Writer.</param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string format = "<script src=\"{0}\" language=\"{1}\"{2}></script>";	

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
				if(value.Length == 0)
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
					return string.Empty;
			}
			set
			{
				if(value.Length == 0)
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

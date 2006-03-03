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
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// The standard button control emits the language attribute which is not 
	/// XHTML compliant.  This is a button to use if you wish to remain XHTML compliant. 
	/// </summary>
	public class CompliantButton : Button
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompliantButton"/> class.
		/// </summary>
		public CompliantButton() : base()
		{
		}

		/// <summary>
		/// Basically a reimplementation of the base 
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			CompliantHtmlTextWriter compliantWriter = new CompliantHtmlTextWriter(writer);
			base.Render(compliantWriter);
		}

		private class CompliantHtmlTextWriter : HtmlTextWriter
		{
			internal CompliantHtmlTextWriter(HtmlTextWriter writer) : base(writer)
			{
			}

			/// <summary>
			/// Ignores the language attribute for the purposes of a submit button.
			/// <see langword="HtmlTextWriter"/> output stream.
			/// </summary>
			/// <param name="name">The HTML attribute to add.</param>
			/// <param name="value"></param>
			public override void AddAttribute(string name, string value)
			{
				if(String.Compare(name, "language", true, CultureInfo.InvariantCulture) == 0)
					return;
				base.AddAttribute(name, value);
			}

			/// <summary>
			/// Ignores the language attribute for the purposes of a submit button. 
			/// </summary>
			/// <remarks>
			/// I don't technically need to override this method, but it should make this 
			/// more version proof.
			/// </remarks>
			/// <param name="name">The HTML attribute to add.</param>
			/// <param name="value"></param>
			/// <param name="fEndode"></param>
			public override void AddAttribute(string name, string value, bool fEndode)
			{
				if(String.Compare(name, "language", true, CultureInfo.InvariantCulture) == 0)
					return;
				base.AddAttribute (name, value, fEndode);
			}

		}
	}
}

using System;
using System.Globalization;
using Subtext.Framework.Configuration;

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

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Footer control, displayed at the bottom of most skins. 
	///	Contains a <see cref="System.Web.UI.WebControls.Literal"/> 
	///	control which displays the author name.
	/// </summary>
	public  class Footer : BaseControl
	{
		protected System.Web.UI.WebControls.Literal FooterText;
        protected System.Web.UI.WebControls.Literal currentYear;

		
		/// <summary>
		/// Sets the FooterText.Text property to the CurrentBlog.Author
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
            if (FooterText != null)
            {
                FooterText.Text = Blog.Author;
            }
		    
		    if(currentYear != null)
		    {
				currentYear.Text = Config.CurrentBlog.TimeZone.Now.Year.ToString(CultureInfo.InvariantCulture);
		    }
		}
	}
}


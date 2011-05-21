#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Footer control, displayed at the bottom of most skins. 
    ///	Contains a <see cref="System.Web.UI.WebControls.Literal"/> 
    ///	control which displays the author name.
    /// </summary>
    public class Footer : BaseControl
    {
        protected Literal currentYear;
        protected Literal FooterText;


        /// <summary>
        /// Sets the FooterText.Text property to the CurrentBlog.Author
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (FooterText != null)
            {
                FooterText.Text = Blog.Author;
            }

            if (currentYear != null)
            {
                currentYear.Text = Blog.TimeZone.Now.Year.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
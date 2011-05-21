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
using System.IO;
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
        /// Basically a reimplementation of the base 
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            var compliantWriter = new CompliantHtmlTextWriter(writer);
            base.Render(compliantWriter);
        }

        private class CompliantHtmlTextWriter : HtmlTextWriter
        {
            internal CompliantHtmlTextWriter(TextWriter writer)
                : base(writer)
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
                if (String.Equals(name, "language", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
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
                if (String.Equals(name, "language", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                base.AddAttribute(name, value, fEndode);
            }
        }
    }
}
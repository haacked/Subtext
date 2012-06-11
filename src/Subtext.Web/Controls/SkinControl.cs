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
using System.Web.UI;
using Subtext.Framework.UI.Skinning;

namespace Subtext.Web.Controls
{
    public class SkinControl : Control
    {
        protected override void OnLoad(EventArgs e)
        {
            var parent = Parent as ISkinControlContainer;
            if (parent != null)
            {
                SkinUserControl = parent.SkinControlLoader.LoadControl(ControlName);
                this.Controls.AddAt(0, SkinUserControl);
            }
            base.OnLoad(e);
        }

        public Control SkinUserControl { get; set; }

        public string ControlName { get; set; }
    }
}

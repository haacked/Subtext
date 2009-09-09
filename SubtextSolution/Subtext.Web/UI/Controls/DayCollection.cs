#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Summary description for LastSevenDaysControl.
    /// </summary>
    public class DayCollection : BaseControl
    {
        private ICollection<EntryDay> bpdc;
        protected Repeater DaysList;

        public ICollection<EntryDay> Days
        {
            set { bpdc = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(bpdc != null)
            {
                DaysList.DataSource = bpdc;
                DaysList.DataBind();
            }
            else
            {
                Visible = false;
            }
        }
    }
}
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

namespace Subtext.Web.HostAdmin
{
    /// <summary>
    /// Shows some diagnostic information.
    /// </summary>
    public partial class Queue : HostAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ltlActiveThreads.Text = ThreadPool.GetAvailableThreads().ToString(CultureInfo.InvariantCulture);
            //ltlWaitingCallbacks.Text = ManagedThreadPool.WaitingCallbacks.ToString(CultureInfo.InvariantCulture);
        }
    }
}
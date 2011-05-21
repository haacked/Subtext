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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Logging;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.Pages
{
    public partial class ErrorLog : StatsPage
    {
        private int _logPageNumber;

        public ErrorLog()
        {
            TabSectionId = "Stats";
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadPage();
            base.OnLoad(e);
        }

        private void LoadPage()
        {
            if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
            {
                _logPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = _logPageNumber;

            BindList();
        }

        private void BindList()
        {
            IPagedCollection<LogEntry> logEntries = LoggingProvider.Instance().GetPagedLogEntries(
                resultsPager.PageIndex, resultsPager.PageSize);
            resultsPager.ItemCount = logEntries.MaxItems;
            LogPage.DataSource = logEntries;
            LogPage.DataBind();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            LoggingProvider.Instance().ClearLog();
            resultsPager.PageIndex = 0; //Back to first page.
            BindList();
        }

        private void BindListForExcel()
        {
            IPagedCollection<LogEntry> logEntries = LoggingProvider.Instance().GetPagedLogEntries(0, int.MaxValue - 1);
            LogPage.DataSource = logEntries;
            LogPage.DataBind();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            BindListForExcel();
            ControlHelper.ExportToExcel(LogPage, "SubtextErrorLog.xls");
        }

        override protected void OnInit(EventArgs e)
        {
            btnClearLog.Click += btnClearLog_Click;
            btnExportToExcel.Click += btnExportToExcel_Click;
        }
    }
}
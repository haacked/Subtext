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
using Subtext.Web.Admin;
using Subtext.Web.Controls;

namespace Subtext.Web.HostAdmin.Pages
{
    public partial class ErrorLog : HostAdminPage
    {
        private int logPageNumber;

        protected override void OnLoad(EventArgs e)
        {
            LoadPage();
            base.OnLoad(e);
        }

        private void LoadPage()
        {
            if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
            {
                logPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = logPageNumber;

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

        protected string FormatLogger(object logger)
        {
            var loggerText = (string)logger;
            return
                loggerText.Replace("Subtext.Framework.", string.Empty).Replace("Subtext.Framework.", string.Empty).
                    Replace("Subtext.", string.Empty);
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClearLog.Click += new EventHandler(this.btnClearLog_Click);
            this.btnExportToExcel.Click += new EventHandler(btnExportToExcel_Click);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Services.SearchEngine;
using Subtext.ImportExport;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    public partial class FullTextSearch : AdminOptionsPage
    {
        private readonly static ILog Log = new Log();

        protected override void BindLocalUI()
        {
            UpdateIndexSize();
            base.BindLocalUI();
        }

        private void UpdateIndexSize()
        {
            ltrPostIndexedCount.Text = SearchEngine.GetIndexedEntryCount(Blog.Id).ToString();
        }

        protected void btnRebuild_Click(object sender, EventArgs e)
        {
            var indexingService = SubtextContext.ServiceLocator.GetService<IIndexingService>();
            try
            {
                indexingService.RebuildIndex();
            }
            catch(Exception ex)
            {
                Log.Error(Resources.FullTextSearch_ReindexingFailed, ex);
                Messages.ShowError(ex.Message, true);
            }
            UpdateIndexSize();
            Messages.ShowMessage(Resources.FullTextSearch_ReindexingCompleted);
        }
    }
}

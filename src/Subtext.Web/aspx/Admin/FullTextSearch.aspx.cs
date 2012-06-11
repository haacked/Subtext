using System;
using System.Linq;
using System.Web.Mvc;
using log4net;
using Subtext.Framework;
using Subtext.Framework.Logging;
using Subtext.Framework.Services.SearchEngine;
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
                var errors = indexingService.RebuildIndex();
                if (errors.Count() == 0)
                    Messages.ShowMessage(Resources.FullTextSearch_ReindexingCompleted);
                else
                {
                    string errormessages = string.Empty;
                    foreach (var error in errors)
                    {
                        errormessages += String.Format("<li>Unable to index post with id {0}: {1}</li>", error.Entry.EntryId, error.Exception.Message);
                    }
                    Messages.ShowError(String.Format(Resources.FullTextSearch_ReindexingCompletedWithErrorsFormat, errormessages));
                }
            }
            catch (Exception ex)
            {
                Log.Error(Resources.FullTextSearch_ReindexingFailed, ex);
                Messages.ShowError(ex.Message, true);
            }
            UpdateIndexSize();
        }
    }
}

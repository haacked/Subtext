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
using System.Web.Mvc;
using log4net;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.ImportExport;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Renders the page used to import and export blog data using 
    /// the BlogML format proposed in 
    /// <see href="http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236">this blog</see>
    /// </summary>
    public partial class ImportExportPage : AdminOptionsPage
    {
        private readonly static ILog Log = new Log();

        protected override void OnInit(EventArgs e)
        {
            Load += Page_Load;
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect(AdminUrl.Export(chkEmbedAttach.Checked));
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    //Temporarily extend script timeout for large BlogML imports
                    if (Server.ScriptTimeout < 3600)
                    {
                        Server.ScriptTimeout = 3600;
                    }
                    LoadBlogML();
                }
                catch (InvalidOperationException)
                {
                    Messages.ShowError(Resources.ImportExport_InvalidBlogMLFile, true);
                }
            }
        }

        private void LoadBlogML()
        {
            ISubtextContext context = SubtextContext;
            var importService = context.ServiceLocator.GetService<IBlogImportService>();

            try
            {
                importService.ImportBlog(importBlogMLFile.PostedFile.InputStream);
            }
            catch (BlogImportException e)
            {
                Log.Error(Resources.ImportExport_ImportFailed, e);
                Messages.ShowError(e.Message, true);
            }
            finally
            {
                importBlogMLFile.PostedFile.InputStream.Close();
            }

            Messages.ShowMessage(Resources.ImportExport_ImportSuccess);
        }

        protected void btnClearContent_Click(object sender, EventArgs e)
        {
            if (chkClearContent.Checked)
            {
                chkClearContent.Checked = false;
                chkClearContent.Visible = false;
                btnClearContent.Visible = false;

                Repository.ClearBlogContent(Config.CurrentBlog.Id);
                msgpnlClearContent.ShowMessage(Resources.ImportExport_ContentObliterated);
            }
            else
            {
                msgpnlClearContent.ShowError(Resources.ImportExport_CheckContinueToClearContent);
            }
        }
    }
}
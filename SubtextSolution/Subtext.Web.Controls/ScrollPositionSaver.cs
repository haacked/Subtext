using System;
using System.Web.UI;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Non-Visible control used to save and restore scroll position 
    /// within a page after a postback.  Just drop it on a page and 
    /// you are good to go.
    /// </summary>
    /// <remarks>
    /// Adapted from <see href="http://aspnet.4guysfromrolla.com/articles/111704-1.2.aspx"/>
    /// </remarks>
    public class ScrollPositionSaver : Control
    {
        private string ClientScriptKey
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Registers hidden fields and the script for handling scroll position.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            Page.ClientScript.RegisterHiddenField("__scrollLeft", Page.Request["__scrollLeft"]);
            Page.ClientScript.RegisterHiddenField("__scrollTop", Page.Request["__scrollTop"]);

            if(!Page.ClientScript.IsStartupScriptRegistered(ClientScriptKey))
            {
                Type cstype = GetType();
                Page.ClientScript.RegisterClientScriptBlock(cstype, ClientScriptKey,
                                                            ScriptHelper.UnpackScript("ScrollPositionSaver.js"));
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Ensures this is being rendered within a Web Form. 
        /// If not, this raises an exception.
        /// </summary>
        /// <param name="writer">The <see langword="HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            Page.VerifyRenderingInServerForm(this);
            base.Render(writer);
        }
    }
}
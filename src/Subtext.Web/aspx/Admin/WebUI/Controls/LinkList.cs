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
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
    public class LinkList : Control
    {
        LinkControlCollection _items = new LinkControlCollection();

        public LinkControlCollection Items
        {
            get { return _items; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (_items.Count > 0)
            {
                output.WriteBeginTag("ul");
                output.WriteAttribute("id", ClientID, false);
                output.Write(HtmlTextWriter.TagRightChar);
                foreach (WebControl currentLink in _items)
                {
                    output.RenderBeginTag(HtmlTextWriterTag.Li);
                    currentLink.RenderControl(output);
                    output.RenderEndTag();
                }
                output.WriteEndTag("ul");
            }
        }
    }

    // REFACTOR:
    // This used to be hyperlinks only, I wanted to add LinkButtons. The share
    // WebControl as an ancestor along with 13,000 other things. So this is kind 
    // of bad pool here, need to reevalutate how to get same functionality but
    // in a cleaner way.
    public sealed class LinkControlCollection : Collection<WebControl>
    {
        public LinkControlCollection()
            : base()
        {
        }

        public void Add(string text, string navigateUrl)
        {
            Add(text, navigateUrl, null, null);
        }

        public void Add(string text, string navigateUrl, string cssClass)
        {
            Add(text, navigateUrl, cssClass, null);
        }

        public void Add(string text, string navigateUrl, string cssClass, string target)
        {
            var adding = new HyperLink();
            adding.Text = text;
            adding.NavigateUrl = navigateUrl;
            if (null != cssClass && cssClass.Length > 0)
            {
                adding.CssClass = cssClass;
            }
            if (null != target && target.Length > 0)
            {
                adding.Target = target;
            }

            Add(adding);
        }

        public LinkButton Add(string text, EventHandler targetHandler)
        {
            return Add(text, null, targetHandler);
        }

        public LinkButton Add(string text, string cssClass, EventHandler targetHandler)
        {
            var adding = new LinkButton();
            adding.Text = text;
            adding.Click += new EventHandler(targetHandler);

            if (null != cssClass && cssClass.Length > 0)
            {
                adding.CssClass = cssClass;
            }

            Add(adding);
            return adding;
        }
    }
}
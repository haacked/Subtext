using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.WebControls
{
    [PartialCaching(600, null, null, "Blogger", true)]
    public class MetaTagsControl : Control
    {
        public Blog Blog
        {
            get;
            set;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            var parent = Parent.Page as ISubtextDependencies;
            IPagedCollection<MetaTag> blogMetaTags = parent.Repository.GetMetaTagsForBlog(Blog, 0, int.MaxValue);
            foreach (MetaTag tag in blogMetaTags)
            {
                var htmlMetaTag = new HtmlMeta { Content = tag.Content };

                if (!string.IsNullOrEmpty(tag.Name))
                {
                    htmlMetaTag.Name = tag.Name;
                }
                else
                {
                    htmlMetaTag.HttpEquiv = tag.HttpEquiv;
                }

                var newLineLiteral = new Literal { Text = Environment.NewLine };
                newLineLiteral.RenderControl(writer);
                htmlMetaTag.RenderControl(writer);
            }
        }
    }
}

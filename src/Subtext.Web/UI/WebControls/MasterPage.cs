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
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;

namespace Subtext.Web.UI.WebControls
{
    //TODO: Get rid of this
    /// <summary>
    /// <p>Serves as the master template for the Subtext site.</p>
    /// <p>
    /// The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off 
    /// of Paul Wilson's excellent demo found
    /// here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
    /// Very MINOR changes were made here. Thanks Paul.
    /// </p>
    /// </summary>
    [ToolboxData("<{0}:MasterPage runat=server></{0}:MasterPage>")]
    [ToolboxItem(typeof(WebControlToolboxItem))]
    [Designer(typeof(ContainerControlDesigner))]
    public class MasterPage : HtmlContainerControl
    {
        private const string SkinPath = "~/Skins/{0}/PageTemplate.ascx";
        private readonly List<ContentRegion> _contents = new List<ContentRegion>();
        private Control _template;
        private string _templateFile;

        /// <summary>
        /// Gets or sets the template file from the Skins directory.
        /// </summary>
        /// <value></value>
        [Category("MasterPage")]
        [Description("Path of Template User Control")]
        public string TemplateFile
        {
            get
            {
                if (_templateFile == null)
                {
                    var skin = SkinConfig.GetCurrentSkin(Config.CurrentBlog, new HttpContextWrapper(HttpContext.Current));
                    _templateFile = string.Format(SkinPath, skin.TemplateFolder);
                }
                return _templateFile;
            }
            set { _templateFile = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {
            var contentRegion = obj as ContentRegion;
            if (contentRegion != null)
            {
                _contents.Add(contentRegion);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            BuildMasterPage();
            BuildContents();
            base.OnInit(e);
        }

        private void BuildMasterPage()
        {
            if (String.IsNullOrEmpty(TemplateFile))
            {
                throw new InvalidOperationException(Resources.InvalidOperation_TemplateFileIsNull);
            }
            _template = Page.LoadControl(TemplateFile);
            _template.ID = ID + "_Template";

            int count = _template.Controls.Count;
            for (int index = 0; index < count; index++)
            {
                Control control = _template.Controls[0];
                _template.Controls.Remove(control);
                if (control.Visible)
                {
                    Controls.Add(control);
                }
            }
            Controls.AddAt(0, _template);
        }

        private void BuildContents()
        {
            foreach (ContentRegion content in _contents)
            {
                Control region = FindControl(content.ID);
                if (region == null)
                {
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_ContentRegionNotFound,
                                                                      content.ID));
                }
                region.Controls.Clear();

                int count = content.Controls.Count;
                for (int index = 0; index < count; index++)
                {
                    Control control = content.Controls[0];
                    content.Controls.Remove(control);
                    region.Controls.Add(control);
                }
            }
        }

        //removes this controls ability to render its own start tag
        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        //removes this controls ability to render its own end tag
        protected override void RenderEndTag(HtmlTextWriter writer)
        {
        }
    }
}
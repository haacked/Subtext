#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Logging;
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
        private const string skinPath = "~/Skins/{0}/PageTemplate.ascx";
        private readonly List<ContentRegion> contents = new List<ContentRegion>();
        Log log = new Log();
        private Control template;
        private string templateFile;

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
                if(templateFile == null)
                {
                    templateFile = string.Format(skinPath, Globals.CurrentSkin.TemplateFolder);
                }
                return templateFile;
            }
            set { templateFile = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {
            var contentRegion = obj as ContentRegion;
            if(contentRegion != null)
            {
                contents.Add(contentRegion);
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
            if(String.IsNullOrEmpty(TemplateFile))
            {
                throw new InvalidOperationException(Resources.InvalidOperation_TemplateFileIsNull);
            }
            template = Page.LoadControl(TemplateFile);
            template.ID = ID + "_Template";

            int count = template.Controls.Count;
            for(int index = 0; index < count; index++)
            {
                Control control = template.Controls[0];
                template.Controls.Remove(control);
                if(control.Visible)
                {
                    Controls.Add(control);
                }
            }
            Controls.AddAt(0, template);
        }

        private void BuildContents()
        {
            foreach(ContentRegion content in contents)
            {
                Control region = FindControl(content.ID);
                if(region == null)
                {
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_ContentRegionNotFound,
                                                                      content.ID));
                }
                region.Controls.Clear();

                int count = content.Controls.Count;
                for(int index = 0; index < count; index++)
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
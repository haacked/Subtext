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

using System.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Email
{
    public class EmbeddedTemplateEngine : ITemplateEngine
    {
        public ITextTemplate GetTemplate(string templateName)
        {
            string resourceName = "Subtext.Framework.Services.Email.Templates." + templateName + ".template";
            string templateText = ResourceHelper.UnpackEmbeddedResource(resourceName, Encoding.UTF8);
            return new NamedFormatTextTemplate(templateText);
        }
    }
}
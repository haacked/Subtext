using System.Text;
using Subtext.Framework.Resource;

namespace Subtext.Framework.Email
{
    public class EmbeddedTemplateEngine : ITemplateEngine
    {
        public ITextTemplate GetTemplate(string templateName)
        {
            string resourceName = "Subtext.Framework.Email.Templates." + templateName + ".template";
            string templateText = ResourceHelper.UnpackEmbeddedResource(resourceName, Encoding.UTF8);
            return new NamedFormatTextTemplate(templateText);
        }
    }
}

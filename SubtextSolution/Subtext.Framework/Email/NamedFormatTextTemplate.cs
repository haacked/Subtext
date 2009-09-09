using Subtext.Framework.Text;

namespace Subtext.Framework.Email
{
    public class NamedFormatTextTemplate : ITextTemplate
    {
        public NamedFormatTextTemplate(string template)
        {
            Template = template;
        }

        public string Template { get; private set; }

        #region ITextTemplate Members

        public string Format(object data)
        {
            return Template.NamedFormat(data);
        }

        #endregion

        public override string ToString()
        {
            return Template;
        }
    }
}
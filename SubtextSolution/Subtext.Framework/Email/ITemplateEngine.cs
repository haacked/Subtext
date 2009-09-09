namespace Subtext.Framework.Email
{
    public interface ITemplateEngine
    {
        ITextTemplate GetTemplate(string templateName);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Email
{
    public interface ITemplateEngine
    {
        ITextTemplate GetTemplate(string templateName);
    }
}

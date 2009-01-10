using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Components;

namespace Subtext.Framework.Email
{
    public interface IEmailService
    {
        void EmailCommentToBlogAuthor(FeedbackItem comment);
    }
}

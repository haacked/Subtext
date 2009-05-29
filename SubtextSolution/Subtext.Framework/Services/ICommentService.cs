using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
    public interface ICommentService
    {
        int Create(FeedbackItem feedback, ISubtextContext context);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Routing
{
    public interface ISubtextHandler : IRoutableHandler
    {
        ISubtextContext SubtextContext {
            get;
            set;
        }
    }
}

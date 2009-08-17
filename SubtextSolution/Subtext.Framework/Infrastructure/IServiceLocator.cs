using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Infrastructure
{
    public interface IServiceLocator
    {
        TService GetService<TService>();
    }
}

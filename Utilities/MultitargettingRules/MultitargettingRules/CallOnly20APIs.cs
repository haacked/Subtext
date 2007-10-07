using System.Resources;
using System.IO;
using System.Reflection;
using Krzysztof.MultitargettingRules.Base;

namespace Krzysztof.MultitargettingRules
{
    public class CallOnly20APIs : DontUseAPIsRuleBase
    {
        public CallOnly20APIs()
            : base("CallOnly20APIs")
        {
        }
    }
}

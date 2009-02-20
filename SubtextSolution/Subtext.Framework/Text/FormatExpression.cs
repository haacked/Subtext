#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using System.Web.UI;

namespace Subtext.Framework.Text
{
    public class FormatExpression : ITextExpression
    {
        bool _invalidExpression = false;

        public FormatExpression(string expression)
        {
            if (!expression.StartsWith("{") || !expression.EndsWith("}"))
            {
                _invalidExpression = true;
                Expression = expression;
                return;
            }

            string expressionWithoutBraces = expression.Substring(1
                , expression.Length - 2);
            int colonIndex = expressionWithoutBraces.IndexOf(':');
            if (colonIndex < 0)
            {
                Expression = expressionWithoutBraces;
            }
            else
            {
                Expression = expressionWithoutBraces.Substring(0, colonIndex);
                Format = expressionWithoutBraces.Substring(colonIndex + 1);
            }
        }

        public string Expression
        {
            get;
            private set;
        }

        public string Format
        {
            get;
            private set;
        }

        public string Eval(object o)
        {
            if (_invalidExpression) {
                throw new FormatException("Invalid expression");
            }
            
            try {
                if (String.IsNullOrEmpty(Format)) {
                    return (DataBinder.Eval(o, Expression) ?? string.Empty).ToString();
                }
                return (DataBinder.Eval(o, Expression, "{0:" + Format + "}") ?? string.Empty).ToString();
            }
            catch (HttpException e) {
                throw new FormatException("Could not format '" + Expression + ":" + Format + "'", e);
            }
        }
    }
}

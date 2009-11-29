#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Text
{
    public class FormatExpression : ITextExpression
    {
        readonly bool _invalidExpression;

        public FormatExpression(string expression)
        {
            if(!expression.StartsWith("{") || !expression.EndsWith("}"))
            {
                _invalidExpression = true;
                Expression = expression;
                return;
            }

            string expressionWithoutBraces = expression.Substring(1
                                                                  , expression.Length - 2);
            int colonIndex = expressionWithoutBraces.IndexOf(':');
            if(colonIndex < 0)
            {
                Expression = expressionWithoutBraces;
            }
            else
            {
                Expression = expressionWithoutBraces.Substring(0, colonIndex);
                Format = expressionWithoutBraces.Substring(colonIndex + 1);
            }
        }

        public string Expression { get; private set; }

        public string Format { get; private set; }

        #region ITextExpression Members

        public string Eval(object o)
        {
            if(_invalidExpression)
            {
                throw new FormatException(Resources.Format_InvalidExpression);
            }

            try
            {
                if(String.IsNullOrEmpty(Format))
                {
                    return (DataBinder.Eval(o, Expression) ?? string.Empty).ToString();
                }
                return (DataBinder.Eval(o, Expression, "{0:" + Format + "}") ?? string.Empty);
            }
            catch(HttpException e)
            {
                throw new FormatException(
                    String.Format(CultureInfo.InvariantCulture, Resources.Format_CouldNotFormatExpression, Expression,
                                  Format), e);
            }
        }

        #endregion
    }
}
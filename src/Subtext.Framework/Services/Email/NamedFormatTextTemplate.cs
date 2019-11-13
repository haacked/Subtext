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
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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Subtext.Framework.Services;

namespace Subtext.Framework.Emoticons
{
    /// <summary>
    /// Converts emoticons to img tags.
    /// </summary>
    public class EmoticonsTransformation : ITextTransformation
    {
        public EmoticonsTransformation(ISubtextContext context)
            : this(new EmoticonsFileSource(context), null)
        {
            _subtextContext = context;
        }

        ISubtextContext _subtextContext;

        public EmoticonsTransformation(IEmoticonsSource emoticonsSource, string appRootUrl)
        {
            EmoticonsTable = emoticonsSource.GetEmoticons();
            _appRootUrl = appRootUrl;
        }

        string _appRootUrl;

        protected IEnumerable<Emoticon> EmoticonsTable
        {
            get;
            private set;
        }

        public string Transform(string original) {
            if (_appRootUrl == null && _subtextContext != null && _subtextContext.UrlHelper != null) {
                //TODO: Temporary Hack.
                _appRootUrl = _subtextContext.UrlHelper.AppRoot();
            }
            return EmoticonsTable.Aggregate(original, (input, transform) => transform.Replace(input, _appRootUrl));
        }
    }
}

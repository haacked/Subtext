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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Subtext.Framework.Emoticons
{
    public class EmoticonsFileSource : IEmoticonsSource, IDisposable
    {
        readonly string _path;
        readonly StreamReader _reader;

        private readonly ISubtextContext _subtextContext;

        public EmoticonsFileSource(ISubtextContext context)
        {
            _subtextContext = context;
        }

        public EmoticonsFileSource(string path)
        {
            _path = path ?? _subtextContext.RequestContext.HttpContext.Request.MapPath("~/emoticons.txt");
            if (!String.IsNullOrEmpty(_path))
            {
                _reader = File.OpenText(_path);
            }
        }

        public EmoticonsFileSource(StreamReader reader)
        {
            _reader = reader ?? File.OpenText(_path);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
        }

        #endregion

        #region IEmoticonsSource Members

        public IEnumerable<Emoticon> GetEmoticons()
        {
            if (_reader == null)
            {
                return new List<Emoticon>();
            }
            return GetEnumerable().ToList();
        }

        #endregion

        private IEnumerable<Emoticon> GetEnumerable()
        {
            string emoticonText = _reader.ReadLine();
            while (emoticonText != null)
            {
                string imageTag = _reader.ReadLine();
                yield return new Emoticon(emoticonText, imageTag);
                emoticonText = _reader.ReadLine();
            }
        }
    }
}
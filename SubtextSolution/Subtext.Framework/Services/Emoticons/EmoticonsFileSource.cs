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
            if(!String.IsNullOrEmpty(_path))
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
            if(_reader != null)
            {
                _reader.Dispose();
            }
        }

        #endregion

        #region IEmoticonsSource Members

        public IEnumerable<Emoticon> GetEmoticons()
        {
            if(_reader == null)
            {
                return new List<Emoticon>();
            }
            return GetEnumerable().ToList();
        }

        #endregion

        private IEnumerable<Emoticon> GetEnumerable()
        {
            string emoticonText = _reader.ReadLine();
            while(emoticonText != null)
            {
                string imageTag = _reader.ReadLine();
                yield return new Emoticon(emoticonText, imageTag);
                emoticonText = _reader.ReadLine();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Subtext.Framework.Emoticons
{
    public class EmoticonsFileSource : IEmoticonsSource, IDisposable
    {
        StreamReader _reader;

        public EmoticonsFileSource(ISubtextContext context)
        {
            _subtextContext = context;
        }
        private ISubtextContext _subtextContext;

        public EmoticonsFileSource(string path) {
            _path = path ?? _subtextContext.RequestContext.HttpContext.Request.MapPath("~/emoticons.txt");
            if (!String.IsNullOrEmpty(_path)) {
                _reader = File.OpenText(_path);
            }
        }

        string _path;

        public EmoticonsFileSource(StreamReader reader) {
            _reader = reader ?? File.OpenText(_path);    
        }

        public IEnumerable<Emoticon> GetEmoticons()
        {
            if (_reader == null) {
                return new List<Emoticon>();
            }
            return GetEnumerable().ToList();
        }

        private IEnumerable<Emoticon> GetEnumerable() {

            string emoticonText = _reader.ReadLine();
            while (emoticonText != null)
            {
                string imageTag = _reader.ReadLine();
                yield return new Emoticon(emoticonText, imageTag);
                emoticonText = _reader.ReadLine();
            }
        }

        public void Dispose()
        {
            if (_reader != null) {

                _reader.Dispose();
            }
        }
    }
}

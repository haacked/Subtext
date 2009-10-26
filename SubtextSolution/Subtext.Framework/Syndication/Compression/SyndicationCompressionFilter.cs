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

using System.IO;

namespace Subtext.Framework.Syndication.Compression
{
    public class SyndicationCompressionFilter
    {
        private readonly string _encoding;
        private readonly Stream _stream;

        #region -- Constructor(Stream) --

        public SyndicationCompressionFilter(Stream stream)
        {
            _stream = stream;
            _encoding = null;
        }

        #endregion

        #region -- Constructor(Stream, encoding) --

        public SyndicationCompressionFilter(Stream stream, string encoding)
        {
            _stream = stream;
            _encoding = encoding;
        }

        #endregion

        #region -- ContentEncoding Property --

        public string ContentEncoding
        {
            get { return _encoding; }
        }

        #endregion

        #region -- Filter Property --

        public Stream Filter
        {
            get { return _stream; }
        }

        #endregion

        /*-- Constructors --*/

        /*-- Properties --*/
    }
}
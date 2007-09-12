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

using System.IO;

namespace Subtext.Framework.Syndication.Compression
{
	public class SyndicationCompressionFilter
	{
		private string _encoding;
		private Stream _stream;

		/*-- Constructors --*/

		#region -- Constructor(Stream) --
		public SyndicationCompressionFilter(Stream stream)
		{
			_stream = stream;
		}
		#endregion

		#region -- Constructor(Stream, encoding) --
		public SyndicationCompressionFilter(Stream stream, string encoding)
		{
			_stream = stream;
			_encoding = encoding;
		}
		#endregion

		/*-- Properties --*/

		#region -- ContentEncoding Property --
		public string ContentEncoding
		{
			get
			{
				return _encoding;
			}
		}
		#endregion

		#region -- Filter Property --
		public Stream Filter
		{
			get
			{
				return _stream;
			}
		}
		#endregion
	}
}

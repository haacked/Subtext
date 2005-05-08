using System;
using System.IO;

namespace Subtext.Framework.Syndication.Compression
{
	public class SyndicationCompressionFilter
	{
		private string _encoding = null;
		private Stream _stream;

		/*-- Constructors --*/

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

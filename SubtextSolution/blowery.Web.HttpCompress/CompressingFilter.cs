using System;
using System.IO;
using System.Web;

namespace blowery.Web.HttpCompress {
  /// <summary>
  /// Base for any HttpFilter that performing compression
  /// </summary>
  /// <remarks>
  /// When implementing this class, you need to implement a <see cref="HttpOutputFilter"/>
  /// along with a <see cref="CompressingFilter.ContentEncoding"/>.  The latter corresponds to a 
  /// content coding (see http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.5)
  /// that your implementation will support.
  /// </remarks>
  public abstract class CompressingFilter : HttpOutputFilter {

    private bool hasWrittenHeaders = false;

    /// <summary>
    /// Protected constructor that sets up the underlying stream we're compressing into
    /// </summary>
    /// <param name="baseStream">The stream we're wrapping up</param>
    /// <param name="compressionLevel">The level of compression to use when compressing the content</param>
    protected CompressingFilter(Stream baseStream, CompressionLevels compressionLevel) : base(baseStream) {
      _compressionLevel = compressionLevel;
    }

    /// <summary>
    /// The name of the content-encoding that's being implemented
    /// </summary>
    /// <remarks>
    /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.5 for more
    /// details on content codings.
    /// </remarks>
    public abstract string ContentEncoding { get; }

    private CompressionLevels _compressionLevel;

    /// <summary>
    /// Allow inheriting classes to get access the the level of compression that should be used
    /// </summary>
    protected CompressionLevels CompressionLevel {
      get { return _compressionLevel; }
    }

    /// <summary>
    /// Keeps track of whether or not we're written the compression headers
    /// </summary>
    protected bool HasWrittenHeaders { 
      get { return hasWrittenHeaders; } 
    }

    /// <summary>
    /// Writes out the compression-related headers.  Subclasses should call this once before writing to the output stream.
    /// </summary>
    protected void WriteHeaders() {
      // this is dangerous.  if Response.End is called before the filter is used, directly or indirectly,
      // the content will not pass through the filter.  However, this header will still be appended.  
      // Look for handling cases in PreRequestSendHeaders and Pre
      HttpContext.Current.Response.AppendHeader("Content-Encoding", this.ContentEncoding);
      HttpContext.Current.Response.AppendHeader("X-Compressed-By", "HttpCompress");
      hasWrittenHeaders = true;
    }


  }
}

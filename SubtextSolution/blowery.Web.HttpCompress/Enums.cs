namespace blowery.Web.HttpCompress
{
  /// <summary>
  /// The available compression algorithms to use with the HttpCompressionModule
  /// </summary>
  public enum Algorithms {
    /// <summary>Use the Deflate algorithm</summary>
    Deflate,
    /// <summary>Use the GZip algorithm</summary>
    GZip,
    /// <summary>Use the default algorithm (picked by client)</summary>
    Default=-1
  }

  /// <summary>
  /// The level of compression to use with deflate
  /// </summary>
  public enum CompressionLevels {
    /// <summary>Use the default compression level</summary>
    Default = -1,
    /// <summary>The highest level of compression.  Also the slowest.</summary>
    Highest = 9,
    /// <summary>A higher level of compression.</summary>
    Higher = 8,
    /// <summary>A high level of compression.</summary>
    High = 7,
    /// <summary>More compression.</summary>
    More = 6,
    /// <summary>Normal compression.</summary>
    Normal = 5,
    /// <summary>Less than normal compression.</summary>
    Less = 4,
    /// <summary>A low level of compression.</summary>
    Low = 3,
    /// <summary>A lower level of compression.</summary>
    Lower = 2,
    /// <summary>The lowest level of compression that still performs compression.</summary>
    Lowest = 1,
    /// <summary>No compression.  Use this is you are quite silly.</summary>
    None = 0
  }
}

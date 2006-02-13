using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace blowery.Web.HttpCompress {
  /// <summary>
  /// This class encapsulates the settings for an HttpCompressionModule
  /// </summary>
  public sealed class Settings {

    private Algorithms _preferredAlgorithm;
    private CompressionLevels _compressionLevel;
    private StringCollection _excludedTypes;
    private StringCollection _excludedPaths;

    /// <summary>
    /// Create an HttpCompressionModuleSettings from an XmlNode
    /// </summary>
    /// <param name="node">The XmlNode to configure from</param>
    public Settings(XmlNode node) : this() {
      AddSettings(node);
    }

    
    private Settings() {
      _preferredAlgorithm = Algorithms.Default;
      _compressionLevel = CompressionLevels.Default;
      _excludedTypes = new StringCollection();
      _excludedPaths = new StringCollection();
    }

    /// <summary>
    /// Suck in some more changes from an XmlNode.  Handy for config file parenting.
    /// </summary>
    /// <param name="node">The node to read from</param>
    public void AddSettings(XmlNode node) {
      if(node == null)
        return;

      XmlAttribute preferredAlgorithm = node.Attributes["preferredAlgorithm"];
      if(preferredAlgorithm != null) {
        try {
          _preferredAlgorithm = (Algorithms)Enum.Parse(typeof(Algorithms), preferredAlgorithm.Value, true);
        }catch(ArgumentException) { }
      }

      XmlAttribute compressionLevel = node.Attributes["compressionLevel"];
      if(compressionLevel != null) {
        try {
          _compressionLevel = (CompressionLevels)Enum.Parse(typeof(CompressionLevels), compressionLevel.Value, true);
        }catch(ArgumentException) { }
      }

      ParseExcludedTypes(node.SelectSingleNode("excludedMimeTypes"));
      ParseExcludedPaths(node.SelectSingleNode("excludedPaths"));

    }


    /// <summary>
    /// Get the current settings from the xml config file
    /// </summary>
    public static Settings GetSettings() {
      Settings settings = (Settings)System.Configuration.ConfigurationSettings.GetConfig("blowery.web/httpCompress");
      if(settings == null)
        return Settings.Default;
      else
        return settings;
    }

    /// <summary>
    /// The default settings.  Deflate + normal.
    /// </summary>
    public static Settings Default {
      get { return new Settings(); }
    }

    /// <summary>
    /// The preferred algorithm to use for compression
    /// </summary>
    public Algorithms PreferredAlgorithm {
      get { return _preferredAlgorithm; }
    }
    

    /// <summary>
    /// The preferred compression level
    /// </summary>
    public CompressionLevels CompressionLevel {
      get { return _compressionLevel; }
    }
    

    /// <summary>
    /// Checks a given mime type to determine if it has been excluded from compression
    /// </summary>
    /// <param name="mimetype">The MimeType to check.  Can include wildcards like image/* or */xml.</param>
    /// <returns>true if the mime type passed in is excluded from compression, false otherwise</returns>
    public bool IsExcludedMimeType(string mimetype) {
      return _excludedTypes.Contains(mimetype.ToLower());
    }

    /// <summary>
    /// Looks for a given path in the list of paths excluded from compression
    /// </summary>
    /// <param name="relUrl">the relative url to check</param>
    /// <returns>true if excluded, false if not</returns>
    public bool IsExcludedPath(string relUrl) {
      return _excludedPaths.Contains(relUrl.ToLower());
    }

    private void ParseExcludedTypes(XmlNode node) {
      if(node == null) return;

      for(int i = 0; i < node.ChildNodes.Count; ++i) {
        switch(node.ChildNodes[i].LocalName) {
          case "add":
            if(node.ChildNodes[i].Attributes["type"] != null)
              _excludedTypes.Add(node.ChildNodes[i].Attributes["type"].Value.ToLower());
            break;
          case "delete":
            if(node.ChildNodes[i].Attributes["type"] != null)
              _excludedTypes.Remove(node.ChildNodes[i].Attributes["type"].Value.ToLower());
            break;
        }
      }
    }

    private void ParseExcludedPaths(XmlNode node) {
      if(node == null) return;

      for(int i = 0; i < node.ChildNodes.Count; ++i) {
        switch(node.ChildNodes[i].LocalName) {
          case "add":
            if(node.ChildNodes[i].Attributes["path"] != null) 
              _excludedPaths.Add(node.ChildNodes[i].Attributes["path"].Value.ToLower());
            break;
          case "delete":
            if(node.ChildNodes[i].Attributes["path"] != null)
              _excludedPaths.Remove(node.ChildNodes[i].Attributes["path"].Value.ToLower());
            break;
        }
      }
    }
    
  }

}


#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext
// 
// Subtext is a BSD Licensed open source weblog system started by Phil Haack 
// based on .TEXT by Scott Watermasysk. 
// 
// Blog: http://haacked.com/
// RSS: http://haacked.com/rss.aspx
// Email: haacked@gmail.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

// This source code provided by Brad Wilson and entered into the public
// domain. (May 22, 2003)
//
// Use, redistribution and modification of this source code is unlimited,
// including any personal and/or commercial use.
//
// There are no warranties of any kind. Use at your own risk!
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security;
using System.Web;

// namespace DotNetGuy.Utility.Web - Renamed for consistency
namespace Subtext.Common.UrlManager
{
  /// <summary>
  /// Replacement static file handler class. Overcomes the "24-hour cache" bug
  /// in ASP.NET's default static file handler implementation.
  /// </summary>
  public class BlogStaticFileHandler : IHttpHandler
  {
    // Static constructor

    /// <summary>
    /// Static constructor that fills the string dictionary with
    /// the known MIME types.
    /// </summary>
    static BlogStaticFileHandler()
    {
      _mimeMap = new StringDictionary();

      _mimeMap.Add("csv",  "application/vnd.ms-excel");
	  _mimeMap.Add("css",	"text/css");
	  _mimeMap.Add("js",	"text/javascript");
      _mimeMap.Add("doc",  "application/msword");
      _mimeMap.Add("gif",  "image/gif");
      _mimeMap.Add("htm",  "text/html");
      _mimeMap.Add("html", "text/html");
      _mimeMap.Add("jpeg", "image/jpeg");
      _mimeMap.Add("jpg",  "image/jpeg");
      _mimeMap.Add("pdf",  "application/pdf");
      _mimeMap.Add("png",  "image/png");
      _mimeMap.Add("ppt",  "application/vnd.ms-powerpoint");
      _mimeMap.Add("rtf",  "application/msword");
      _mimeMap.Add("txt",  "text/plain");
      _mimeMap.Add("xls",  "application/vnd.ms-excel");
      _mimeMap.Add("xml",  "text/xml");
    }

    // IHttpHandler implementation

    /// <summary>
    /// (IHttpHandler.IsReusable)
    /// </summary>
    bool IHttpHandler.IsReusable
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    /// (IHttpHandler.ProcessRequest)
    /// </summary>
    /// <param name="ctxt">HTTP context for the request</param>
    void IHttpHandler.ProcessRequest(HttpContext ctxt)
    {
      string filename = ctxt.Request.PhysicalPath;
    	FileInfo fi = _GetFileInfo(filename);

      if (!_FileIsOkayToServe(fi, filename))
        throw new HttpException(404, "Resource not found");

      if (_CachedVersionIsOkay(fi.LastWriteTime.ToUniversalTime(), ctxt.Request))
      {
        ctxt.Response.StatusCode = 304;
        ctxt.Response.SuppressContent = true;
        return;
      }

      ctxt.Response.ContentType = _GetMimeMapping(filename);
      ctxt.Response.AppendHeader("Content-Length", fi.Length.ToString());
      ctxt.Response.Cache.SetLastModified(fi.LastWriteTime);
      ctxt.Response.Cache.SetCacheability(HttpCacheability.Public);

      // TODO: we send the file in one big chunk; if the file is potentially
      // large, this code should be re-written to chunk the transfer in
      // decent sized pieces (say, 64k at a time)

	  ctxt.Response.WriteFile(filename);
    }

    // Private methods

    private bool _CachedVersionIsOkay(DateTime lastWriteTime, HttpRequest Request)
    {
      string ifModified = Request.Headers["If-Modified-Since"];

      if (ifModified != null)
      {
        string lastModified = lastWriteTime.ToString("r");
        return (ifModified == lastModified);
      }

      return false;
    }

    /// <summary>
    /// Verifies that the file is okay to serve.
    /// </summary>
    /// <param name="fi">Information about the file</param>
    /// <param name="filename">The filename</param>
    /// <returns>Returns true if the file is okay; false otherwise</returns>
    private bool _FileIsOkayToServe(FileInfo fi, string filename)
    {
      // Don't serve hidden files or directories

      if ((fi.Attributes & FileAttributes.Hidden) != 0)
        return false;

      if ((fi.Attributes & FileAttributes.Directory) != 0)
        return false;

      // We specifically exclude filenames that end in periods because the
      // ASP.NET static file handler does, but I'm not sure why (there's
      // probably some security reason for it...)

      if (filename[filename.Length - 1] == '.')
        return false;

      return true;
    }

    /// <summary>
    /// Returns a populated FileInfo structure for a file, throwing HTTP exceptions
    /// in response to missing files or security problems.
    /// </summary>
    /// <param name="filename">The filename</param>
    /// <returns>A populated FileInfo structure</returns>
    private FileInfo _GetFileInfo(string filename)
    {
      if (!File.Exists(filename))
        throw new HttpException(404, "Resource not found");

      try
      {
        return new FileInfo(filename);
      }
      catch (IOException)
      {
        throw new HttpException(404, "Resource not found");
      }
      catch (SecurityException)
      {
        throw new HttpException(401, "Access denied");
      }
    }

    /// <summary>
    /// Gets the MIME type for the given filename
    /// </summary>
    /// <param name="filename">The filename</param>
    /// <returns>Returns the mime type for the file; if unknown, return
    /// application/octet-stream (default binary data format in MIME)</returns>
    private static string _GetMimeMapping(string filename) 
    {
      string result = null;
      int idx = filename.LastIndexOf('.');

      if (idx > 0 && idx > filename.LastIndexOf('\\'))
        result = _mimeMap[filename.Substring(idx+1).ToLower(CultureInfo.InvariantCulture)];
		
      if (result == null)
        return "application/octet-stream";
      else
        return result;
    }

    // Local data

    private static StringDictionary _mimeMap;
  }
}


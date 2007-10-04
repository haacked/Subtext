using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Subtext.TestLibrary
{
	public interface IHttpResponse
	{
		void AddCacheDependency(params CacheDependency[] dependencies);
		void AddCacheItemDependencies(string[] cacheKeys);
		void AddCacheItemDependencies(ArrayList cacheKeys);
		void AddCacheItemDependency(string cacheKey);
		void AddFileDependencies(ArrayList filenames);
		void AddFileDependencies(string[] filenames);
		void AddFileDependency(string filename);
		void AddHeader(string name, string value);
		void AppendCookie(HttpCookie cookie);
		void AppendHeader(string name, string value);
		[AspNetHostingPermission(SecurityAction.Demand, Level=AspNetHostingPermissionLevel.Medium)]
		void AppendToLog(string param);
		string ApplyAppPathModifier(string virtualPath);
		void BinaryWrite(byte[] buffer);
		void Clear();
		void ClearContent();
		void ClearHeaders();
		void Close();
		void DisableKernelCache();
		void End();
		void Flush();
		void Pics(string value);
		void Redirect(string url);
		void Redirect(string url, bool endResponse);
		void SetCookie(HttpCookie cookie);
		void TransmitFile(string filename);
		void TransmitFile(string filename, long offset, long length);
		void Write(char ch);
		void Write(object obj);
		void Write(string s);
		void Write(char[] buffer, int index, int count);
		void WriteFile(string filename);
		void WriteFile(string filename, bool readIntoMemory);
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
		void WriteFile(IntPtr fileHandle, long offset, long size);
		void WriteFile(string filename, long offset, long size);
		void WriteSubstitution(HttpResponseSubstitutionCallback callback);
		// Properties
		bool Buffer { get; set; }
		bool BufferOutput { get; set; }
		HttpCachePolicy Cache { get; }
		string CacheControl { get; set; }
		string Charset { get; set; }
		Encoding ContentEncoding { get; set; }
		string ContentType { get; set; }
		HttpCookieCollection Cookies { get; }
		int Expires { get; set; }
		DateTime ExpiresAbsolute { get; set; }
		Stream Filter { get; set; }
		Encoding HeaderEncoding { get; set; }
		NameValueCollection Headers { get; }
		bool IsClientConnected { get; }
		bool IsRequestBeingRedirected { get; }
		TextWriter Output { get; }
		Stream OutputStream { get; }
		string RedirectLocation { get; set; }
		string Status { get; set; }
		int StatusCode { get; set; }
		string StatusDescription { get; set; }
		int SubStatusCode { get; set; }
		bool SuppressContent { get; set; }
	}
}

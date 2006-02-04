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

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using MbUnit.Framework;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Contains helpful methods for packing and unpacking resources
	/// </summary>
	public sealed class UnitTestHelper
	{
		private UnitTestHelper() {}

		/// <summary>
		/// Unpacks an embedded resource into the specified directory.
		/// </summary>
		/// <remarks>Omit the UnitTests.GameServer.Resources. part of the 
		/// resource name.</remarks>
		/// <param name="resourceName"></param>
		/// <param name="outputPath">The path to write the file as.</param>
		public static void UnpackEmbeddedResource(string resourceName, string outputPath)
		{
			Stream stream = UnpackEmbeddedResource(resourceName);
			using(StreamReader reader = new StreamReader(stream))
			{
				using(StreamWriter writer = File.CreateText(outputPath))
				{
					writer.Write(reader.ReadToEnd());
					writer.Flush();
				}
			}
		}

		/// <summary>
		/// Unpacks an embedded resource as a string.
		/// </summary>
		/// <remarks>Omit the UnitTests.GameServer.Resources. part of the 
		/// resource name.</remarks>
		/// <param name="resourceName"></param>
		/// <param name="encoding">The path to write the file as.</param>
		public static string UnpackEmbeddedResource(string resourceName, Encoding encoding)
		{
			Stream stream = UnpackEmbeddedResource(resourceName);
			using(StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Unpacks an embedded binary resource into the specified directory.
		/// </summary>
		/// <remarks>Omit the UnitTests.GameServer.Resources. part of the 
		/// resource name.</remarks>
		/// <param name="resourceName"></param>
		/// <param name="outputPath">The path to write the file as.</param>
		public static void UnpackEmbeddedBinaryResource(string resourceName, string outputPath)
		{
			using(Stream stream = UnpackEmbeddedResource(resourceName))
			{
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				using(FileStream outStream = File.Create(outputPath))
				{
					outStream.Write(buffer, 0, buffer.Length);
				}
			}
		}

		/// <summary>
		/// Unpacks an embedded resource into a Stream.
		/// </summary>
		/// <remarks>Omit the UnitTests.GameServer.Resources. part of the 
		/// resource name.</remarks>
		/// <param name="resourceName">Name of the resource.</param>
		public static Stream UnpackEmbeddedResource(string resourceName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			return assembly.GetManifestResourceStream("UnitTests.Subtext.Resources." + resourceName);
		}

		/// <summary>
		/// Generates a valid unique hostname (without preceding "www.").
		/// </summary>
		/// <returns></returns>
		public static string GenerateUniqueHost()
		{
			return Guid.NewGuid().ToString().Replace("-", "") + ".com";
		}

		/// <summary>
		/// Sets the HTTP context with a valid request for the blog specified 
		/// by the host and application.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="blogName">Application.</param>
		public static void SetHttpContextWithBlogRequest(string host, string blogName)
		{
			SetHttpContextWithBlogRequest(host, blogName, string.Empty);
		}

		/// <summary>
		/// Sets the HTTP context with a valid request for the blog specified 
		/// by the host and application hosted in a virtual directory.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="blogName">Application.</param>
		/// <param name="virtualDir"></param>
		public static void SetHttpContextWithBlogRequest(string host, string blogName, string virtualDir)
		{
			SetHttpContextWithBlogRequest(host, blogName, virtualDir, "default.aspx");
		}
		
		public static void SetHttpContextWithBlogRequest(string host, string blogName, string virtualDir, string page)
		{
			virtualDir = StripSlashes(virtualDir);	// Subtext.Web
			blogName = StripSlashes(blogName);		// MyBlog

			string appPhysicalDir = @"c:\projects\SubtextSystem\";	
			if(virtualDir.Length == 0)
			{
				virtualDir = "/";
			}
			else
			{
				appPhysicalDir += virtualDir + @"\";	//	c:\projects\SubtextSystem\Subtext.Web\
				virtualDir = "/" + virtualDir;			//	/Subtext.Web
			}

			if(blogName.Length > 0)
			{
				page = blogName + "/" + page;			//	MyBlog/default.aspx
				blogName = "/" + blogName;				//	/MyBlog
			}

			//page = "/" + page;							//	/MyBlog/default.aspx

			string query = string.Empty;
			TextWriter output = null;

			SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(virtualDir, appPhysicalDir, page, query, output, host);
			HttpContext.Current = new HttpContext(workerRequest);

			Console.WriteLine("host: " + host);
			Console.WriteLine("blogName: " + blogName);
			Console.WriteLine("virtualDir: " + virtualDir);
			Console.WriteLine("page: " + page);
			Console.WriteLine("appPhysicalDir: " + appPhysicalDir);
			Console.WriteLine("Request.Url.Host: " + HttpContext.Current.Request.Url.Host);
			Console.WriteLine("Request.FilePath: " + HttpContext.Current.Request.FilePath);
			Console.WriteLine("Request.Path: " + HttpContext.Current.Request.Path);
			Console.WriteLine("Request.RawUrl: " + HttpContext.Current.Request.RawUrl);
			Console.WriteLine("Request.Url: " + HttpContext.Current.Request.Url);
			Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);
		}

		/// <summary>
		/// Strips the slashes from the target string.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <returns></returns>
		public static string StripSlashes(string target)
		{
			if(target.Length == 0)
				return target;

			return target.Replace(@"\", string.Empty).Replace("/", string.Empty);
		}

		/// <summary>
		/// Strips the outer slashes.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <returns></returns>
		public static string StripOuterSlashes(string target)
		{
			if(target.Length == 0)
				return target;

			char firstChar = target[0];
			if(firstChar == '\\' || firstChar == '/')
			{
				target = target.Substring(1);
			}

			if(target.Length > 0)
			{
				char lastChar = target[target.Length - 1];
				if(lastChar == '\\' || lastChar == '/')
				{
					target = target.Substring(0, target.Length - 1);
				}	
			}
			return target;
		}

		#region ...Assert.AreNotEqual replacements...
		/// <summary>
		/// Asserts that the two values are not equal.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="compare">The compare.</param>
		public static void AssertAreNotEqual(int first, int compare)
		{
			AssertAreNotEqual(first, compare, "");
		}

		/// <summary>
		/// Asserts that the two values are not equal.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="compare">The compare.</param>
		public static void AssertAreNotEqual(int first, int compare, string message)
		{
			Assert.IsTrue(first != compare, message + "{0} is equal to {1}", first, compare);
		}

		/// <summary>
		/// Asserts that the two values are not equal.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="compare">The compare.</param>
		public static void AssertAreNotEqual(string first, string compare)
		{
			AssertAreNotEqual(first, compare, "");
		}

		/// <summary>
		/// Asserts that the two values are not equal.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="compare">The compare.</param>
		public static void AssertAreNotEqual(string first, string compare, string message)
		{
			Assert.IsTrue(first != compare, message + "{0} is equal to {1}", first, compare);
		}
		#endregion
	}
}

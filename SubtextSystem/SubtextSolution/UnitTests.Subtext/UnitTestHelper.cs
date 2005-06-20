using System;
using System.IO;
using System.Reflection;
using System.Web;

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
		/// <param name="application">Application.</param>
		public static void SetHttpContextWithBlogRequest(string host, string application)
		{
			SetHttpContextWithBlogRequest(host, application, string.Empty);
		}

		/// <summary>
		/// Sets the HTTP context with a valid request for the blog specified 
		/// by the host and application hosted in a virtual directory.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="application">Application.</param>
		/// <param name="virtualDir"></param>
		public static void SetHttpContextWithBlogRequest(string host, string application, string virtualDir)
		{
			virtualDir = virtualDir.Replace(@"\", string.Empty).Replace("/", string.Empty);
			application = application.Replace(@"\", string.Empty).Replace("/", string.Empty);

			string appPhysicalDir = @"c:\projects\SubtextSystem\";			
			if(virtualDir.Length == 0)
			{
				virtualDir = "/";
			}
			else
			{
				appPhysicalDir += virtualDir + @"\";
				virtualDir = "/" + virtualDir;
			}

			if(application.Length > 0)
			{
				application = "/" + application;
			}

			string page = virtualDir + application + "/default.aspx";
			string query = string.Empty;
			TextWriter output = null;

			SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(virtualDir, appPhysicalDir, page, query, output, host);
			HttpContext.Current = new HttpContext(workerRequest);

			Console.WriteLine("Request.FilePath: " + HttpContext.Current.Request.FilePath);
			Console.WriteLine("Request.Path: " + HttpContext.Current.Request.Path);
			Console.WriteLine("Request.RawUrl: " + HttpContext.Current.Request.RawUrl);
			Console.WriteLine("Request.Url: " + HttpContext.Current.Request.Url);
			Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);
		}
	}
}

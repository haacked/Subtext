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
			SimulatedHttpRequest workerRequest = new SimulatedHttpRequest("/", @"c:\projects\SubtextSystem\Subtext.Web\", "/" + application.Replace("/", string.Empty) + "/default.aspx", string.Empty, null, host);
			HttpContext.Current = new HttpContext(workerRequest);
			Console.WriteLine(HttpContext.Current.Request.Url);
		}
	}
}

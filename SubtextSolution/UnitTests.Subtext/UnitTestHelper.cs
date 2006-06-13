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
using System.Xml;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Contains helpful methods for packing and unpacking resources
	/// </summary>
	public static class UnitTestHelper
	{
		/// <summary>
		/// Creates a provider info instance using the specified name and type.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static ProviderInfo CreateProviderInfoInstance(string name, string type)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement element = doc.CreateElement("Root");
			doc.AppendChild(element);
			
			XmlAttribute nameAttribute = doc.CreateAttribute("name");
			nameAttribute.Value = name;
			XmlAttribute typeAttribute = doc.CreateAttribute("type");
			typeAttribute.Value = type;
			element.Attributes.Append(nameAttribute);
			element.Attributes.Append(typeAttribute);
			return new ProviderInfo(element.Attributes);
		}
		
		/// <summary>
		/// Unpacks an embedded resource into the specified directory.
		/// </summary>
		/// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
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
		/// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
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
		/// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
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
		/// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
		/// resource name.</remarks>
		/// <param name="resourceName">Name of the resource.</param>
		public static Stream UnpackEmbeddedResource(string resourceName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			return assembly.GetManifestResourceStream("UnitTests.Subtext.Resources." + resourceName);
		}

		/// <summary>
		/// Generates a random hostname.
		/// </summary>
		/// <returns></returns>
		public static string GenerateRandomHostname()
		{
			return System.Guid.NewGuid().ToString().Replace("-", "") + ".com";
		}


		/// <summary>
		/// Sets the HTTP context with a valid request for the blog specified 
		/// by the host and application.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, string.Empty);
		}

		/// <summary>
		/// Sets the HTTP context with a valid request for the blog specified 
		/// by the host and subfolder hosted in a virtual directory.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <param name="applicationPath"></param>
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, "default.aspx");
		}
		
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, page, null);
		}

		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page, TextWriter output)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, page, output, "GET");
		}
		
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page, TextWriter output, string httpVerb)
		{
			HttpContext.Current = null;
		    	    
			applicationPath = UrlFormats.StripSurroundingSlashes(applicationPath);	// Subtext.Web
			subfolder = StripSlashes(subfolder);		// MyBlog

			string appPhysicalDir = @"c:\projects\SubtextSystem\";	
			if(applicationPath.Length == 0)
			{
				applicationPath = "/";
			}
			else
			{
				appPhysicalDir += applicationPath + @"\";	//	c:\projects\SubtextSystem\Subtext.Web\
				applicationPath = "/" + applicationPath;			//	/Subtext.Web
			}

            SetHttpRequestApplicationPath(applicationPath);
		    
			if(subfolder.Length > 0)
			{
				page = subfolder + "/" + page;			//	MyBlog/default.aspx
			}

			string query = string.Empty;

            SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(applicationPath, appPhysicalDir, page, query, output, host, httpVerb);
			HttpContext.Current = new HttpContext(workerRequest);
			HttpContext.Current.Items.Clear();
			HttpContext.Current.Cache.Remove("BlogInfo-");
			HttpContext.Current.Cache.Remove("BlogInfo-" + subfolder);
			
			HttpContext.Current.Items["Subtext__CurrentRequest"] = new BlogRequest(host, subfolder);

			#region Console Debug INfo
			
			Console.WriteLine("host: " + host);
			Console.WriteLine("blogName: " + subfolder);
			Console.WriteLine("virtualDir: " + applicationPath);
			Console.WriteLine("page: " + page);
			Console.WriteLine("appPhysicalDir: " + appPhysicalDir);
			Console.WriteLine("Request.Url.Host: " + HttpContext.Current.Request.Url.Host);
			Console.WriteLine("Request.FilePath: " + HttpContext.Current.Request.FilePath);
			Console.WriteLine("Request.Path: " + HttpContext.Current.Request.Path);
			Console.WriteLine("Request.RawUrl: " + HttpContext.Current.Request.RawUrl);
			Console.WriteLine("Request.Url: " + HttpContext.Current.Request.Url);
			Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);

			#endregion

			return workerRequest;
		}
	    
	    static void SetHttpRequestApplicationPath(string applicationPath)
	    {
	        //We cheat by using reflection.
	        FieldInfo runtimeField = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static);
	        Assert.IsNotNull(runtimeField);
            HttpRuntime currentRuntime = runtimeField.GetValue(null) as HttpRuntime;
            Assert.IsNotNull(currentRuntime);
            FieldInfo appDomainAppVPathField = typeof(HttpRuntime).GetField("_appDomainAppVPath", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(appDomainAppVPathField);

            Type virtualPathType = Type.GetType("System.Web.VirtualPath, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            Assert.IsNotNull(virtualPathType);
            MethodInfo createMethod = virtualPathType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public, null, new Type[] {typeof(string)}, null);
            object virtualPath = createMethod.Invoke(null, new object[] { applicationPath });
	        
	        appDomainAppVPathField.SetValue(currentRuntime, virtualPath);
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

		/// <summary>
		/// This is useful when two strings appear to be but Assert.AreEqual says they are not.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="expected"></param>
		public static void AssertStringsEqualCharacterByCharacter(string original, string expected)
		{
			if(original != expected)
			{
				for(int i = 0; i < Math.Max(original.Length, expected.Length); i++)
				{
					char originalChar = (char)0;
					char expectedChar = (char)0;
					if(i < original.Length)
					{
						originalChar = original[i];
					}

					if(i < expected.Length)
					{
						expectedChar = expected[i];
					}

					string originalCharDisplay = "" + originalChar;
					if(char.IsWhiteSpace(originalChar))
					{
						originalCharDisplay = "{" + (int)originalChar  + "}";
					}

					string expectedCharDisplay = "" + expectedChar;
					if(char.IsWhiteSpace(expectedChar))
					{
						expectedCharDisplay = "{" + (int)expectedChar + "}";
					}

					Console.WriteLine("{0}:\t{1} ({2})\t{3} ({4})", i, originalCharDisplay, (int)originalChar, expectedCharDisplay, (int)expectedChar);
				}
				Assert.AreEqual(original, expected);
			}
		}

		/// <summary>
		/// Creates an entry instance with the proper syndication settings.
		/// </summary>
		/// <param name="author">The author.</param>
		/// <param name="body">The body.</param>
		/// <param name="title">The title.</param>
		public static Entry CreateEntryInstanceForSyndication(string author, string title, string body)
		{
			return CreateEntryInstanceForSyndication(author, title, body, null, DateTime.Now);
		}

		/// <summary>
		/// Creates an entry instance with the proper syndication settings.
		/// </summary>
		/// <param name="author">The author.</param>
		/// <param name="title">The title.</param>
		/// <param name="body">The body.</param>
		/// <param name="entryName">Name of the entry.</param>
		/// <param name="dateCreated">The date created.</param>
		/// <returns></returns>
		public static Entry CreateEntryInstanceForSyndication(string author, string title, string body, string entryName, DateTime dateCreated)
		{
			Entry entry = new Entry(PostType.BlogPost);
			if(entryName != null)
			{
				entry.EntryName = entryName;
			}
			entry.BlogId = Config.CurrentBlog.BlogId;
			entry.DateCreated = dateCreated;
			entry.DateUpdated = entry.DateCreated;
			entry.DateSyndicated = entry.DateCreated;
			entry.Title = title;
			entry.Author = author;
			entry.Body = body;
			entry.DisplayOnHomePage = true;
			entry.IsAggregated = true;
			entry.IsActive = true;
			entry.AllowComments = true;
			entry.IncludeInMainSyndication = true;
			
			return entry;
		}

		public static string ExtractArchiveToString(Stream compressedArchive)
		{
			StringBuilder target = new StringBuilder();
			using(ZipInputStream inputStream = new ZipInputStream(compressedArchive))
			{
				ZipEntry nextEntry = inputStream.GetNextEntry();
				
				while(nextEntry != null)
				{
					target.Append(Extract(inputStream));
					nextEntry = inputStream.GetNextEntry();
				}
			}
			return target.ToString();
		}

		public static string Extract(ZipInputStream inputStream)
		{
			MemoryStream output = new MemoryStream();
			
			byte[] buffer = new byte[4096];
			int count = inputStream.Read(buffer, 0, 4096);
			while(count > 0)
			{
				output.Write(buffer, 0, count);
				count = inputStream.Read(buffer, 0, 4096);
			}
			
			byte[] bytes = output.ToArray();
			return Encoding.UTF8.GetString(bytes);
		}

		public static void ExtractArchive(Stream compressedArchive, string targetDirectory)
		{
			using(ZipInputStream inputStream = new ZipInputStream(compressedArchive))
			{
				ZipEntry nextEntry = inputStream.GetNextEntry();
				if(!Directory.Exists(targetDirectory))
				{
					Directory.CreateDirectory(targetDirectory);
				}
				while(nextEntry != null)
				{
					if(nextEntry.IsDirectory)
					{
						Directory.CreateDirectory(Path.Combine(targetDirectory, nextEntry.Name));
					}
					else
					{
						if(!Directory.Exists(Path.Combine(targetDirectory, Path.GetDirectoryName(nextEntry.Name))))
						{
							Directory.CreateDirectory(Path.Combine(targetDirectory, Path.GetDirectoryName(nextEntry.Name)));
						}

						ExtractFile(targetDirectory, nextEntry, inputStream);						
					}
					nextEntry = inputStream.GetNextEntry();
				}
			}
		}

		private static void ExtractFile(string targetDirectory, ZipEntry nextEntry, ZipInputStream inputStream)
		{
			using(FileStream fileStream = new FileStream(Path.Combine(targetDirectory, nextEntry.Name), FileMode.OpenOrCreate, FileAccess.Write))
			{
				byte[] buffer = new byte[4096];
				int count = inputStream.Read(buffer, 0, 4096);
				while(count > 0)
				{
					fileStream.Write(buffer, 0, count);
					count = inputStream.Read(buffer, 0, 4096);
				}
				fileStream.Flush();
			}
		}

		/// <summary>
		/// Returns a deflated version of the response sent by the web server. If the 
		/// web server did not send a compressed stream then the original stream is returned. 
		/// </summary>
		/// <param name="encoding">Encoding of the stream. One of 'deflate' or 'gzip' or Empty.</param>
		/// <param name="inputStream">Input Stream</param>
		/// <returns>Seekable Stream</returns>
		public static Stream GetDeflatedResponse(string encoding, Stream inputStream)
		{
			//BORROWED FROM RSS BANDIT.
			const int BUFFER_SIZE = 4096;	// 4K read buffer

			Stream compressed = null, input = inputStream; 
			bool tryAgainDeflate = true;
			
			if (input.CanSeek)
				input.Seek(0, SeekOrigin.Begin);

			if (encoding=="deflate") 
			{	//to solve issue "invalid checksum" exception with dasBlog and "deflate" setting:
				//input = ResponseToMemory(input);			// need them within mem to have a seekable stream
				compressed = new InflaterInputStream(input);	// try deflate with headers
			}
			else if (encoding=="gzip") 
			{
				compressed = new GZipInputStream(input);
			}

			retry_decompress:			
				if (compressed != null) 
				{
			
					MemoryStream decompressed = new MemoryStream();

					try 
					{

						int size = BUFFER_SIZE;
						byte[] writeData = new byte[BUFFER_SIZE];
						while (true) 
						{
							size = compressed.Read(writeData, 0, size);
							if (size > 0) 
							{
								decompressed.Write(writeData, 0, size);
							} 
							else 
							{
								break;
							}
						}
					} 
					catch (ICSharpCode.SharpZipLib.GZip.GZipException) 
					{
						if (tryAgainDeflate && (encoding=="deflate")) 
						{
							input.Seek(0, SeekOrigin.Begin);	// reset position
							compressed = new InflaterInputStream(input, new ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true));
							tryAgainDeflate = false;
							goto retry_decompress;
						} 
						else
							throw;
					}
				
					//reposition to beginning of decompressed stream then return
					decompressed.Seek(0, SeekOrigin.Begin);
					return decompressed;
				}
				else
				{
					// allready seeked, just return
					return input;
				}
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

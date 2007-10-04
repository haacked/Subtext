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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MbUnit.Framework;
using Rhino.Mocks;
using SubSonic;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Security;
using Subtext.Framework.Text;
using Subtext.Framework.Web.HttpModules;
using Subtext.TestLibrary;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Contains helpful methods for packing and unpacking resources
	/// </summary>
	public static class UnitTestHelper
	{
		internal static string MembershipTestUsername
        {
            get
            {
                return GenerateRandomString();
            }
        }
	    internal static string MembershipTestEmail
	    {
	        get
	        {
                return MembershipTestUsername + "@example.com";
	        }
	    }
		internal static readonly string MembershipTestPassword = GenerateRandomString();

		public static void ClearAllBlogData()
		{
			string[] tables = new string[]
				{
					"subtext_PluginData"
					, "subtext_PluginBlog"
					, "subtext_KeyWords"	
					,"subtext_Images"	
					,"subtext_Links"
					,"subtext_EntryViewCount"
					,"subtext_Log"
					,"subtext_Feedback"
					,"subtext_EntryTag"
					,"subtext_Tag"
					,"subtext_Content"
					,"subtext_LinkCategories"
					,"subtext_Config"
					,"subtext_Referrals"
					,"subtext_URLs"
				};

			foreach(string tableName in tables)
			{
				QueryCommand command = new QueryCommand(string.Format("DELETE [{0}]", tableName));
				DataService.ExecuteQuery(command);
			}
		}

		/// <summary>
		/// Unpacks an embedded resource as a string. The resource name should 
		/// be everything after 'UnitTests.Subtext.Resources.'.
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
		/// Unpacks an embedded resource into a Stream.  The resource name should 
		/// be everything after 'UnitTests.Subtext.Resources.'.
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
		internal static string GenerateRandomString()
		{
			return Guid.NewGuid().ToString().Replace("-", "") + ".com";
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context 
		/// with the blog.
		/// </summary>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated 
		/// requests.
		/// </returns>
		internal static SimulatedRequestContext SetupBlog()
		{
			return SetupBlog(string.Empty);
		}
		
		internal static MembershipUser CreateUserInstanceForTest()
		{
			return new MembershipUser("SubtextMembershipProvider", "Phil Haack", Guid.Empty, "test@example.com", "comment", "comment", true, false, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated 
		/// requests.
		/// </returns>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		internal static SimulatedRequestContext SetupBlog(string subfolder)
		{
			return SetupBlog(subfolder, string.Empty);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated 
		/// requests.
		/// </returns>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		/// <param name="applicationPath">The name of the IIS virtual directory the blog lives in.</param>
		internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath)
		{
			return SetupBlog(subfolder, applicationPath, 80);
		}
		
		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated 
		/// requests.
		/// </returns>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		/// <param name="applicationPath">The name of the IIS virtual directory the blog lives in.</param>
		/// <param name="port">The port for this blog.</param>
		internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, int port)
		{
			return SetupBlog(subfolder, applicationPath, port, string.Empty);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		/// <param name="applicationPath">The name of the IIS virtual directory the blog lives in.</param>
		/// <param name="page">The page to request.</param>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated
		/// requests.
		/// </returns>
		internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, string page)
		{
			return SetupBlog(subfolder, applicationPath, 80, page);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		/// <param name="applicationPath">The name of the IIS virtual directory the blog lives in.</param>
		/// <param name="port">The port for this blog.</param>
		/// <param name="page">The page to request.</param>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated
		/// requests.
		/// </returns>
		internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, int port, string page)
		{
			return SetupBlog(subfolder, applicationPath, port, page, MembershipTestUsername, MembershipTestPassword);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.
		/// </summary>
		/// <param name="subfolder">The 'virtualized' subfolder the blog lives in.</param>
		/// <param name="applicationPath">The name of the IIS virtual directory the blog lives in.</param>
		/// <param name="port">The port for this blog.</param>
		/// <param name="page">The page to request.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>
		/// Returns a reference to a string builder.
		/// The stringbuilder will end up containing the Response of any simulated
		/// requests.
		/// </returns>
		internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, int port, string page, string userName, string password)
		{
			string host = GenerateRandomString();

			MembershipUser owner = CreateMembershipUser(userName, password);

			HttpContext.Current = null;
			Assert.IsNotNull(Config.CreateBlog("Unit Test Blog", host, subfolder, owner), "Could Not Create Blog");

			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			SimulatedHttpRequest request = SetHttpContextWithBlogRequest(host, port, subfolder, applicationPath, page, output, "GET");

			if (Config.CurrentBlog != null)
			{
				Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
				Config.CurrentBlog.ImageDirectory = Path.Combine(Environment.CurrentDirectory, "image") + Path.DirectorySeparatorChar;
				Config.CurrentBlog.ImagePath = "/image/";
			}
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName), new string[] { RoleNames.Administrators });

			return new SimulatedRequestContext(request, sb, output, host);
		}

		public static MembershipUser CreateMembershipUser()
		{
			return CreateMembershipUser(GenerateRandomString(), GenerateRandomString());
		}

		public static MembershipUser CreateMembershipUser(string userName, string password)
		{
			MembershipCreateStatus status;
			MembershipUser owner = Membership.CreateUser(userName, password, MembershipTestEmail, "What time is it?", "It's Subtext Time!", true, out status);
			Assert.AreEqual(status, MembershipCreateStatus.Success, "User was unable not created");
			Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(owner.UserName), new string[] { RoleNames.Administrators });
			return owner;
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.  The blog will have an admin with the specified 
		/// username and password.
		/// </summary>
		public static void SetupBlogWithUserAndPassword(string username, string password)
		{
			SetupBlogWithUserAndPassword(username, password, string.Empty);
		}

		/// <summary>
		/// Takes all the necessary steps to create a blog and set up the HTTP Context
		/// with the blog.  The blog will have an admin with the specified 
		/// username and password.
		/// </summary>
		public static void SetupBlogWithUserAndPassword(string username, string password, string subfolder)
		{
			SetupBlog(subfolder, string.Empty, 80, string.Empty, username, password);
		}

		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, string.Empty, string.Empty);
		}

		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, string.Empty);
		}

		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page)
	    {
	        return SetHttpContextWithBlogRequest(host, 80, subfolder, applicationPath, page);
	    }
	    
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, int port, string subfolder, string applicationPath, string page)
		{
			return SetHttpContextWithBlogRequest(host, port, subfolder, applicationPath, page, null, "GET");
		}

		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page, TextWriter output)
		{
			return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, page, output, "GET");
		}
		
	    public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder, string applicationPath, string page, TextWriter output, string httpVerb)
	    {
            return SetHttpContextWithBlogRequest(host, 80, subfolder, applicationPath, page, output, httpVerb);
	    }
	    
		public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, int port, string subfolder, string applicationPath, string page, TextWriter output, string httpVerb)
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

            SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(applicationPath, appPhysicalDir, Path.Combine(appPhysicalDir, page), page, query, output, host, port, httpVerb);
			HttpContext.Current = new HttpContext(workerRequest);
			HttpContext.Current.Items.Clear();
			HttpContext.Current.Cache.Remove("BlogInfo-");
			HttpContext.Current.Cache.Remove("BlogInfo-" + subfolder);

			Type appFactoryType = Type.GetType("System.Web.HttpApplicationFactory, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			Assert.IsNotNull(appFactoryType, "The HttpApplicationFactory type is null");
			object appFactory = ReflectionHelper.GetStaticFieldValue<object>("_theApplicationFactory", appFactoryType);
			ReflectionHelper.SetPrivateInstanceFieldValue("_state", appFactory, HttpContext.Current.Application);
			
			BlogRequest.Current = new BlogRequest(host, subfolder, HttpContext.Current.Request.Url, host == "localhost");

			#region Console Debug INfo
			/*
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
            Console.WriteLine("Request.Url.Port: " + HttpContext.Current.Request.Url.Port);
			Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);
			*/
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
                int unequalPos = 0;
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

                    if (unequalPos == 0 && originalChar != expectedChar)
				        unequalPos = i;

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
				Assert.AreEqual(original, expected, "Strings are not equal starting at character {0}", unequalPos);
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
			entry.BlogId = Config.CurrentBlog.Id;
			entry.DateCreated = entry.DateModified = entry.DateSyndicated = dateCreated;
			entry.Title = title;
			entry.Author = CreateUserInstanceForTest();
			entry.Body = body;
			entry.DisplayOnHomePage = true;
			entry.IsAggregated = true;
			entry.IsActive = true;
			entry.AllowComments = true;
			entry.IncludeInMainSyndication = true;
			
			return entry;
		}

		public static Link CreateLinkInDb(int categoryId, string title, int? entryId)
		{
			Link link = new Link();
			link.BlogId = Config.CurrentBlog.Id;
			link.IsActive = true;
			link.CategoryID = categoryId;
			link.Title = title;
			link.Url = "http://noneofyourbusiness.com/";
			if (entryId != null)
				link.PostID = (int)entryId;
			link.Id = Links.CreateLink(link);
			return link;
		}
		
		/// <summary>
		/// Creates an entry instance with the proper syndication settings.
		/// </summary>
		/// <param name="parentEntryId">The parent entry.</param>
		/// <param name="author">The author.</param>
		/// <param name="title">The title.</param>
		/// <param name="body">The body.</param>
		/// <param name="dateCreated">The date created.</param>
		/// <returns></returns>
		public static FeedbackItem CreateCommentInstance(int parentEntryId, string author, string title, string body, DateTime dateCreated)
		{
			FeedbackItem entry = new FeedbackItem(FeedbackType.Comment);
			entry.SourceUrl = new Uri("http://subtextproject.com/blah/");
			entry.BlogId = Config.CurrentBlog.Id;
			entry.EntryId = parentEntryId;
			entry.DateCreated = dateCreated;
			entry.DateModified = entry.DateCreated;
			entry.Title = title;
			entry.Author = author;
			entry.Body = body;
			entry.Approved = true;

			return entry;
		}

		public static void CreateBlog(string title, string username, string email, string password, string hostName, string subfolder)
		{
			MembershipUser owner = CreateMembershipUser();
			Config.CreateBlog("title", hostName, subfolder, owner);
		}

		/// <summary>
	    /// Creates a blog post link category.
	    /// </summary>
	    /// <param name="blogId"></param>
	    /// <param name="title"></param>
	    /// <returns></returns>
	    public static int CreateCategory(int blogId, string title)
	    {
			return CreateCategory(blogId, title, CategoryType.PostCollection);
	    }

		/// <summary>
		/// Creates a blog post link category.
		/// </summary>
		/// <param name="blogId">The blog id.</param>
		/// <param name="title">The title.</param>
		/// <param name="categoryType">Type of the category.</param>
		/// <returns></returns>
		public static int CreateCategory(int blogId, string title, CategoryType categoryType)
		{
			LinkCategory category = new LinkCategory();
			category.BlogId = Config.CurrentBlog.Id;
			category.Title = title;
			category.CategoryType = categoryType;
			category.IsActive = true;
			return Links.CreateLinkCategory(category);
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
	    /// This method will read the FormsAuthentication cookie from HttpContext 
	    /// and then set the HttpContext User with the roles determined by the cookie.
	    /// 
	    /// TODO: This code is heavily based on the AuthenticationModule code... would 
	    /// be nice if we could figure out a way to share that same code, rather than 
	    /// having duplicate code.
	    /// </summary>
        public static void AuthenticateFormsAuthenticationCookie()
        {
			string cookieName = SecurityHelper.GetFullCookieName();
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

	    	if(authCookie == null)
	    		return;
	    	
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            // When the ticket was created, the UserData property was assigned a
            // pipe delimited string of role names.
            string[] roles = authTicket.UserData.Split(new char[] { '|' });
            // Create an Identity object
            FormsIdentity id = new FormsIdentity(authTicket);

            // This principal will flow throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, roles);
            // Attach the new principal object to the current HttpContext object
            HttpContext.Current.User = principal;
        }

		/// <summary>
		/// Useful for unit testing that classes implement serialization.  This simply takes in a class, 
		/// serializes it into a byte array, deserializes the byte array, and returns the result. 
		/// The unit test should check that all the properties are set correctly.
		/// </summary>
		/// <param name="serializableObject">The serializable object.</param>
		/// <returns></returns>
		public static T SerializeRoundTrip<T>(T serializableObject)
		{
			MemoryStream stream = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, serializableObject);
			byte[] serialized = stream.ToArray();
			
			stream = new MemoryStream(serialized);
			stream.Position = 0;
			formatter = new BinaryFormatter();
			object o = formatter.Deserialize(stream);
			return (T)o;
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
					catch (GZipException) 
					{
						if (tryAgainDeflate && (encoding=="deflate")) 
						{
							input.Seek(0, SeekOrigin.Begin);	// reset position
							compressed = new InflaterInputStream(input, new Inflater(true));
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

		/// <summary>
		/// Sets all public read/write properties to have a 
		/// property behavior when using Rhino Mocks.
		/// </summary>
		/// <param name="mock"></param>
		public static void SetPropertyBehaviorOnAllProperties(object mock)
		{
		  PropertyInfo[] properties = mock.GetType().GetProperties();
		  foreach (PropertyInfo property in properties)
		  {
			if (property.CanRead && property.CanWrite)
			{
			  property.GetValue(mock, null);
			  LastCall.On(mock).PropertyBehavior();
			}
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
	    /// Makes sure we can read app settings
	    /// </summary>
	    public static void AssertAppSettings()
	    {
            Assert.AreEqual("UnitTestValue", ConfigurationManager.AppSettings["UnitTestKey"], "Cannot read app settings");
	    }

		/// <summary>
		/// Asserts that the two values are not equal.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="compare">The compare.</param>
		/// <param name="message">The message.</param>
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
		/// <param name="message">The message.</param>
		public static void AssertAreNotEqual(string first, string compare, string message)
		{
			Assert.IsTrue(first != compare, message + "{0} is equal to {1}", first, compare);
		}
		#endregion

	    public static BlogInfo CreateBlogAndSetupContext()
	    {
	    	SetupBlog();
	    	return Config.CurrentBlog;
	    }

		public static BlogAlias CreateBlogAlias(BlogInfo info, string host, string subfolder)
		{
			return CreateBlogAlias(info, host, subfolder, true);
		}
		public static BlogAlias CreateBlogAlias(BlogInfo info, string host, string subfolder, bool active)
		{
			BlogAlias alias = new BlogAlias();
			alias.BlogId = info.Id;
			alias.Host = host;
			alias.Subfolder = subfolder;
			alias.IsActive = active;

			Config.AddBlogAlias(alias);
			return alias;
		}

	    public static MetaTag BuildMetaTag(string content, string name, string httpEquiv, int blogId, int? entryId, DateTime created)
	    {
	        MetaTag mt = new MetaTag();
	        mt.Name = name;
	        mt.HttpEquiv = httpEquiv;
	        mt.Content = content;
	        mt.BlogId = blogId;

	        if (entryId.HasValue)
	            mt.EntryId = entryId.Value;

	        mt.DateCreated = created;

	        return mt;
	    }

        public static IList<MetaTag> BuildMetaTagsFor(BlogInfo blog, Entry entry, int numberOfTags)
        {
            List<MetaTag> tags = new List<MetaTag>(numberOfTags);

            int? entryId = null;
            if (entry != null)
                entryId = entry.Id;
            
            for (int i = 0; i < numberOfTags; i++)
            {
                MetaTag aTag = BuildMetaTag(
                    StringHelper.Left(GenerateRandomString(), 50),
                    // if even, make a name attribute, else http-equiv
                    (i%2 == 0) ? StringHelper.Left(GenerateRandomString(), 25) : null,
                    (i%2 == 1) ? StringHelper.Left(GenerateRandomString(), 25) : null,
                    blog.Id,
                    entryId,
                    DateTime.Now);

                tags.Add(aTag);
            }

            return tags;
        }

		/// <summary>
		/// Helper method. Makes sure you can create
		/// </summary>
		/// <param name="allowedRoles">The allowed roles.</param>
		/// <param name="constructorArguments">The constructor arguments.</param>
		public static void AssertSecureCreation<T>(string[] allowedRoles, params object[] constructorArguments)
		{
			try
			{
				Activator.CreateInstance(typeof(T), constructorArguments);
				Assert.Fail("Was able to create the instance with no security.");
			}
			catch(TargetInvocationException e)
			{
				Assert.IsInstanceOfType(typeof(SecurityException), e.InnerException, "Expected a security exception, got something else.");
			}

			MockRepository mocks = new MockRepository();

			IPrincipal principal;
			SetCurrentPrincipalRoles(mocks, out principal, allowedRoles);

			using (mocks.Playback())
			{
				IPrincipal oldPrincipal = Thread.CurrentPrincipal;
				try
				{
					Thread.CurrentPrincipal = principal;
					Activator.CreateInstance(typeof(T), constructorArguments);
					//Test passes if no exception is thrown.
				}
				finally
				{
					Thread.CurrentPrincipal = oldPrincipal;
				}
			}
		}

		public static void SetCurrentPrincipalRoles(MockRepository mocks, out IPrincipal principal, params string[] roles)
		{
			using (mocks.Record())
			{
				
				IIdentity identity = mocks.CreateMock<IIdentity>();
				SetupResult.For(identity.IsAuthenticated).Return(true);
				IPrincipal user = mocks.CreateMock<IPrincipal>();
				SetupResult.For(user.Identity).Return(identity);
				Array.ForEach(roles, delegate(string role)
             	{
					SetupResult.For(user.IsInRole(role)).Return(true);		
             	});
				principal = user;
			}
		}
	}
}

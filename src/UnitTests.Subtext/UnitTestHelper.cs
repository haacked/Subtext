#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Moq;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Emoticons;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Framework.Web.HttpModules;
using Subtext.Infrastructure;

namespace UnitTests.Subtext
{
    /// <summary>
    /// Contains helpful methods for packing and unpacking resources
    /// </summary>
    public static class UnitTestHelper
    {
        /// <summary>
        /// Unpacks an embedded resource into the specified directory. The resource name should 
        /// be everything after 'UnitTests.Subtext.Resources.'.
        /// </summary>
        /// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
        /// resource name.</remarks>
        /// <param name="resourceName"></param>
        /// <param name="outputPath">The path to write the file as.</param>
        public static void UnpackEmbeddedResource(string resourceName, string outputPath)
        {
            Stream stream = UnpackEmbeddedResource(resourceName);
            using (var reader = new StreamReader(stream))
            {
                using (StreamWriter writer = File.CreateText(outputPath))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                }
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
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Unpacks an embedded binary resource into the specified directory. The resource name should 
        /// be everything after 'UnitTests.Subtext.Resources.'.
        /// </summary>
        /// <remarks>Omit the UnitTests.Subtext.Resources. part of the 
        /// resource name.</remarks>
        /// <param name="resourceName"></param>
        /// <param name="fileName">The file to write the resourcce.</param>
        public static string UnpackEmbeddedBinaryResource(string resourceName, string fileName)
        {
            using (Stream stream = UnpackEmbeddedResource(resourceName))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                string filePath = !Path.IsPathRooted(fileName) ? GetPathInExecutingAssemblyLocation(fileName) : Path.GetFullPath(fileName);
                using (FileStream outStream = File.Create(filePath))
                {
                    outStream.Write(buffer, 0, buffer.Length);
                }
                return filePath;
            }
        }

        public static string GetPathInExecutingAssemblyLocation(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
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
        /// Generates a unique string.
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// Generates a unique host name.
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueHostname()
        {
            return GenerateUniqueString() + ".com";
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
        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder,
                                                                         string applicationPath)
        {
            return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, "default.aspx");
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, int port, string subfolder,
                                                                         string applicationPath)
        {
            return SetHttpContextWithBlogRequest(host, port, subfolder, applicationPath, "default.aspx");
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder,
                                                                         string applicationPath, string page)
        {
            return SetHttpContextWithBlogRequest(host, 80, subfolder, applicationPath, page);
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, int port, string subfolder,
                                                                         string applicationPath, string page)
        {
            return SetHttpContextWithBlogRequest(host, port, subfolder, applicationPath, page, null, "GET");
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder,
                                                                         string applicationPath, string page,
                                                                         TextWriter output)
        {
            return SetHttpContextWithBlogRequest(host, subfolder, applicationPath, page, output, "GET");
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, string subfolder,
                                                                         string applicationPath, string page,
                                                                         TextWriter output, string httpVerb)
        {
            return SetHttpContextWithBlogRequest(host, 80, subfolder, applicationPath, page, output, httpVerb);
        }

        public static SimulatedHttpRequest SetHttpContextWithBlogRequest(string host, int port, string subfolder,
                                                                         string applicationPath, string page,
                                                                         TextWriter output, string httpVerb)
        {
            HttpContext.Current = null;

            applicationPath = HttpHelper.StripSurroundingSlashes(applicationPath); // Subtext.Web
            subfolder = StripSlashes(subfolder); // MyBlog

            string appPhysicalDir = @"c:\projects\SubtextSystem\";
            if (applicationPath.Length == 0)
            {
                applicationPath = "/";
            }
            else
            {
                appPhysicalDir += applicationPath + @"\"; //	c:\projects\SubtextSystem\Subtext.Web\
                applicationPath = "/" + applicationPath; //	/Subtext.Web
            }

            SetHttpRequestApplicationPath(applicationPath);

            if (subfolder.Length > 0)
            {
                page = subfolder + "/" + page; //	MyBlog/default.aspx
            }

            string query = string.Empty;

            var workerRequest = new SimulatedHttpRequest(applicationPath, appPhysicalDir, appPhysicalDir + page, page,
                                                         query, output, host, port, httpVerb);
            HttpContext.Current = new HttpContext(workerRequest);
            BlogRequest.Current = new BlogRequest(host, subfolder, HttpContext.Current.Request.Url, host == "localhost");

            return workerRequest;
        }

        static void SetHttpRequestApplicationPath(string applicationPath)
        {
            //We cheat by using reflection.
            FieldInfo runtimeField = typeof(HttpRuntime).GetField("_theRuntime",
                                                                  BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(runtimeField);
            var currentRuntime = runtimeField.GetValue(null) as HttpRuntime;
            Assert.IsNotNull(currentRuntime);
            FieldInfo appDomainAppVPathField = typeof(HttpRuntime).GetField("_appDomainAppVPath",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.Instance);
            Assert.IsNotNull(appDomainAppVPathField);

            Type virtualPathType =
                Type.GetType(
                    "System.Web.VirtualPath, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                    true);
            Assert.IsNotNull(virtualPathType);
            MethodInfo createMethod = virtualPathType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public,
                                                                null, new[] { typeof(string) }, null);
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
            if (target.Length == 0)
            {
                return target;
            }

            return target.Replace(@"\", string.Empty).Replace("/", string.Empty);
        }

        /// <summary>
        /// Strips the outer slashes.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <returns></returns>
        public static string StripOuterSlashes(string target)
        {
            if (target.Length == 0)
            {
                return target;
            }

            char firstChar = target[0];
            if (firstChar == '\\' || firstChar == '/')
            {
                target = target.Substring(1);
            }

            if (target.Length > 0)
            {
                char lastChar = target[target.Length - 1];
                if (lastChar == '\\' || lastChar == '/')
                {
                    target = target.Substring(0, target.Length - 1);
                }
            }
            return target;
        }

        /// <summary>
        /// This is useful when two strings appear to be but Assert.AreEqual says they are not.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="expected"></param>
        public static void AssertStringsEqualCharacterByCharacter(string expected, string result)
        {
            if (result != expected)
            {
                int unequalPos = 0;
                for (int i = 0; i < Math.Max(result.Length, expected.Length); i++)
                {
                    var originalChar = (char)0;
                    var expectedChar = (char)0;
                    if (i < result.Length)
                    {
                        originalChar = result[i];
                    }

                    if (i < expected.Length)
                    {
                        expectedChar = expected[i];
                    }

                    if (unequalPos == 0 && originalChar != expectedChar)
                    {
                        unequalPos = i;
                    }

                    string expectedCharText = "" + originalChar;
                    if (char.IsWhiteSpace(originalChar))
                    {
                        expectedCharText = "{" + (int)originalChar + "}";
                    }

                    string expectedCharDisplay = "" + expectedChar;
                    if (char.IsWhiteSpace(expectedChar))
                    {
                        expectedCharDisplay = "{" + (int)expectedChar + "}";
                    }

                    Console.WriteLine("{0}:\t{1} ({2})\t{3} ({4})", i, expectedCharDisplay, (int)expectedChar,
                                      expectedCharText, (int)originalChar);
                }
                int snippetLength = 40;
                string sourceSnippet = result.Substring(unequalPos, Math.Min(snippetLength, expected.Length - unequalPos));
                string expectedSnippet = expected.Substring(unequalPos, Math.Min(snippetLength, expected.Length - unequalPos));

                Assert.AreEqual(expectedSnippet, sourceSnippet, "Strings are not equal starting at character {0}.", unequalPos);
            }
        }

        public static Entry CreateEntryInstanceForSyndication(string author, string title, string body)
        {
            return CreateEntryInstanceForSyndication(Config.CurrentBlog, author, title, body);
        }

        public static Entry CreateEntryInstanceForSyndication(Blog blog, string author, string title, string body)
        {
            return CreateEntryInstanceForSyndication(blog, author, title, body, null, DateTime.UtcNow, DateTime.UtcNow);
        }

        public static Entry CreateEntryInstanceForSyndication(string author, string title, string body, string entryName,
                                                              DateTime dateCreatedUtc)
        {
            return CreateEntryInstanceForSyndication(Config.CurrentBlog, author, title, body, entryName, dateCreatedUtc);
        }

        public static Entry CreateEntryInstanceForSyndication(string author, string title, string body, string entryName,
                                                              DateTime dateCreatedUtc, DateTime datePublishedUtc)
        {
            return CreateEntryInstanceForSyndication(Config.CurrentBlog, author, title, body, entryName, dateCreatedUtc, datePublishedUtc);
        }

        public static Entry CreateEntryInstanceForSyndication(Blog blog, string author, string title, string body,
                                                              string entryName, DateTime dateCreatedUtc)
        {
            return CreateEntryInstanceForSyndication(blog, author, title, body, entryName, dateCreatedUtc, NullValue.NullDateTime);
        }

        public static Entry CreateEntryInstanceForSyndication(Blog blog, string author, string title, string body,
                                                              string entryName, DateTime dateCreatedUtc, DateTime datePublishedUtc)
        {
            var entry = new Entry(PostType.BlogPost);
            if (entryName != null)
            {
                entry.EntryName = entryName;
            }
            entry.BlogId = blog.Id;
            if (dateCreatedUtc != NullValue.NullDateTime)
            {
                if (dateCreatedUtc.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException("DateCreated must be UTC");
                }
                if (!datePublishedUtc.IsNull() && datePublishedUtc.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException("DatePublished must be UTC");
                }

                entry.DateCreatedUtc = dateCreatedUtc;
                entry.DateModifiedUtc = entry.DateCreatedUtc;
                entry.DatePublishedUtc = datePublishedUtc;
            }
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

        public static Entry CreateAndSaveEntryForSyndication(string author, string title, string body, string entryName, DateTime dateCreatedUtc, DateTime datePublishedUtc)
        {
            var entry = UnitTestHelper.CreateEntryInstanceForSyndication(author, title, body, entryName, dateCreatedUtc, datePublishedUtc);
            UnitTestHelper.Create(entry);
            return entry;
        }

        public static Link CreateLinkInDb(int categoryId, string title, int? entryId, string rel)
        {
            var link = new Link
            {
                BlogId = Config.CurrentBlog.Id,
                IsActive = true,
                CategoryId = categoryId,
                Title = title,
                Url = "http://noneofyourbusiness.com/",
                Relation = rel
            };
            if (entryId != null)
            {
                link.PostId = (int)entryId;
            }
            link.Id = Links.CreateLink(link);
            return link;
        }

        /// <summary>
        /// Creates a blog post link category.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static int CreateCategory(int blogId, string title)
        {
            var category = new LinkCategory
            {
                BlogId = Config.CurrentBlog.Id,
                Title = title,
                CategoryType = CategoryType.PostCollection,
                IsActive = true
            };
            return new DatabaseObjectProvider().CreateLinkCategory(category);
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
            var category = new LinkCategory
            {
                BlogId = Config.CurrentBlog.Id,
                Title = title,
                CategoryType = categoryType,
                IsActive = true
            };
            return new DatabaseObjectProvider().CreateLinkCategory(category);
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
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, serializableObject);
            byte[] serialized = stream.ToArray();

            stream = new MemoryStream(serialized) { Position = 0 };
            formatter = new BinaryFormatter();
            object o = formatter.Deserialize(stream);
            return (T)o;
        }

        public static Blog CreateBlogAndSetupContext(string hostName = null, string subfolder = "")
        {
            hostName = hostName ?? GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("Just A Test Blog", "test", "test", hostName, subfolder /* subfolder */);
            Blog blog = repository.GetBlog(hostName, subfolder);
            SetHttpContextWithBlogRequest(hostName, subfolder);
            BlogRequest.Current.Blog = blog;
            Assert.IsNotNull(Config.CurrentBlog, "Current Blog is null.");

            // NOTE- is this OK?
            return Config.CurrentBlog;
        }

        public static BlogAlias CreateBlogAlias(Blog info, string host, string subfolder)
        {
            return CreateBlogAlias(info, host, subfolder, true);
        }

        public static BlogAlias CreateBlogAlias(Blog info, string host, string subfolder, bool active)
        {
            var alias = new BlogAlias { BlogId = info.Id, Host = host, Subfolder = subfolder, IsActive = active };

            new global::Subtext.Framework.Data.DatabaseObjectProvider().AddBlogAlias(alias);
            return alias;
        }

        public static MetaTag BuildMetaTag(string content, string name, string httpEquiv, int blogId, int? entryId,
                                           DateTime created)
        {
            if (created.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The create date must me UTC", "created");
            }

            var mt = new MetaTag { Name = name, HttpEquiv = httpEquiv, Content = content, BlogId = blogId };

            if (entryId.HasValue)
            {
                mt.EntryId = entryId.Value;
            }

            mt.DateCreatedUtc = created;

            return mt;
        }

        public static ICollection<MetaTag> BuildMetaTagsFor(Blog blog, Entry entry, int numberOfTags)
        {
            var tags = new List<MetaTag>(numberOfTags);

            int? entryId = null;
            if (entry != null)
            {
                entryId = entry.Id;
            }

            for (int i = 0; i < numberOfTags; i++)
            {
                MetaTag aTag = BuildMetaTag(
                    GenerateUniqueString().Left(50),
                    // if even, make a name attribute, else http-equiv
                    (i % 2 == 0) ? GenerateUniqueString().Left(25) : null,
                    (i % 2 == 1) ? GenerateUniqueString().Left(25) : null,
                    blog.Id,
                    entryId,
                    DateTime.UtcNow);

                tags.Add(aTag);
            }

            return tags;
        }

        public static Enclosure BuildEnclosure(string title, string url, string mimetype, int entryId, long size, bool addToFeed, bool showWithPost)
        {
            var enc = new Enclosure
            {
                EntryId = entryId,
                Title = title,
                Url = url,
                Size = size,
                MimeType = mimetype,
                ShowWithPost = showWithPost,
                AddToFeed = addToFeed
            };
            return enc;
        }

        public static void AssertSimpleProperties(object o, params string[] excludedProperties)
        {
            var excludes = new StringDictionary();
            foreach (string exclude in excludedProperties)
            {
                excludes.Add(exclude, "");
            }

            Type t = o.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo property in props)
            {
                if (excludes.ContainsKey(property.Name))
                {
                    continue;
                }

                if (property.CanRead && property.CanWrite)
                {
                    object valueToSet;
                    if (property.PropertyType == typeof(int)
                       || property.PropertyType == typeof(short)
                       || property.PropertyType == typeof(decimal)
                       || property.PropertyType == typeof(double)
                       || property.PropertyType == typeof(long))
                    {
                        valueToSet = 42;
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        valueToSet = "This Is a String";
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        valueToSet = DateTime.UtcNow;
                    }
                    else if (property.PropertyType == typeof(Uri))
                    {
                        valueToSet = new Uri("http://subtextproject.com/");
                    }
                    else if (property.PropertyType == typeof(IPAddress))
                    {
                        valueToSet = IPAddress.Parse("127.0.0.1");
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        valueToSet = true;
                    }
                    else if (property.PropertyType == typeof(PageType))
                    {
                        valueToSet = PageType.HomePage;
                    }
                    else if (property.PropertyType == typeof(ICollection<Link>))
                    {
                        valueToSet = new List<Link>();
                    }
                    else if (property.PropertyType == typeof(ICollection<Image>))
                    {
                        valueToSet = new List<Image>();
                    }
                    else
                    {
                        //Don't know what to do.
                        continue;
                    }

                    property.SetValue(o, valueToSet, null);
                    object retrievedValue = property.GetValue(o, null);
                    Assert.AreEqual(valueToSet, retrievedValue,
                                    string.Format(CultureInfo.InvariantCulture,
                                                  "Could not set and get this property '{0}'", property.Name));
                }
            }
        }

        public static IPrincipal MockPrincipalWithRoles(params string[] roles)
        {
            var principal = new Mock<IPrincipal>();
            principal.Setup(p => p.Identity.IsAuthenticated).Returns(true);
            principal.Setup(p => p.Identity.Name).Returns("Username");
            Array.ForEach(roles, role => principal.Setup(p => p.IsInRole(role)).Returns(true));
            return principal.Object;
        }

        public static void AssertEnclosures(Enclosure expected, Enclosure result)
        {
            Assert.AreEqual(expected.Title, result.Title, "Wrong title.");
            Assert.AreEqual(expected.Url, result.Url, "Wrong Url.");
            Assert.AreEqual(expected.MimeType, result.MimeType, "Wrong mimetype.");
            Assert.AreEqual(expected.Size, result.Size, "Wrong size.");
            Assert.AreEqual(expected.AddToFeed, result.AddToFeed, "Wrong AddToFeed flag.");
            Assert.AreEqual(expected.ShowWithPost, result.ShowWithPost, "Wrong ShowWithPost flag.");
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
        internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, int port,
                                                          string page)
        {
            return SetupBlog(subfolder, applicationPath, port, page, "username", "password");
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
        internal static SimulatedRequestContext SetupBlog(string subfolder, string applicationPath, int port,
                                                          string page, string userName, string password)
        {
            string host = GenerateUniqueString();

            HttpContext.Current = null;
            //I wish this returned the blog it created.
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("Unit Test Blog", userName, password, host, subfolder);
            Blog blog = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(host, subfolder);

            var sb = new StringBuilder();
            TextWriter output = new StringWriter(sb);
            SimulatedHttpRequest request = SetHttpContextWithBlogRequest(host, port, subfolder, applicationPath, page,
                                                                         output, "GET");
            BlogRequest.Current.Blog = blog;

            if (Config.CurrentBlog != null)
            {
                Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
            }
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName), new[] { "Administrators" });

            return new SimulatedRequestContext(request, sb, output, host);
        }

        public static int Create(Entry entry)
        {
            var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
            var serviceLocator = new Mock<IDependencyResolver>().Object;
            var searchEngineService = new Mock<IIndexingService>().Object;
            DependencyResolver.SetResolver(serviceLocator);
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, serviceLocator);
            Routes.RegisterRoutes(subtextRoutes);
            var urlHelper = new BlogUrlHelper(requestContext, routes);
            var subtextContext = new SubtextContext(Config.CurrentBlog, requestContext, urlHelper,
                                                    new DatabaseObjectProvider(), requestContext.HttpContext.User,
                                                    new SubtextCache(requestContext.HttpContext.Cache), serviceLocator);
            IEntryPublisher entryPublisher = CreateEntryPublisher(subtextContext, searchEngineService);
            int id = entryPublisher.Publish(entry);
            entry.Id = id;
            return id;
        }

        public static IEntryPublisher CreateEntryPublisher(ISubtextContext subtextContext, IIndexingService searchEngineService)
        {
            var slugGenerator = new SlugGenerator(FriendlyUrlSettings.Settings, subtextContext.Repository);
            var transformations = new CompositeTextTransformation
            {
                new XhtmlConverter(), 
                new EmoticonsTransformation(subtextContext)
            };
            return new EntryPublisher(subtextContext, transformations, slugGenerator, searchEngineService);
        }

        public static Stream ToStream(this string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static IKernel MockKernel(Func<IEnumerable<object>> returnFunc)
        {
            var request = new Mock<IRequest>();
            var kernel = new Mock<IKernel>();
            kernel.Setup(
                k =>
                k.CreateRequest(It.IsAny<Type>(), It.IsAny<Func<IBindingMetadata, bool>>(),
                                It.IsAny<IEnumerable<IParameter>>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(request.Object);
            kernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(returnFunc);
            return kernel.Object;
        }

        public static BlogUrlHelper SetupUrlHelper(string appPath)
        {
            return SetupUrlHelper(appPath, new RouteData());
        }

        public static BlogUrlHelper SetupUrlHelper(string appPath, RouteData routeData)
        {
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.ApplicationPath).Returns(appPath);
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var helper = new BlogUrlHelper(requestContext, routes);
            return helper;
        }

        /// <summary>
        /// Updates the specified entry in the data provider.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void Update(Entry entry, ISubtextContext context)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            ObjectProvider repository = new DatabaseObjectProvider();
            var transform = new CompositeTextTransformation
            {
                new XhtmlConverter(),
                new EmoticonsTransformation(context),
                new KeywordExpander(repository)
            };

            //TODO: Maybe use a INinjectParameter to control this.
            var searchEngineService = new Mock<IIndexingService>().Object;
            var publisher = new EntryPublisher(context, transform, new SlugGenerator(FriendlyUrlSettings.Settings), searchEngineService);
            publisher.Publish(entry);
        }

        public static Entry GetEntry(int entryId, PostConfig postConfig, bool includeCategories)
        {
            bool isActive = ((postConfig & PostConfig.IsActive) == PostConfig.IsActive);
            return new DatabaseObjectProvider().GetEntry(entryId, isActive, includeCategories);
        }

        public static ArgumentNullException AssertThrowsArgumentNullException(this Action action)
        {
            return action.AssertThrows<ArgumentNullException>();
        }

        public static TException AssertThrows<TException>(this Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException exception)
            {
                return exception;
            }
            return null;
        }

        /// <summary>
        /// Makes sure we can read app settings
        /// </summary>
        public static void AssertAppSettings()
        {
            Assert.AreEqual("UnitTestValue", ConfigurationManager.AppSettings["UnitTestKey"], "Cannot read app settings");
        }

        public static void WriteTableToOutput(string tableName)
        {
            string sql = String.Format("SELECT * FROM {0}", tableName);
            Console.WriteLine("Table: " + tableName);
            using (var dataset = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql))
            {
                foreach (DataColumn column in dataset.Tables[0].Columns)
                {
                    Console.Write(column.ColumnName + "\t");
                }
                Console.WriteLine();

                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    foreach (DataColumn column in dataset.Tables[0].Columns)
                    {
                        Console.Write(row[column] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
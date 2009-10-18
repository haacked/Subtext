#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Web;

namespace Subtext.Framework.Routing
{
    public class UrlHelper
    {
        protected UrlHelper()
        {
        }

        public UrlHelper(RequestContext context, RouteCollection routes)
        {
            RequestContext = context ??
                             new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
            Routes = routes ?? RouteTable.Routes;
        }

        public HttpContextBase HttpContext
        {
            get { return RequestContext.HttpContext; }
        }

        protected RequestContext RequestContext { get; private set; }

        public RouteCollection Routes { get; private set; }

        public virtual VirtualPath AppRoot()
        {
            return new VirtualPath(GetNormalizedAppPath());
        }

        private string GetNormalizedAppPath()
        {
            string appRoot = HttpContext.Request.ApplicationPath;
            if(!appRoot.EndsWith("/"))
            {
                appRoot += "/";
            }
            return appRoot;
        }

        public virtual VirtualPath FeedbackUrl(FeedbackItem comment)
        {
            if(comment == null)
            {
                throw new ArgumentNullException("comment");
            }
            if(comment.FeedbackType == FeedbackType.ContactPage || comment.Entry == null)
            {
                return null;
            }
            string entryUrl = EntryUrl(comment.Entry);
            if(string.IsNullOrEmpty(entryUrl))
            {
                return null;
            }
            return entryUrl + "#" + comment.Id;
        }

        public virtual VirtualPath EntryUrl(IEntryIdentity entry)
        {
            return EntryUrl(entry, null);
        }

        public virtual VirtualPath EntryUrl(IEntryIdentity entry, Blog entryBlog)
        {
            if(entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if(entry.PostType == PostType.None)
            {
                throw new ArgumentException(Resources.Argument_EntryMustHaveValidPostType, "entry");
            }

            if(NullValue.IsNull(entry.Id))
            {
                return null;
            }

            string routeName;
            var routeValues = new RouteValueDictionary();

            if(entry.PostType == PostType.BlogPost)
            {
                routeValues.Add("year", entry.DateCreated.ToString("yyyy", CultureInfo.InvariantCulture));
                routeValues.Add("month", entry.DateCreated.ToString("MM", CultureInfo.InvariantCulture));
                routeValues.Add("day", entry.DateCreated.ToString("dd", CultureInfo.InvariantCulture));
                routeName = "entry-";
            }
            else
            {
                routeName = "article-";
            }

            if(string.IsNullOrEmpty(entry.EntryName))
            {
                routeValues.Add("id", entry.Id);
                routeName += "by-id";
            }
            else
            {
                routeValues.Add("slug", entry.EntryName);
                routeName += "by-slug";
            }
            if(entryBlog != null)
            {
                routeValues.Add("subfolder", entryBlog.Subfolder);
            }

            VirtualPathData virtualPath = Routes.GetVirtualPath(RequestContext, routeName, routeValues);
            if(virtualPath != null)
            {
                return virtualPath.VirtualPath;
            }
            return null;
        }

        private static string NormalizeFileName(string filename)
        {
            if(filename.StartsWith("/"))
            {
                return filename.Substring(1);
            }
            return filename;
        }

        private string GetImageDirectoryTildePath(Blog blog)
        {
            string host = blog.Host.Replace(":", "_").Replace(".", "_");
            string appPath = GetNormalizedAppPath().Replace(".", "_");
            string subfolder = String.IsNullOrEmpty(blog.Subfolder) ? String.Empty : blog.Subfolder + "/";
            return "~/images/" + host + appPath + subfolder;
        }

        private string GetImageTildePath(Blog blog, string filename)
        {
            return GetImageDirectoryTildePath(blog) + NormalizeFileName(filename);
        }

        private string GetGalleryImageTildePath(Image image, string filename)
        {
            return GetImageDirectoryTildePath(image.Blog) + image.CategoryID + "/" + filename;
        }

        /// <summary>
        /// Returns the URL for an image that was uploaded to a blog via MetaWeblog API. The image 
        /// is not associated with an image gallery.
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public virtual VirtualPath ImageUrl(Blog blog, string filename)
        {
            return ResolveUrl(GetImageTildePath(blog, filename));
        }

        /// <summary>
        /// Returns the direct URL to an image within a gallery.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual VirtualPath GalleryImageUrl(Image image)
        {
            return GalleryImageUrl(image, image.OriginalFile);
        }

        public VirtualPath GalleryImageUrl(Image image, string fileName)
        {
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }

            if(!String.IsNullOrEmpty(image.Url))
            {
                return ResolveUrl(image.Url + fileName);
            }
            return ResolveUrl(GetGalleryImageTildePath(image, fileName));
        }

        public virtual VirtualPath ImageDirectoryUrl(Blog blog)
        {
            return ResolveUrl(GetImageDirectoryTildePath(blog));
        }

        /// <summary>
        /// Returns the physical gallery path for the specified category.
        /// </summary>
        public virtual string GalleryDirectoryPath(Blog blog, int categoryId)
        {
            string path = ImageGalleryDirectoryUrl(blog, categoryId);
            return HttpContext.Server.MapPath(path);
        }

        public virtual string ImageDirectoryPath(Blog blog)
        {
            return HttpContext.Server.MapPath(ImageDirectoryUrl(blog));
        }

        /// <summary>
        /// Returns the URL to a page that displays an image within a gallery.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual VirtualPath GalleryImagePageUrl(Image image)
        {
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }
            return GetVirtualPath("gallery-image", new {id = image.ImageID, subfolder = image.Blog.Subfolder});
        }

        public virtual VirtualPath ImageGalleryDirectoryUrl(Blog blog, int galleryId)
        {
            var image = new Image {Blog = blog, CategoryID = galleryId};
            string imageUrl = GalleryImageUrl(image, string.Empty);
            if(!imageUrl.EndsWith("/"))
            {
                imageUrl += "/";
            }
            return imageUrl;
        }

        public virtual VirtualPath GalleryUrl(int id)
        {
            return GetVirtualPath("gallery", new {id});
        }

        public virtual VirtualPath GalleryUrl(Image image)
        {
            return GetVirtualPath("gallery", new {id = image.CategoryID, subfolder = image.Blog.Subfolder});
        }

        public virtual VirtualPath AggBugUrl(int id)
        {
            return GetVirtualPath("aggbug", new {id});
        }

        public virtual VirtualPath ResolveUrl(string virtualPath)
        {
            return RequestContext.HttpContext.ExpandTildePath(virtualPath);
        }

        public virtual VirtualPath BlogUrl()
        {
            string vp = GetVirtualPath("root", new {});
            return BlogUrl(vp);
        }

        public virtual VirtualPath BlogUrl(Blog blog)
        {
            string vp = GetVirtualPath("root", new {subfolder = blog.Subfolder});
            return BlogUrl(vp);
        }

        private static VirtualPath BlogUrl(string virtualPath)
        {
            if(!(virtualPath ?? string.Empty).EndsWith("/"))
            {
                virtualPath += "/";
            }
            //TODO: Make this an option.
            virtualPath += "default.aspx";
            return virtualPath;
        }

        public virtual VirtualPath ContactFormUrl()
        {
            return GetVirtualPath("contact", null);
        }

        public virtual VirtualPath MonthUrl(DateTime dateTime)
        {
            return GetVirtualPath("entries-by-month",
                                  new
                                  {
                                      year = dateTime.ToString("yyyy", CultureInfo.InvariantCulture),
                                      month = dateTime.ToString("MM", CultureInfo.InvariantCulture)
                                  });
        }

        public virtual VirtualPath CommentApiUrl(int entryId)
        {
            return GetVirtualPath("comment-api", new {id = entryId});
        }

        public virtual VirtualPath CommentRssUrl(int entryId)
        {
            return GetVirtualPath("comment-rss", new {id = entryId});
        }

        public virtual VirtualPath TrackbacksUrl(int entryId)
        {
            return GetVirtualPath("trackbacks", new {id = entryId});
        }

        public virtual VirtualPath CategoryUrl(Category category)
        {
            return GetVirtualPath("category", new {slug = category.Id, categoryType = "category"});
        }

        public virtual VirtualPath CategoryRssUrl(Category category)
        {
            return GetVirtualPath("rss", new {catId = category.Id});
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual VirtualPath DayUrl(DateTime date)
        {
            return GetVirtualPath("entries-by-day",
                                  new
                                  {
                                      year = date.ToString("yyyy", CultureInfo.InvariantCulture),
                                      month = date.ToString("MM", CultureInfo.InvariantCulture),
                                      day = date.ToString("dd", CultureInfo.InvariantCulture)
                                  });
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        public virtual Uri RssUrl(Blog blog)
        {
            if(blog.RssProxyEnabled)
            {
                return RssProxyUrl(blog);
            }

            return GetVirtualPath("rss", null).ToFullyQualifiedUrl(blog);
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        public virtual Uri AtomUrl(Blog blog)
        {
            if(blog.RssProxyEnabled)
            {
                return RssProxyUrl(blog);
            }

            return GetVirtualPath("atom", null).ToFullyQualifiedUrl(blog);
        }

        public virtual Uri RssProxyUrl(Blog blog)
        {
            //TODO: Store this in db.
            string feedburnerUrl = ConfigurationManager.AppSettings["FeedBurnerUrl"];
            feedburnerUrl = String.IsNullOrEmpty(feedburnerUrl) ? "http://feedproxy.google.com/" : feedburnerUrl;
            return new Uri(new Uri(feedburnerUrl), blog.RssProxyUrl);
        }

        public virtual VirtualPath GetVirtualPath(string routeName, object routeValues)
        {
            RouteValueDictionary routeValueDictionary;

            if(routeValues is RouteValueDictionary)
            {
                routeValueDictionary = (RouteValueDictionary)routeValues;
            }
            else
            {
                routeValueDictionary = new RouteValueDictionary(routeValues);
            }

            VirtualPathData virtualPath = Routes.GetVirtualPath(RequestContext, routeName, routeValueDictionary);
            if(virtualPath == null)
            {
                return null;
            }
            return virtualPath.VirtualPath;
        }

        public virtual VirtualPath LoginUrl()
        {
            return GetVirtualPath("login", new {});
        }

        public virtual VirtualPath LogoutUrl()
        {
            return GetVirtualPath("logout", new {});
        }

        public virtual VirtualPath ArchivesUrl()
        {
            return GetVirtualPath("archives", new {});
        }

        public virtual VirtualPath AdminUrl(string path)
        {
            return AdminUrl(path, null);
        }

        public virtual VirtualPath AdminUrl(string path, object routeValues)
        {
            RouteValueDictionary routeValueDict = (routeValues as RouteValueDictionary) ??
                                                  new RouteValueDictionary(routeValues);
            return AdminUrl(path, routeValueDict);
        }

        public virtual VirtualPath AdminUrl(string path, RouteValueDictionary routeValues)
        {
            routeValues = routeValues ?? new RouteValueDictionary();
            // TODO: Provide a flag to turn this off.
            //       This is to support IIS 6 / IIS 7 Classic Mode
            if(!path.EndsWith(".aspx"))
            {
                if(path.Length > 0 && !path.EndsWith("/"))
                {
                    path += "/";
                }
                path += "default.aspx";
            }
            routeValues.Add("pathinfo", path);
            return GetVirtualPath("admin", routeValues);
        }

        public virtual VirtualPath AdminRssUrl(string feedName)
        {
            return GetVirtualPath("admin-rss", new {feedName});
        }

        public virtual Uri MetaWeblogApiUrl(Blog blog)
        {
            VirtualPath vp = GetVirtualPath("metaweblogapi", null);
            return vp.ToFullyQualifiedUrl(blog);
        }

        public virtual Uri RsdUrl(Blog blog)
        {
            VirtualPath vp = GetVirtualPath("rsd", null);
            return vp.ToFullyQualifiedUrl(blog);
        }

        public virtual VirtualPath CustomCssUrl()
        {
            return GetVirtualPath("customcss", null);
        }

        public virtual VirtualPath EditIconUrl()
        {
            return AppRoot() + "images/edit.gif";
        }

        public virtual VirtualPath TagUrl(string tagName)
        {
            return GetVirtualPath("tag", new {tag = tagName.Replace("#", "{:#:}")});
        }

        public virtual VirtualPath TagCloudUrl()
        {
            return GetVirtualPath("tag-cloud", null);
        }
    }
}
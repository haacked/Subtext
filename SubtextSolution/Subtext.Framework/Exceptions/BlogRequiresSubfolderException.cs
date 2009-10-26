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
using System.Globalization;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Exceptions
{
    /// <summary>
    /// Exception thrown when creating a new blog, or changing an existing 
    /// blog, without a Subfolder value specified, when another blog 
    /// with the same Host name exists.
    /// </summary>
    /// <remarks>
    /// An example of this case is where a system has a blog with the host 
    /// "example.com" and the subfolder name "MyBlog".  Attempting to create 
    /// a new blog with the host name "example.com" and an empty subfolder 
    /// name will result in this exception being thrown.
    /// </remarks>
    [Serializable]
    public class BlogRequiresSubfolderException : BaseBlogConfigurationException
    {
        readonly string _host;

        /// <summary>
        /// Creates a new <see cref="BlogRequiresSubfolderException"/> instance.
        /// </summary>
        public BlogRequiresSubfolderException(string hostName, int blogsWithSameHostCount, int blogId)
        {
            _host = hostName;
            BlogsWithSameHostCount = blogsWithSameHostCount;
            BlogId = blogId;
        }

        /// <summary>
        /// Creates a new <see cref="BlogRequiresSubfolderException"/> instance.
        /// </summary>
        public BlogRequiresSubfolderException(string hostName, int blogsWithSameHostCount)
            : this(hostName, blogsWithSameHostCount, NullValue.NullInt32)
        {
        }

        /// <summary>
        /// Gets the blogs with same host count.
        /// </summary>
        /// <value></value>
        public int BlogsWithSameHostCount { get; private set; }

        /// <summary>
        /// Gets the blog id.
        /// </summary>
        /// <value></value>
        public int BlogId { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                string blogCountClause = Resources.IsAnotherBlog;
                if(BlogsWithSameHostCount >= 1)
                {
                    blogCountClause = String.Format(CultureInfo.InvariantCulture, Resources.BlogCountClause,
                                                    BlogsWithSameHostCount);
                }

                return String.Format(CultureInfo.InvariantCulture,
                                     Resources.BlogRequiresSubfolder_ThereAreBlogsWithSameHostName, blogCountClause,
                                     _host);
            }
        }
    }
}
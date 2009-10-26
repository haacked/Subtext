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

namespace Subtext.Framework.Exceptions
{
    /// <summary>
    /// Exception thrown when creating or updating a blog that would cause 
    /// another blog to be hidden.  This should be a rare occurrence, but 
    /// entirely possible with multiple blogs.
    /// </summary>
    /// <remarks>
    /// <p>This exception occurs when adding a blog with the same hostname as another blog, 
    /// but the original blog does not have an subfolder name defined.</p>  
    /// <p>For example, if there exists a blog with the host "www.example.com" with no 
    /// subfolder defined, and the admin adds another blog with the host "www.example.com" 
    /// and subfolder as "MyBlog", this creates a multiple blog situation in the example.com 
    /// domain.  In that situation, each example.com blog MUST have an subfolder name defined. 
    /// The URL to the example.com with no subfolder becomes the aggregate blog.
    /// </p>
    /// </remarks>
    [Serializable]
    public class BlogHiddenException : BaseBlogConfigurationException
    {
        readonly int _blogId;
        readonly Blog _hiddenBlog;

        /// <summary>
        /// Creates a new <see cref="BlogHiddenException"/> instance.
        /// </summary>
        /// <param name="hidden">Hidden.</param>
        /// <param name="blogId"></param>
        public BlogHiddenException(Blog hidden, int blogId)
        {
            _hiddenBlog = hidden;
            _blogId = blogId;
        }

        /// <summary>
        /// Creates a new <see cref="BlogHiddenException"/> instance.
        /// </summary>
        /// <param name="hidden">Hidden.</param>
        public BlogHiddenException(Blog hidden) : this(hidden, NullValue.NullInt32)
        {
        }

        /// <summary>
        /// Gets the hidden blog.
        /// </summary>
        /// <value></value>
        public Blog HiddenBlog
        {
            get { return _hiddenBlog; }
        }

        /// <summary>
        /// Gets the blog id.
        /// </summary>
        /// <value></value>
        public int BlogId
        {
            get { return _blogId; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                //TODO: We need to move the message out of the exception class.

                string message = _blogId == NullValue.NullInt32 ? "<p>Creating/Activating this blog " : "<p>Sorry, but by changing this blog to use that host combination ";

                message += "would cause the blog entitled &#8220;" + _hiddenBlog.Title + "&#8221; to be hidden. "
                           + "by causing more than one blog to have the host &#8220;" + _hiddenBlog.Host +
                           "&#8221;.</p><p>"
                           + "When two or more blogs have the same host, they both need to have an subfolder defined. "
                           +
                           "The previously mentioned blog does not have a subfolder defined.  Please update it before ";

                if(_blogId == NullValue.NullInt32)
                {
                    message += "creating/activating this blog.</p>";
                }
                else
                {
                    message += "making this change.</p>";
                }
                return message;
            }
        }
    }
}
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
    /// Exception thrown when trying to add a blog that 
    /// duplicates another blog in both host and application.
    /// </summary>
    [Serializable]
    public class BlogDuplicationException : BaseBlogConfigurationException
    {
        readonly int _blogId = NullValue.NullInt32;

        readonly Blog _duplicateBlog;

        /// <summary>
        /// Creates a new <see cref="BlogDuplicationException"/> instance.
        /// </summary>
        /// <param name="duplicate">Duplicate.</param>
        public BlogDuplicationException(Blog duplicate) : this(duplicate, NullValue.NullInt32)
        {
        }

        /// <summary>
        /// Creates a new <see cref="BlogDuplicationException"/> instance.
        /// </summary>
        /// <param name="duplicate">Duplicate.</param>
        /// <param name="blogId">Blog id of the blog we were updating.  If this is .</param>
        public BlogDuplicationException(Blog duplicate, int blogId)
        {
            _duplicateBlog = duplicate;
            _blogId = blogId;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                if(_blogId == NullValue.NullInt32)
                {
                    return string.Format("Oooh. A blog with the same host '{0}' and subfolder '{1}' already exists.", _duplicateBlog.Host, _duplicateBlog.Subfolder);
                }
                return string.Format("Sorry, but changing this blog to use that host '{0}' and subfolder '{1}' would conflict with another blog.", _duplicateBlog.Host, _duplicateBlog.Subfolder);
            }
        }

        /// <summary>
        /// Gets the duplicate blog.
        /// </summary>
        /// <value></value>
        public Blog DuplicateBlog
        {
            get { return _duplicateBlog; }
        }

        /// <summary>
        /// Id of the blog being updated that caused this exception.  This 
        /// would be populated if updating a blog to have the same host and 
        /// subfolder as another blog.  Otherwise this is equal to NullValue.NullInt32.
        /// </summary>
        /// <value></value>
        public int BlogId
        {
            get { return _blogId; }
        }
    }
}
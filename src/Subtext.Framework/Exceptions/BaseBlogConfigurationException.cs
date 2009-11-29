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
    /// Base exception class for blog configuration errors.
    /// </summary>
    [Serializable]
    public abstract class BaseBlogConfigurationException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="BaseBlogConfigurationException"/> instance.
        /// </summary>
        protected BaseBlogConfigurationException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="BaseBlogConfigurationException"/> instance.
        /// </summary>
        /// <param name="innerException">Inner exception.</param>
        protected BaseBlogConfigurationException(Exception innerException) : base(null, innerException)
        {
        }
    }
}
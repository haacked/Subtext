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
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Subtext.Akismet
{
    /// <summary>
    /// Exception thrown when a response other than 200 is returned.
    /// </summary>
    /// <remarks>
    /// This exception does not have any custom properties, 
    /// thus it does not implement ISerializable.
    /// </remarks>
    [Serializable]
    public sealed class InvalidResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
        /// </summary>
        public InvalidResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidResponseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
        /// </summary>
        public InvalidResponseException(string message, HttpStatusCode status)
            : base(message)
        {
            HttpStatus = status;
        }

        private InvalidResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            HttpStatus = (HttpStatusCode)info.GetInt32("HttpStatus");
        }

        /// <summary>
        /// Gets the HTTP status returned by the service.
        /// </summary>
        /// <value>The HTTP status.</value>
        public HttpStatusCode HttpStatus { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("HttpStatus", (int)HttpStatus);
            base.GetObjectData(info, context);
        }
    }
}
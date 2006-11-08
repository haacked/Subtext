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
	public sealed class InvalidResponseException : Exception, ISerializable
	{
		HttpStatusCode status = (HttpStatusCode)0;

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		public InvalidResponseException() : base()
		{
		}
		
		private InvalidResponseException(SerializationInfo info, StreamingContext context)
		{
			status = (HttpStatusCode)(info.GetValue("Status", typeof(HttpStatusCode)));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public InvalidResponseException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public InvalidResponseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>
		/// with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Status", this.status);
			GetObjectData(info, context);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public InvalidResponseException(string message, HttpStatusCode status) : base(message)
		{
			this.status = status;
		}

		/// <summary>
		/// Gets the HTTP status returned by the service.
		/// </summary>
		/// <value>The HTTP status.</value>
		public HttpStatusCode HttpStatus
		{
			get
			{
				return this.status;
			}
		}
	}
}

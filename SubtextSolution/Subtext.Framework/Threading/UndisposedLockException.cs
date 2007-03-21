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
using System.Text;
using System.Runtime.Serialization;

namespace Subtext.Framework.Threading
{
	#if DEBUG
		/// <summary>
		/// This exception indicates that a user of the TimedLock struct 
		/// failed to leave a Monitor.  This could be the result of a 
		/// deadlock or forgetting to use the using statement or a try 
		/// finally block.
		/// </summary>
		[Serializable]
		public class UndisposedLockException : Exception, ISerializable
		{
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="message"></param>
			public UndisposedLockException(string message)
				: base(message)
			{
			}

			/// <summary>
			/// Special constructor used for deserialization.
			/// </summary>
			/// <param name="info"></param>
			/// <param name="context"></param>
			protected UndisposedLockException(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}

			public UndisposedLockException()
				: base()
			{
			}

			public UndisposedLockException(string message, Exception innerException)
				: base(message, innerException)
			{
			}
		}
	#endif
}

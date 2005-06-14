using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when the HostInfo table (or other backing store 
	/// depending on the data provider) does not exist.
	/// </summary>
	[Serializable]
	public class HostDataDoesNotExistException : BaseSubtextException
	{
		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return "The HostInfo table does not exist.";
			}
		}

		/// <summary>
		/// Returns a resource key for the message.  This is used to 
		/// look up the message in the correct language within a 
		/// resource file (when we get around to I8N).
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get { throw new NotImplementedException(); }
		}
	}
}

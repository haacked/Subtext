using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Summary description for HostNotConfiguredException.
	/// </summary>
	public class HostNotConfiguredException : BaseSubtextException
	{
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

using System;

namespace Subtext.Framework.Exceptions
{
	[Serializable]
	public class DuplicateEntryException : Exception
	{
		public DuplicateEntryException(string message) : this(message, null)
		{
		}

		public DuplicateEntryException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}

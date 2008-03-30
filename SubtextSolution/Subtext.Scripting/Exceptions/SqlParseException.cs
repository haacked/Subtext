using System;

namespace Subtext.Scripting.Exceptions
{
	public class SqlParseException : Exception
	{
		public SqlParseException(string message) : base(message)
		{
		}
	}
}

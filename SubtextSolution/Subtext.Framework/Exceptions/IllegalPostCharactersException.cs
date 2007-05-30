using System;
using System.Runtime.Serialization;

namespace Subtext.Framework.Exceptions
{
	[Serializable]
	public class IllegalPostCharactersException : Exception
	{
		public IllegalPostCharactersException() { }
		public IllegalPostCharactersException(String s) : base(s) { }
		public IllegalPostCharactersException(String s, Exception inner) : base(s, inner) { }
		protected IllegalPostCharactersException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

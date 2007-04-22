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
using Subtext.Framework.Properties;
using System.Runtime.Serialization;

namespace Subtext.Framework
{
	[Serializable]
	public class IllegalPostCharactersException : Exception
	{
		public IllegalPostCharactersException() : base() {}
		public IllegalPostCharactersException(String s) : base(s) {}
		public IllegalPostCharactersException(String s, Exception inner) : base(s, inner) {}
		protected IllegalPostCharactersException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

	[Serializable]
	public class BlogSkinException : Exception
	{
		public BlogSkinException() : base() {}
		public BlogSkinException(String s) : base(s) {}
		public BlogSkinException(String s, Exception inner) : base(s, inner) {}
		protected BlogSkinException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

	[Serializable]
	public class BlogFailedPostException : Exception 
	{
		public BlogFailedPostException() : base() {}
		public BlogFailedPostException(String s) : base(s) {}
		public BlogFailedPostException(String s, Exception inner) : base(s, inner) {}
        protected BlogFailedPostException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class BlogAssemblyConfigException : Exception 
	{
		public BlogAssemblyConfigException() : base() {}
		public BlogAssemblyConfigException(String s) : base(s) {}
		public BlogAssemblyConfigException(String s, Exception inner) : base(s, inner) {}
        protected BlogAssemblyConfigException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class ExtendedPropertiesOverFlowException : Exception 
	{
		public ExtendedPropertiesOverFlowException() : base(Resources.ExtendedPropertiesOverflow_Generic) {}
		public ExtendedPropertiesOverFlowException(String s) : base(s) {}
		public ExtendedPropertiesOverFlowException(String s, Exception inner) : base(s, inner) {}
        protected ExtendedPropertiesOverFlowException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}

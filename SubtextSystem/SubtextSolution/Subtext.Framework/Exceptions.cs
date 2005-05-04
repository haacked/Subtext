using System;

namespace Subtext.Framework
{
	public class BlogDoesNotExistException : ApplicationException 
	{
		public BlogDoesNotExistException() {}
		public BlogDoesNotExistException(String s) : base(s) {}
		public BlogDoesNotExistException(String s, Exception inner) : base(s, inner) {}
	}

	public class IllegalPostCharactersException : ApplicationException 
	{
		public IllegalPostCharactersException() {}
		public IllegalPostCharactersException(String s) : base(s) {}
		public IllegalPostCharactersException(String s, Exception inner) : base(s, inner) {}
	}

	public class BlogSkinException : ApplicationException 
	{
		public BlogSkinException() {}
		public BlogSkinException(String s) : base(s) {}
		public BlogSkinException(String s, Exception inner) : base(s, inner) {}
		
	}

	public class BlogFailedPostException : ApplicationException 
	{
		public BlogFailedPostException() {}
		public BlogFailedPostException(String s) : base(s) {}
		public BlogFailedPostException(String s, Exception inner) : base(s, inner) {}		
	}

	public class BlogAssemblyConfigException : ApplicationException 
	{
		public BlogAssemblyConfigException() {}
		public BlogAssemblyConfigException(String s) : base(s) {}
		public BlogAssemblyConfigException(String s, Exception inner) : base(s, inner) {}		
	}

	public class ExtendedPropertiesOverFlowException : ApplicationException 
	{
		public ExtendedPropertiesOverFlowException():base("ExtendedProperties bytes overflow. The ExtendedProperties is limited to 7800 bytes")
		{
			
		}
		public ExtendedPropertiesOverFlowException(String s) : base(s) {}
		public ExtendedPropertiesOverFlowException(String s, Exception inner) : base(s, inner) {}		
	}
}

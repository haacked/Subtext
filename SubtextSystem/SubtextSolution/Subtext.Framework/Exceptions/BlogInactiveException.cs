using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when attempting to visit a blog that is no longer active.
	/// </summary>
	public class BlogInactiveException : BaseBlogConfigurationException
	{
		public BlogInactiveException() : base()
		{
		}

		public override string MessageResourceKey
		{
			get
			{
				throw new NotImplementedException("Not yet implemented.");
			}
		}

	}
}

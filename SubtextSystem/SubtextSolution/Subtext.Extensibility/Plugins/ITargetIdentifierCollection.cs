using System;
using System.Collections;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for ITargetIdentifierCollection.
	/// </summary>
	public interface ITargetIdentifierCollection : ICollection
	{
		ITargetIdentifier this[int index] {get;}
		bool Contains(ITargetIdentifier targetIdentifier);
		void CopyTo(ITargetIdentifier[] targetIdentifiers, int index);
	}
}

using System;
using System.Collections;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IVersionDeclarationCollection.
	/// </summary>
	public interface IVersionDeclarationCollection : ICollection
	{
		IVersionDeclaration this[int index] {get;}
		bool Contains(IVersionDeclaration versionDeclaration);
		void CopyTo(IVersionDeclaration[] versionDeclarations, int index);
	}
}

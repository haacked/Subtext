using System;
using System.Collections;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IPluginIdentifierCollection.
	/// </summary>
	public interface IPluginIdentifierCollection : ICollection
	{
		IPluginIdentifier this[int index] {get;}
		bool Contains(IPluginIdentifier pluginIdentifier);
		void CopyTo(IPluginIdentifier[] pluginIdentifiers, int index);
	}
}

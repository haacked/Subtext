// This file contains documentation for the various Namespaces within this assembly.
// These classes are only used by NDoc to generate namespace documentation.
// They should all be internal sealed classes with private constructors.
//
// http://ndoc.sourceforge.net/reference/NDoc.Core.Reflection.BaseReflectionDocumenterConfig.UseNamespaceDocSummaries.html

#if debug
namespace Subtext.Framework
{
	/// <summary>
	/// Contains the primary framework classes used by 
	/// the Subtext blogging engine.
	/// </summary>
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Contains the primary business layer classes such as <see cref="Entry"/>, 
	/// <see cref="Image"/>, <see cref="KeyWord"/>.
	/// </summary>
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}
#endif

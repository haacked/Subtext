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

// This file contains documentation for the various Namespaces within this assembly.
// These classes are only used by NDoc to generate namespace documentation.
// They should all be internal sealed classes with private constructors.
//
// http://ndoc.sourceforge.net/reference/NDoc.Core.Reflection.BaseReflectionDocumenterConfig.UseNamespaceDocSummaries.html

#if DOCUMENTATION
namespace Subtext.BlogML
{
	/// <summary>
	/// <para>Contains classes used to make implementing BlogML easier for 
	/// developers.</para> In particular,
	/// <para>
	/// In particular, a developer can simply implement <see cref="BlogMLProvider" /> 
	/// and then add a reference to <see cref="BlogMLHttpHandler" /> within web.config.
	/// </para>
	/// </summary>
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}

namespace Subtext.BlogML.Conversion
{
	/// <summary>
	/// Contains concrete instances of <see cref="IdConversionStrategy" />.  This allows an export 
	/// to represent internal IDs in another format (such as a guid) within the blogml export file 
	/// yet still maintain referential integrity between elements.
	/// </summary>
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}

namespace Subtext.BlogML.Interfaces
{
	/// <summary>
	/// Contains the <see cref="IBlogMLContext" /> and <see cref="IBlogMLProvider" /> interfaces.
	/// </summary>
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}
#endif

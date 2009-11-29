#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

#if DOCUMENTATION
using System.Diagnostics.CodeAnalysis;

namespace Subtext
{
	/// <summary>
	/// Subtext is a blogging engine built on the .NET Framework.  
	/// For more information, check out SubtextProject.com.
	/// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is used to generate namespace summary documentation.")]
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}

namespace Subtext.Framework
{
	/// <summary>
	/// Contains the primary framework classes used by 
	/// the Subtext blogging engine.
	/// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is used to generate namespace summary documentation.")]
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// <p>
	/// Contains classes used to read various configuration data 
	/// for Subtext.  Configuration data is generally stored in two places, 
	/// Web.config or the blog_config table.</p>
	/// <p>
	/// Either way, the class to use when accessing any configuration setting 
	/// is the <see cref="Subtext.Framework.Configuration.Config" /> class.  
	/// </p>
	/// <p>
	/// The <see cref="Config.Settings"/> returns an instance of <see cref="BlogConfigurationSettings"/> 
	/// which contains settings configured in a custom section of Web.config (see the &lt;BlogConfigurationSettings&gt; 
	/// tag in Web.config).
	/// </p>
	/// <p>
	/// The <see cref="Config.CurrentBlog"/> method returns an instance of <see cref="Blog"/> 
	/// contains settings stored in the blog_config table.  This can be used to save settings to 
	/// the configuration as well.
	/// </p>
	/// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is used to generate namespace summary documentation.")]
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
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is used to generate namespace summary documentation.")]
	internal sealed class NamespaceDoc
	{
		private NamespaceDoc()
		{
		}
	}
}
#endif
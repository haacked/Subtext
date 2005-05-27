using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Provides general information about the module
	/// </summary>
	public interface IImplementationInfo
	{
		/// <summary>
		/// Name of the implementation
		/// </summary>
		string Name {get;}

		/// <summary>
		/// Name of the author
		/// </summary>
		string Author {get;}

		/// <summary>
		/// Company name
		/// </summary>
		string Company {get;}

		/// <summary>
		/// Copyright information
		/// </summary>
		string Copyright {get;}

		/// <summary>
		/// Homepage Url
		/// </summary>
		Uri HomePageUrl {get;}

		/// <summary>
		/// Version information
		/// </summary>
		IVersionDeclaration Version {get;}
	}
}

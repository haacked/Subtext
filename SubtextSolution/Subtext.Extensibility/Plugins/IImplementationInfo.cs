#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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
		Uri HomepageUrl {get;}

		/// <summary>
		/// Version information
		/// </summary>
		Version Version {get;}
	}
}

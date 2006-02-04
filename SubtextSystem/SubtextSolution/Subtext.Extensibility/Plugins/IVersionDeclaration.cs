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

using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IVersionDeclaration.
	/// </summary>
	public interface IVersionDeclaration
	{
		int MajorVersion {get;}
		int MinorVersion {get;}
		int BuildNumber {get;}
		int Revision {get;}

		/// <summary>
		/// Compares this instance with a specified version declaration
		/// </summary>
		/// <param name="versionDeclaration">version declaration to compare with</param>
		/// <returns>0 if same version, -1 if the version is lower than the argument, 1 if the version is higher</returns>
		int Compare(IVersionDeclaration versionDeclaration);
	}
}

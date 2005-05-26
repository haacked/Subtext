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

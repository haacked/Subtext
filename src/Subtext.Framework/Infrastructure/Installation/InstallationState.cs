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

namespace Subtext.Framework.Infrastructure.Installation
{
    /// <summary>
    /// Returns the current state of the installation.
    /// </summary>
    public enum InstallationState
    {
        /// <summary>No information available</summary>
        None = 0,
        /// <summary>Subtext is installed, but needs to be upgraded.</summary>
        NeedsUpgrade = 1,
        /// <summary>Subtext needs to be installed.</summary>
        NeedsInstallation = 3,
        /// <summary>Subtext is installed and seems to be working properly.</summary>
        Complete = 4,
    }
}

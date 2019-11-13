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

using System;

namespace Subtext.Framework.Infrastructure.Installation
{
    public interface IInstaller
    {
        void Install(Version assemblyVersion);

        /// <summary>
        /// Upgrades this instance. Returns true if it was successful.
        /// </summary>
        /// <returns></returns>
        void Upgrade(Version assemblyVersion);

        /// <summary>
        /// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
        /// This is the value stored in the database. If it does not match the actual 
        /// assembly version, we may need to run an upgrade.
        /// </summary>
        /// <returns></returns>
        Version GetCurrentInstallationVersion();

        /// <summary>
        /// Gets a value indicating whether the subtext installation needs an upgrade 
        /// to occur.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [needs upgrade]; otherwise, <c>false</c>.
        /// </value>
        bool NeedsUpgrade(Version installationVersion, Version currentAssemblyVersion);
    }
}

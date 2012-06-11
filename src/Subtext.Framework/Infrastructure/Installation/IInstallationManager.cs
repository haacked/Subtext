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
using Subtext.Framework.Services;

namespace Subtext.Framework.Infrastructure.Installation
{
    public interface IInstallationManager
    {
        void Install(Version currentAssemblyVersion);
        void CreateWelcomeContent(ISubtextContext context, IEntryPublisher entryPublisher, Blog blog);
        void Upgrade(Version currentAssemblyVersion);
        bool InstallationActionRequired(Version assemblyVersion, Exception unhandledException);
        void ResetInstallationStatusCache();
        InstallationState GetInstallationStatus(Version currentAssemblyVersion);
        bool IsPermissionDeniedException(Exception exception);
    }
}
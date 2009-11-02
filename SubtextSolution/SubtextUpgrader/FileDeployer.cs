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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubtextUpgrader
{
    public class FileDeployer
    {
        public void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            var files = from file in source.GetFiles()
                        where !String.Equals("Web.config", Path.GetFileName(file.Name))
                        select file;

            CopyFiles(files, destination);
            foreach(var subdir in source.GetDirectories())
            {
                var directoryName = Path.GetDirectoryName(subdir.Name);
                var destinationPath = Path.Combine(destination.Name, directoryName);
                var destinationSubdir = EnsureDirectory(destinationPath);
                CopyDirectory(subdir, destinationSubdir);
            }
        }

        private static void CopyFiles(IEnumerable<FileInfo> files, DirectoryInfo destination)
        {
            foreach(var file in files)
            {
                file.CopyTo(Path.Combine(destination.Name, file.Name));
            }
        }

        private static DirectoryInfo EnsureDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            if(directory.Exists)
            {
                return directory;
            }
            directory.Create();
            return directory;
        }

        public void RemoveOldDirectories(DirectoryInfo destination)
        {
            var directories = new[] { "Admin", "HostAdmin", "Install", "SystemMessages" };
            foreach(var directory in directories)
            {
                Directory.Delete(Path.Combine(destination.Name, directory), true);
            }

            var fileNames = new[] { "SystemMessages", "AggDefault.aspx", "DTP.aspx",
                "ForgotPassword.aspx", "login.aspx", "logout.aspx", "MainFeed.aspx",
                @"Admin\Skins.config", @"Admin\Skins.user.config", @"bin\Subtext.BlogML.dll",
                @"bin\Subtext.Installation.dll", @"bin\Subtext.Scripting", @"bin\Identicon.dll"};
            foreach(var fileName in fileNames)
            {
                File.Delete(Path.Combine(destination.Name, fileName));
            }
        }
    }
}

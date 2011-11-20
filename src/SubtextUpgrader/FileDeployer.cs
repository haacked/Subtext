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

namespace SubtextUpgrader
{
    public class FileDeployer
    {
        public FileDeployer(IDirectory sourceWebroot, IDirectory destination)
        {
            WebRoot = sourceWebroot;
            Destination = destination;
        }

        public IDirectory WebRoot
        {
            get; 
            private set;
        }

        public IDirectory Destination
        {
            get;
            private set;
        }

        public void Deploy()
        {
            WebRoot.CopyTo(Destination, f => !f.Name.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase));
            RemoveOldDirectories();
            RemoveOldFiles();
        }

        public void RemoveOldDirectories()
        {
            var folderNames = new[] { "Admin", "HostAdmin", "Install", "SystemMessages", "Providers", "Sitemap" };
            foreach(var folderName in folderNames)
            {
                Destination.Combine(folderName).Delete(true /*recursive*/);
            }
        }

        public void RemoveOldFiles()
        {
            var fileNames = new[] { "AggDefault.aspx", "DTP.aspx",
                                    "ForgotPassword.aspx", "login.aspx", "logout.aspx", 
                                    "MainFeed.aspx", @"Admin\Skins.config", 
                                    @"Admin\Skins.user.config", @"bin\Subtext.BlogML.dll",
                                    @"bin\Subtext.Installation.dll", @"bin\Subtext.Scripting", 
                                    @"bin\Subtext.Akismet.dll", @"bin\Subtext.Web.Controls.dll", 
                                    @"bin\DotNetOpenId.dll", @"bin\WebControlCaptcha.dll", 
                                    @"bin\PostBackRitalin.dll"};
            
            foreach(var fileName in fileNames)
            {
                Destination.CombineFile(fileName).Delete();
            }
        }
    }
}

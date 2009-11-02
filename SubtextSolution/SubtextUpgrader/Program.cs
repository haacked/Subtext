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
using System.IO;
using System.Reflection;

namespace SubtextUpgrader
{
    /// <summary>
    /// This tool is used to help upgrade existing installations of 
    /// Subtext to the latest version.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            string destinationPath;
            if(args.Length == 0)
            {
                Console.WriteLine("Please enter the destination path and hit ENTER:");
                destinationPath = Console.ReadLine();
            }
            else
            {
                destinationPath = args[0];
            }

            var sourceDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            var destinationDirectory = new DirectoryInfo(destinationPath);
            
            var skinUpgrader = new SkinUpgrader();
            skinUpgrader.UpgradeCustomSkins(destinationDirectory);

            var deployer = new FileDeployer();
            deployer.CopyDirectory(sourceDirectory, destinationDirectory);
            deployer.RemoveOldDirectories(destinationDirectory);
        }
    }
}

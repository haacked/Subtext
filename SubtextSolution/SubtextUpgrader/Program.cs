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
            if(args.Length == 0)
            {
                Console.WriteLine("SubtextUpgrader.exe");
                Console.WriteLine("Utility for upgrading Subtext instances");
                Console.WriteLine();
                Console.Write(CommandLineParser.CommandLineParser.GetInfo<Settings>());
                return;
            }
            var settings = CommandLineParser.CommandLineParser.Parse<Settings>(args);
            var program = new Program(settings);
            program.Run();
        }

        public Program(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings
        {
            get; 
            private set;
        }

        public void Run()
        {
            string destinationPath = Settings.UpgradeTargetDirectory;

            var sourceDirectory = new SubtextDirectory(Settings.SourceDirectory ?? Path.Combine(Assembly.GetExecutingAssembly().Location ?? ".", "Subtext.Web"));
            if(!VerifyDirectory(sourceDirectory, "source"))
            {
                return;
            }
            var destinationDirectory = new SubtextDirectory(destinationPath);
            if(!VerifyDirectory(destinationDirectory, "target"))
            {
                return;
            }

            IDirectory backup = null;
            if(!String.IsNullOrEmpty(Settings.BackupDirectory))
            {
                backup = new SubtextDirectory(Settings.BackupDirectory);
                if(!VerifyDirectory(backup, "backup"))
                {
                    return;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Upgrading using the following settings:");
            Console.WriteLine("");
            Console.WriteLine("\tSource Directory: '{0}'", Settings.SourceDirectory);
            Console.WriteLine("\tTarget Directory: '{0}", Settings.UpgradeTargetDirectory);
            if(backup != null)
            {
                Console.WriteLine("\tBackup Directory: '{0}", Settings.BackupDirectory);
            }
            Console.WriteLine(""); if(!Settings.QuietMode)
            {
                Console.WriteLine("");
                Console.WriteLine("Press 'Y' to continue or any other key to quit");
                var keyInfo = Console.ReadKey(false);
                if(keyInfo.KeyChar != 'y' && keyInfo.KeyChar != 'Y')
                {
                    return;
                }
            }
            Console.WriteLine();

            if(backup != null)
            {
                Console.WriteLine("Backing up source and target directories");
                backup.Delete(true);
                backup.Create();
                sourceDirectory.CopyTo(backup.Combine("source"));
                destinationDirectory.CopyTo(backup.Combine("target"));
            }

            var configUpgrader = new WebConfigUpgrader(sourceDirectory);
            Console.WriteLine("Upgrading Web.config");
            configUpgrader.UpgradeConfig(destinationDirectory);

            var skinsDirectory = sourceDirectory.Combine(@"Admin\Skins.config");
            LegacySkinsConfig skinConfig = skinsDirectory.GetCustomSkinsConfig();
            skinConfig.UpgradeSkins(destinationDirectory.Combine(@"pages\skins"));

            var deployer = new FileDeployer(sourceDirectory, destinationDirectory);
            deployer.Deploy();
            deployer.RemoveOldDirectories();
        }

        static bool VerifyDirectory(IDirectory directory, string directoryLabel)
        {
            if(!directory.Exists)
            {
                Console.WriteLine("The {0} directory: '{1}' does not exist", directoryLabel, directory.Path);
                return false;
            }
            return true;
        }
    }
}

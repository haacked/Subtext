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
        //TODO: Consider Replace Assembly="Subtext.Web.Controls" with Assembly="Subtext.Web" 
        //      in all skin files.

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
            var sourceDirectory = new SubtextDirectory(Settings.SourceDirectory ?? Path.Combine(Assembly.GetExecutingAssembly().Location ?? ".", "Subtext.Web"));
            if(!VerifyDirectory(sourceDirectory, "source"))
            {
                return;
            }

            var targetDirectory = new SubtextDirectory(Settings.UpgradeTargetDirectory);
            if(!VerifyDirectory(targetDirectory, "target"))
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
                Console.WriteLine("Clearing backup directory '{0}'", backup.Path);
                backup.Delete(true);
                backup.Create();
                Console.WriteLine("Backing up source and target directories");
                sourceDirectory.CopyTo(backup.Combine("source").Create());
                targetDirectory.CopyTo(backup.Combine("target").Create());
            }

            var configUpgrader = new WebConfigUpgrader(sourceDirectory);
            Console.WriteLine("Upgrading Web.config");
            configUpgrader.UpgradeConfig(targetDirectory);

            var customSkinConfig = targetDirectory.CombineFile(@"Admin\Skins.User.config");
            if(customSkinConfig.Exists)
            {
                Console.WriteLine("Updating skin.config for custom skins");
                var skinConfig = new LegacySkinsConfig(customSkinConfig);
                var skinsDirectory = sourceDirectory.Combine(@"pages\skins").Ensure();
                skinConfig.UpgradeSkins(skinsDirectory);
            }
            else
            {
                Console.WriteLine("Did not find custom skins file at '{0}'", customSkinConfig.Path);
            }

            Console.WriteLine("Deploying '{0}' to '{1}'", sourceDirectory.Path, targetDirectory.Path);
            var deployer = new FileDeployer(sourceDirectory, targetDirectory);
            deployer.Deploy();
            Console.WriteLine("Cleaning up old directories");
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

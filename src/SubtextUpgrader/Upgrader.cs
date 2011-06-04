using System;
using System.ComponentModel;

namespace SubtextUpgrader
{
    public class Upgrader
    {
        public Upgrader(Settings settings, BackgroundWorker progress)
        {
            Settings = settings;
            Progress = progress;
        }

        public Settings Settings
        {
            get;
            private set;
        }

        protected BackgroundWorker Progress
        {
            get;
            private set;
        }

        public void Run()
        {
            //assuming sourceDirectory is set, either explicitly or by unzipped resources.
            var sourceDirectory = new SubtextDirectory(Settings.SourceDirectory);
            if (!VerifyDirectory(sourceDirectory, "source"))
            {
                return;
            }

            var targetDirectory = new SubtextDirectory(Settings.UpgradeTargetDirectory);
            if (!VerifyDirectory(targetDirectory, "target"))
            {
                return;
            }

            IDirectory backup = null;
            if (!String.IsNullOrEmpty(Settings.BackupDirectory))
            {
                backup = new SubtextDirectory(Settings.BackupDirectory);
                if (!VerifyDirectory(backup, "backup"))
                {
                    return;
                }
            }

            string message = "Upgrading using the following settings:";

            Progress.ReportProgress(10, message);

            if (UserCancelled())
                return;

            message = string.Format("Source Directory: '{0}'", Settings.SourceDirectory);

            Progress.ReportProgress(10, message);

            if (UserCancelled())
                return;

            message = string.Format("Target Directory: '{0}", Settings.UpgradeTargetDirectory);

            Progress.ReportProgress(10, message);

            if (UserCancelled())
                return;

            if (backup != null)
            {
                message = string.Format("Backup Directory: '{0}", Settings.BackupDirectory);

                Progress.ReportProgress(20, message);

                if (UserCancelled())
                    return;

                message = string.Format("Clearing backup directory '{0}'", backup.Path);

                Progress.ReportProgress(20, message);

                if (UserCancelled())
                    return;

                backup.Delete(true);
                backup.Create();
                message = "Backing up source and target directories";

                Progress.ReportProgress(20, message);

                if (UserCancelled())
                    return;

                sourceDirectory.CopyTo(backup.Combine("source").Create());
                targetDirectory.CopyTo(backup.Combine("target").Create());
            }

            var configUpgrader = new WebConfigUpgrader(sourceDirectory);
            message = "Upgrading Web.config";

            Progress.ReportProgress(30, message);

            if (UserCancelled())
                return;

            configUpgrader.UpgradeConfig(targetDirectory);

            var customSkinConfig = targetDirectory.CombineFile(@"Admin\Skins.User.config");
            if (customSkinConfig.Exists)
            {
                message = "Updating skin.config for custom skins";

                Progress.ReportProgress(60, message);

                if (UserCancelled())
                    return;

                var skinConfig = new LegacySkinsConfig(customSkinConfig);
                var skinsDirectory = sourceDirectory.Combine(@"Skins").Ensure();
                skinConfig.UpgradeSkins(skinsDirectory);
            }
            else
            {
                message = string.Format("Did not find custom skins file at '{0}'", customSkinConfig.Path);

                Progress.ReportProgress(60, message);

                if (UserCancelled())
                    return;
            }

            message = string.Format("Deploying '{0}' to '{1}'", sourceDirectory.Path, targetDirectory.Path);

            Progress.ReportProgress(80, message);

            if (UserCancelled())
                return;

            var deployer = new FileDeployer(sourceDirectory, targetDirectory);
            deployer.Deploy();

            message = "Cleaning up old directories";

            Progress.ReportProgress(90, message);

            if (UserCancelled())
                return;

            deployer.RemoveOldDirectories();


            message = "Checking skins for references to legacy Subtext.Web.Controls assembly.";
            Progress.ReportProgress(95, message);

            var skinUpgrader = new SkinUpgrader(targetDirectory.Combine("Skins"));
            skinUpgrader.Run();


            message = "Done!";

            Progress.ReportProgress(100, message);
        }

        bool UserCancelled()
        {
            if (Progress.CancellationPending)
            {
                const string message = "User Cancelled!";
                Progress.ReportProgress(100, message);
            }

            return Progress.CancellationPending;
        }

        bool VerifyDirectory(IDirectory directory, string directoryLabel)
        {
            if (Settings.Verbose)
            {
                string message = "Verifying the " + directoryLabel +
                    " directory...";
                Progress.ReportProgress(10, message);
            }

            if (!directory.Exists)
            {
                string message = string.Format("The {0} directory: '{1}' does not exist", directoryLabel, directory.Path);

                Progress.ReportProgress(0, message);

                return false;
            }
            return true;
        }
    }
}

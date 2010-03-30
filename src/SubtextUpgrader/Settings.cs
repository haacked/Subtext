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

namespace SubtextUpgrader
{
    public class Settings
    {
		public Settings() { }

		public Settings(string sourceDirectory, string upgradeTargetDirectory, bool quietMode, string backupDirectory, bool verbose) : this() {
			SourceDirectory = sourceDirectory;
			UpgradeTargetDirectory = upgradeTargetDirectory;
			QuietMode = quietMode;
			BackupDirectory = backupDirectory;
			Verbose = verbose;
		}

        public string SourceDirectory { get; private set; }

        public string UpgradeTargetDirectory { get; private set; }

        public bool QuietMode { get; private set; }

        public string BackupDirectory { get; private set; }

        public bool Verbose { get; private set; }
    }
}

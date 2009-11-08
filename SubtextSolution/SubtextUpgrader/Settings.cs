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

using SubtextUpgrader.CommandLineParser.Attributes;
using System.ComponentModel;

namespace SubtextUpgrader
{
    public class Settings
    {
        [OptionName("s"), DefaultValue("'Subtext.Web' folder in the current directory"), Description("Source directory with new Subtext files")]
        public string SourceDirectory { get; private set; }

        [OptionName("t"), Description("The target directory to upgrade.")]
        public string UpgradeTargetDirectory { get; private set; }

        [OptionName("q"), Description("Quiet mode, do not prompt to continue.")]
        public bool QuietMode { get; private set; }

        [OptionName("b"), Description("Backup directory.")]
        public string BackupDirectory { get; private set; }

        [OptionName("v"), Description("Enables verbose logging")]
        public bool Verbose { get; private set; }
    }
}

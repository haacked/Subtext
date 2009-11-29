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
using System.Text.RegularExpressions;

namespace Subtext.Framework.Infrastructure.Installation
{
    internal class InstallationScriptInfo
    {
        //Have the compiled regex as static to get the full benefit of compilation
        private static readonly Regex ScriptParseRegex =
            new Regex(@"(?<ScriptName>Installation\.(?<version>\d+\.\d+\.\d+)\.sql)$",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private InstallationScriptInfo(string scriptName, Version version)
        {
            Version = version;
            ScriptName = scriptName;
        }

        public string ScriptName { get; set; }

        public Version Version { get; set; }

        internal static InstallationScriptInfo Parse(string resourceName)
        {
            Match match = ScriptParseRegex.Match(resourceName);
            if(!match.Success)
            {
                return null;
            }
            var version = new Version(match.Groups["version"].Value);
            string scriptName = match.Groups["ScriptName"].Value;
            return new InstallationScriptInfo(scriptName, version);
        }
    }

}

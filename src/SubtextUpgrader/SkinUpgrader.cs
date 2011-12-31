using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtextUpgrader
{
    /// <summary>
    /// Subtext 2.1 skins rely on controls defined in the assembly
    /// Subtext.Web.Controls. In Sbtext 2.5, those controls were moved to
    /// the assembly Subtext.Web. This class runs through all aspx and 
    /// ascx files in the "Skins" directory and updates any references
    /// to Subtext.Web.Controls assembly to point to Subtext.Web.
    /// </summary>
    public class SkinUpgrader
    {
        private readonly IDirectory _skinsDirectory;

        public SkinUpgrader(IDirectory skinsDirectory)
        {
            _skinsDirectory = skinsDirectory;
        }

        public void Run()
        {
            Run(_skinsDirectory);
        }

        private static void Run(IDirectory directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (file.Name.EndsWith(".aspx", StringComparison.CurrentCultureIgnoreCase)
                    || file.Name.EndsWith(".ascx", StringComparison.CurrentCultureIgnoreCase))
                {
                    var contents = file.Contents;
                    string newContent = GetUpgradedSkinFileContents(contents);
                    if (contents != newContent)
                    {
                        using (var stream = new StreamWriter(file.OpenWrite()))
                        {
                            stream.Write(newContent);
                            stream.Close();
                        }
                    }
                }
            }
            foreach (var dir in directory.GetDirectories())
            {
                Run(dir);
            }
        }

        public static string GetUpgradedSkinFileContents(string content)
        {
            string newContent = ReplaceLegacyControlTags(content);
            newContent = ReplaceDeprecatedSecurityHelperCalls(newContent);
            return newContent;
        }

        private static string ReplaceDeprecatedSecurityHelperCalls(string content)
        {
            var regex = new Regex(@"(?<![a-zA-Z])User.IsAdministrator\(\)(?![a-zA-Z])", RegexOptions.Compiled);
            return regex.Replace(content, "User.IsAdministrator()");
        }

        private static string ReplaceLegacyControlTags(string contents)
        {
            var regex = new Regex
                (@"(<%@\s*?Register\s+?TagPrefix="")(.+?)(""\s+?Namespace="".+?""\s+?Assembly="")(Subtext.Web.Controls)(""\s*?%>)"
                , RegexOptions.IgnoreCase | RegexOptions.Multiline
                );

            var newContent = regex.Replace(contents, delegate(Match m)
            {
                if (m.Groups[2].Value.Equals("st", StringComparison.CurrentCultureIgnoreCase))
                {
                    return string.Empty;
                }
                var sb = new StringBuilder();

                sb.Append(m.Groups[1].Value);
                sb.Append(m.Groups[2].Value);
                sb.Append(m.Groups[3].Value);
                sb.Append("Subtext.Web");
                sb.Append(m.Groups[5].Value);

                return sb.ToString();
            });
            return newContent;
        }
    }
}
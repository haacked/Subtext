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
using System.Text;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Contains helper methods for unpacking scripts 
    /// embedded as assembly resources.
    /// </summary>
    public static class ScriptHelper
    {
        /// <summary>
        /// Returns a string representation of the specified embedded 
        /// script file.  The script is wrapped with script start and end tags 
        /// and assumes the script language is "vbscript" if the file extension 
        /// is ".vbs" and "javascript" otherwise.
        /// </summary>
        /// <remarks>
        /// Using a naming convention, all scripts are placed in the Resources\Scripts 
        /// folder. The ScriptName should just be the filename of the script.  For example, 
        /// if you embed a file at the following location "Resources\Scripts\MyScript.js", 
        /// the ScriptName to pass is "MyScript.js".
        /// </remarks>
        /// <param name="scriptName">FileName of the script.  Just the file name.</param>
        /// <returns>Contents of the script.</returns>
        public static string UnpackScript(string scriptName)
        {
            string language = "javascript";
            string extension = Path.GetExtension(scriptName);

            if (String.Equals(extension, ".vbs", StringComparison.OrdinalIgnoreCase))
            {
                language = "vbscript";
            }

            return UnpackScript(scriptName, language);
        }

        /// <summary>
        /// Returns a string representation of the specified embedded 
        /// script file.  The script is wrapped with script start and end tags 
        /// and assumes the script language is "javascript".
        /// </summary>
        /// <remarks>
        /// Using a naming convention, all scripts are placed in the Resources\Scripts 
        /// folder. The ScriptName should just be the filename of the script.  For example, 
        /// if you embed a file at the following location "Resources\Scripts\MyScript.js", 
        /// the ScriptName to pass is "MyScript.js".
        /// </remarks>
        /// <param name="scriptName">FileName of the script.  Just the file name.</param>
        /// <param name="scriptLanguage">The script language.</param>
        /// <returns>Contents of the script.</returns>
        public static string UnpackScript(string scriptName, string scriptLanguage)
        {
            return string.Format("<script type=\"text/{0}\">{1}{2}{3}</script>", scriptLanguage, Environment.NewLine, UnpackEmbeddedResourceToString(string.Format("Resources.{0}", scriptName)), Environment.NewLine);
        }

        /// <summary>
        /// Unpacks the embedded resource to string.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        static string UnpackEmbeddedResourceToString(string resourceName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = executingAssembly.GetManifestResourceStream(typeof(ScriptHelper), resourceName);
            if (resourceStream == null)
            {
                return string.Empty;
            }
            using (var reader = new StreamReader(resourceStream, Encoding.ASCII))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
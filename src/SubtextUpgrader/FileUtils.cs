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

using System.Xml;
using System.IO;

namespace SubtextUpgrader
{
    public static class FileUtils
    {
        public static IFile CreateXmlFile(this IDirectory directory, string filename, XmlDocument xml)
        {
            IFile file = directory.CombineFile(filename);
            using(var stream = file.OpenWrite())
            {
                xml.Save(stream);
            }
            return file;
        }

        public static string ToStringContents(this MemoryStream stream)
        {
            stream.Position = 0;
            using(var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

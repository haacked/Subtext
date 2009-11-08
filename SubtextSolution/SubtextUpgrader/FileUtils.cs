using System.Xml;
using System.IO;

namespace SubtextUpgrader
{
    public static class FileUtils
    {
        public static IFile CreateXmlFile(this IDirectory directory, string filename, XmlDocument xml)
        {
            IFile file = directory.CombineFile(filename);
            xml.Save(file.OpenWrite());
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

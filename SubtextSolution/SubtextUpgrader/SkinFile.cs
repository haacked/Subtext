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

using System.IO;

namespace SubtextUpgrader
{
    public class SkinFile : IFile
    {
        public SkinFile(FileInfo file)
        {
            File = file;
        }

        protected FileInfo File
        {
            get; 
            private set;
        }

        public string Contents
        {
            get
            {
               if(_contents == null)
               {
                   using(var stream = File.OpenRead())
                   using(var reader = new StreamReader(stream))
                   {
                       _contents = reader.ReadToEnd();
                   }
               }
               return _contents;
            }
        }

        string _contents;

        public Stream OpenWrite()
        {
            return File.OpenWrite();
        }
    }
}

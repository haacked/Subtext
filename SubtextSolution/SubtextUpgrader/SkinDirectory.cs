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
    public class SkinDirectory : IDirectory
    {
        public SkinDirectory(DirectoryInfo directory)
        {
            PhysicalDirectory = directory;
        }

        protected DirectoryInfo PhysicalDirectory
        {
            get; 
            private set;
        }

        public bool Exists
        {
            get
            {
                return PhysicalDirectory.Exists;
            }
        }

        public string Path
        {
            get 
            {
                return PhysicalDirectory.FullName;
            }
        }

        public IDirectory Combine(string path)
        {
            return new SkinDirectory(new DirectoryInfo(CombinePath(path)));
        }

        public void Create()
        {
            PhysicalDirectory.Create();
        }

        public string CombinePath(string path)
        {
            return System.IO.Path.Combine(Path, path);
        }


        public IFile CombineFile(string path)
        {
            return new SkinFile(new FileInfo(CombinePath(path)));
        }
    }
}

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

using System.Collections.Generic;
using System.IO;

namespace SubtextUpgrader
{
    public class SubtextDirectory : IDirectory
    {
        public SubtextDirectory(string path) : this(new DirectoryInfo(path))
        {
        }

        public SubtextDirectory(DirectoryInfo directory)
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

        public string Name
        {
            get
            {
                return PhysicalDirectory.Name;
            }
        }

        public IDirectory Combine(string path)
        {
            return new SubtextDirectory(new DirectoryInfo(CombinePath(path)));
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
            return new SubtextFile(new FileInfo(CombinePath(path)));
        }

        public virtual IEnumerable<IFile> GetFiles()
        {
            foreach(var file in PhysicalDirectory.GetFiles())
            {
                yield return new SubtextFile(file);
            }
        }

        public virtual IEnumerable<IDirectory> GetDirectories()
        {
            foreach(var directory in PhysicalDirectory.GetDirectories())
            {
                yield return new SubtextDirectory(directory);
            }
        }

        public void Delete(bool recursive)
        {
            PhysicalDirectory.Delete(recursive);
        }

        public IDirectory CopyTo(IDirectory destination)
        {
            foreach(var file in GetFiles())
            {
                file.CopyTo(destination);
            }

            foreach(var subdir in GetDirectories())
            {
                var directoryName = subdir.Name;
                var destinationSubDir = destination.Combine(directoryName);
                EnsureDirectory(destinationSubDir);
                subdir.CopyTo(destinationSubDir);
            }
            return destination;
        }

        private static void EnsureDirectory(IDirectory destination)
        {
            if(!destination.Exists)
            {
                destination.Create();
            }
        }

    }
}

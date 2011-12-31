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
using System.Collections.Generic;
using System.IO;

namespace SubtextUpgrader
{
    public class SubtextDirectory : IDirectory
    {
        public SubtextDirectory(string path)
            : this(new DirectoryInfo(path))
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

        public IDirectory Parent
        {
            get
            {
                if (_parent == null)
                {
                    _parent = new SubtextDirectory(PhysicalDirectory.Parent);
                }
                return _parent;
            }
        }

        IDirectory _parent;

        public IDirectory Combine(string path)
        {
            return new SubtextDirectory(new DirectoryInfo(CombinePath(path)));
        }

        public IDirectory Create()
        {
            PhysicalDirectory.Create();
            return this;
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
            foreach (var file in PhysicalDirectory.GetFiles())
            {
                yield return new SubtextFile(file);
            }
        }

        public virtual IEnumerable<IDirectory> GetDirectories()
        {
            foreach (var directory in PhysicalDirectory.GetDirectories())
            {
                yield return new SubtextDirectory(directory);
            }
        }

        public void Delete(bool recursive)
        {
            if (PhysicalDirectory.Exists)
            {
                PhysicalDirectory.Delete(recursive);
            }
        }

        public IDirectory CopyTo(IDirectory destination)
        {
            //Console.WriteLine("Copying Directory '{0}' to '{1}", Path, destination.Path);

            return CopyTo(destination, f => true);
        }

        public IDirectory CopyTo(IDirectory destination, Predicate<IFile> shouldOverwriteExistingFile)
        {
            foreach (var file in GetFiles())
            {
                var destinationFile = destination.CombineFile(file.Name);

                if (!destinationFile.Exists || shouldOverwriteExistingFile(file))
                    file.CopyTo(destination);

            }

            foreach (var subdir in GetDirectories())
            {
                var directoryName = subdir.Name;
                var destinationSubDir = destination.Combine(directoryName);
                destinationSubDir.Ensure();
                subdir.CopyTo(destinationSubDir);
            }

            return destination;
        }

        public IDirectory Ensure()
        {
            if (!Exists)
            {
                Create();
            }
            return this;
        }
    }
}

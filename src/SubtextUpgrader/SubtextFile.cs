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
    public class SubtextFile : IFile
    {
        public SubtextFile(FileInfo file)
        {
            File = file;
            Path = file.FullName;
            Name = file.Name;
        }

        protected FileInfo File
        {
            get; 
            private set;
        }

        public string Path
        {
            get; 
            private set;
        }

        public string Name
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

        public bool Exists
        {
            get
            {
                return File.Exists;
            }
        }

        public IDirectory Directory
        {
            get
            {
                if(_directory == null)
                {
                    _directory = new SubtextDirectory(File.Directory);
                }
                return _directory;
            }
        }

        IDirectory _directory;

        public string CombinePath(string fileName)
        {
            return Directory.CombinePath(fileName);
        }

        public IFile CopyTo(string path)
        {
            return new SubtextFile(File.CopyTo(path, true /*overwrite*/));
        }

        public IFile CopyTo(IDirectory directory)
        {
            return Overwrite(directory.CombineFile(Name));
        }

        public IFile Overwrite(IFile file)
        {
            return CopyTo(file.Path);
        }

        public IFile Backup(string fileName)
        {
            return new SubtextFile(File.CopyTo(Directory.CombinePath(fileName), true /*overwrite*/));
        }

        public void Delete()
        {
            if(File.Exists)
            {
                File.Delete();
            }
        }
    }
}

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

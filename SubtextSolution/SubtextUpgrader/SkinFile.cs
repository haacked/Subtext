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

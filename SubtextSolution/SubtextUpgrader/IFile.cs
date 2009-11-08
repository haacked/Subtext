using System.IO;

namespace SubtextUpgrader
{
    public interface IFile
    {
        string Contents { get; }
        Stream OpenWrite();
    }
}

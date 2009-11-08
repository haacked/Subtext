
namespace SubtextUpgrader
{
    public interface IDirectory
    {
        bool Exists { get; }
        string Path { get; }
        IDirectory Combine(string path);
        IFile CombineFile(string path);
        string CombinePath(string path);
        void Create();
    }
}

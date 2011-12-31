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
    public interface IFile
    {
        string Contents { get; }
        Stream OpenWrite();
        bool Exists { get; }
        string CombinePath(string path);
        IDirectory Directory { get; }
        IFile CopyTo(string path);
        IFile CopyTo(IDirectory directory);
        IFile Overwrite(IFile file);
        string Name { get; }
        string Path { get; }
        IFile Backup(string fileName);
        void Delete();
    }
}

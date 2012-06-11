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

namespace SubtextUpgrader
{
    public interface IDirectory
    {
        bool Exists { get; }
        string Name { get; }
        string Path { get; }
        IDirectory Parent { get; }
        IDirectory Combine(string path);
        IFile CombineFile(string fileName);
        string CombinePath(string path);
        IDirectory Create();
        IEnumerable<IFile> GetFiles();
        IEnumerable<IDirectory> GetDirectories();
        void Delete(bool recursive);
        IDirectory CopyTo(IDirectory directory);
        IDirectory CopyTo(IDirectory directory, Predicate<IFile> overwiteExistingFile);
        IDirectory Ensure();
    }
}

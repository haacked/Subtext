using System;
using NuGet;

namespace Subtext.Framework.Services.NuGet
{
    public class SubtextPackagePathResolver : DefaultPackagePathResolver
    {
        public SubtextPackagePathResolver(IFileSystem fileSystem)
            : base(fileSystem, useSideBySidePaths: true)
        {

        }

        public override string GetPackageDirectory(string packageId, Version version)
        {
            return string.Empty;
        }
    }
}

using NuGet;

namespace Subtext.Framework.Services.NuGet
{
    public class PackageInstallationState
    {
        public IPackage Installed { get; set; }
        public IPackage Update { get; set; }
    }
}

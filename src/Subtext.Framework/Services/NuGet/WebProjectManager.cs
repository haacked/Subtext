using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet;

namespace Subtext.Framework.Services.NuGet
{
    public class WebProjectManager
    {
        private readonly IProjectManager _projectManager;

        public WebProjectManager(string remoteSource, string siteRoot)
        {
            string webRepositoryDirectory = GetWebRepositoryDirectory(siteRoot);
            var fileSystem = new PhysicalFileSystem(webRepositoryDirectory);
            var packagePathResolver = new SubtextPackagePathResolver(fileSystem);
            _projectManager = new ProjectManager(sourceRepository: PackageRepositoryFactory.Default.CreateRepository(remoteSource),
                                       pathResolver: packagePathResolver,
                                       localRepository: new LocalPackageRepository(packagePathResolver, fileSystem),
                                       project: new WebProjectSystem(siteRoot));
        }

        public IPackageRepository LocalRepository
        {
            get
            {
                return _projectManager.LocalRepository;
            }
        }

        public IPackageRepository SourceRepository
        {
            get
            {
                return _projectManager.SourceRepository;
            }
        }

        public IQueryable<IPackage> GetRemotePackages(string searchTerms)
        {
            return GetPackages(SourceRepository, searchTerms);
        }

        public IQueryable<IPackage> GetInstalledPackages(string searchTerms)
        {
            return GetPackages(LocalRepository, searchTerms);
        }

        public IQueryable<IPackage> GetPackagesWithUpdates(string searchTerms)
        {
            IQueryable<IPackage> packages = LocalRepository.GetUpdates(SourceRepository.GetPackages()).AsQueryable();
            return GetPackages(packages, searchTerms);
        }

        /// <summary>
        /// Installs and adds a package reference to the project
        /// </summary>
        /// <returns>Warnings encountered when installing the package.</returns>
        public IEnumerable<string> InstallPackage(IPackage package)
        {
            return PerformLoggedAction(() =>
            {
                _projectManager.AddPackageReference(package.Id, package.Version, ignoreDependencies: false);
            });
        }

        /// <summary>
        /// Updates a package reference. Installs the package to the App_Data repository if it does not already exist.
        /// </summary>
        /// <returns>Warnings encountered when updating the package.</returns>
        public IEnumerable<string> UpdatePackage(IPackage package)
        {
            return PerformLoggedAction(() =>
            {
                _projectManager.UpdatePackageReference(package.Id, package.Version, updateDependencies: true);
            });
        }
        /// <summary>
        /// Removes a package reference and uninstalls the package
        /// </summary>
        /// <returns>Warnings encountered when uninstalling the package.</returns>
        public IEnumerable<string> UninstallPackage(IPackage package, bool removeDependencies)
        {
            return PerformLoggedAction(() =>
            {
                _projectManager.RemovePackageReference(package.Id, forceRemove: false, removeDependencies: removeDependencies);
            });
        }

        public bool IsPackageInstalled(IPackage package)
        {
            return LocalRepository.Exists(package);
        }

        public IPackage GetUpdate(IPackage package)
        {
            return SourceRepository.GetUpdates(LocalRepository.GetPackages()).FirstOrDefault(p => package.Id == p.Id);
        }

        public IEnumerable<IPackage> GetUpdates()
        {
            return SourceRepository.GetUpdates(LocalRepository.GetPackages());
        }

        private IEnumerable<string> PerformLoggedAction(Action action)
        {
            ErrorLogger logger = new ErrorLogger();
            _projectManager.Logger = logger;
            try
            {
                action();
            }
            finally
            {
                _projectManager.Logger = null;
            }
            return logger.Errors;
        }

        internal IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package)
        {
            return GetPackagesRequiringLicenseAcceptance(package, localRepository: LocalRepository, sourceRepository: SourceRepository);
        }

        internal static IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
        {
            var dependencies = GetPackageDependencies(package, localRepository, sourceRepository);

            return from p in dependencies
                   where p.RequireLicenseAcceptance
                   select p;
        }

        private static IEnumerable<IPackage> GetPackageDependencies(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
        {
            InstallWalker walker = new InstallWalker(localRepository: localRepository, sourceRepository: sourceRepository, logger: NullLogger.Instance, ignoreDependencies: false);
            IEnumerable<PackageOperation> operations = walker.ResolveOperations(package);

            return from operation in operations
                   where operation.Action == PackageAction.Install
                   select operation.Package;
        }

        internal static IQueryable<IPackage> GetPackages(IPackageRepository repository, string searchTerm)
        {
            return GetPackages(repository.GetPackages(), searchTerm);
        }

        internal static IQueryable<IPackage> GetPackages(IQueryable<IPackage> packages, string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                packages = packages.Find(searchTerm.Split());
            }
            return packages;
        }

        internal static string GetWebRepositoryDirectory(string siteRoot)
        {
            return Path.Combine(siteRoot, "App_Data", "packages");
        }

        private class ErrorLogger : ILogger
        {
            private readonly IList<string> _errors = new List<string>();

            public IEnumerable<string> Errors
            {
                get
                {
                    return _errors;
                }
            }

            public void Log(MessageLevel level, string message, params object[] args)
            {
                if (level == MessageLevel.Warning)
                {
                    _errors.Add(String.Format(CultureInfo.CurrentCulture, message, args));
                }
            }
        }
    }
}
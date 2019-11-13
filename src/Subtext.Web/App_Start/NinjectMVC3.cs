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
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Store;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Mvc;
using Subtext.Configuration;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Emoticons;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.HttpModules;
using Subtext.ImportExport;
using Subtext.Infrastructure;
using Subtext.Web.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Subtext.Web.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Subtext.Web.App_Start.NinjectMVC3), "Stop")]
namespace Subtext.Web.App_Start
{
    public static class NinjectMVC3
    {
        private static readonly Ninject.Web.Mvc.Bootstrapper bootstrapper = new Ninject.Web.Mvc.Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public static void RegisterServices(IKernel kernel)
        {
            // Main Services
            kernel.Bind<ITextTransformation>().ToMethod(context =>
            {
                var transform = new CompositeTextTransformation
                    {
                        context.Kernel.Get<XhtmlConverter>(),
                        context.Kernel.Get<EmoticonsTransformation>(),
                        context.Kernel.Get<KeywordExpander>()
                    };
                return transform;
            }).InRequestScope();
            kernel.Bind<IBlogLookupService>().To<BlogLookupService>().InSingletonScope();
            kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();
            kernel.Bind<ICommentFilter>().To<CommentFilter>().InRequestScope();
            kernel.Bind<IStatisticsService>().To<StatisticsService>().InRequestScope();
            kernel.Bind<ICommentSpamService>().To<AkismetSpamService>()
                .When(r => !String.IsNullOrEmpty(r.ParentContext.Kernel.Get<Blog>().FeedbackSpamServiceKey))
                .InRequestScope()
                .WithConstructorArgument("apiKey", c => c.Kernel.Get<Blog>().FeedbackSpamServiceKey)
                .WithConstructorArgument("akismetClient", c => null);

            kernel.Bind<ICommentSpamService>().To<NullSpamService>()
                .When(r => String.IsNullOrEmpty(r.ParentContext.Kernel.Get<Blog>().FeedbackSpamServiceKey))
                .InRequestScope();

            var indexingSettings = FullTextSearchEngineSettings.Settings;

            if (indexingSettings.IsEnabled)
            {
                kernel.Bind<Lucene.Net.Store.Directory>()
                    .ToMethod(c => FSDirectory.Open(new DirectoryInfo(HostingEnvironment.MapPath(indexingSettings.IndexFolderLocation))))
                    .InSingletonScope();
                kernel.Bind<Analyzer>().To<SnowballAnalyzer>().InSingletonScope()
                    .WithConstructorArgument("name", indexingSettings.Language)
                    .WithConstructorArgument("stopSet", indexingSettings.StopWords);
            }

            BindCoreDependencies(kernel);
            BindGenericDependencies(kernel);
            BindHttpModules(kernel);
        }

        private static void BindCoreDependencies(IKernel kernel)
        {
            BindBlogMLDependencies(kernel);

            kernel.Bind<RequestContext>().ToMethod(c => HttpContext.Current.Request.RequestContext).InRequestScope();
            kernel.Bind<IControllerActivator>().To<SubtextControllerActivator>().InSingletonScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
            kernel.Bind<IEntryPublisher>().To<EntryPublisher>().InRequestScope();
            kernel.Bind<FriendlyUrlSettings>().ToMethod(context => FriendlyUrlSettings.Settings).InRequestScope();
            kernel.Bind<FullTextSearchEngineSettings>().ToMethod(context => FullTextSearchEngineSettings.Settings).InRequestScope();
            kernel.Bind<ISubtextPageBuilder>().To<SubtextPageBuilder>().InSingletonScope();
            kernel.Bind<ISlugGenerator>().To<SlugGenerator>().InRequestScope();
            kernel.Bind<IPrincipal>().ToMethod(context => context.Kernel.Get<RequestContext>().HttpContext.User).InRequestScope();
            kernel.Bind<Blog>().ToMethod(c => BlogRequest.Current.Blog).When(r => BlogRequest.Current.Blog != null).InRequestScope();
            kernel.Bind<ObjectRepository>().ToMethod(c => new DatabaseObjectProvider()).InRequestScope();
            kernel.Bind<ICache>().To<SubtextCache>().InRequestScope();
            kernel.Bind<Cache>().ToMethod(c => HttpContext.Current.Cache).InRequestScope();
            kernel.Bind<OpmlWriter>().To<OpmlWriter>().InRequestScope();
            kernel.Bind<IKernel>().ToMethod(context => context.Kernel).InSingletonScope();
            kernel.Bind<Tracking>().ToMethod(context => Config.Settings.Tracking).InSingletonScope();
            kernel.Bind<IInstallationManager>().To<InstallationManager>().InRequestScope();
            kernel.Bind<IInstaller>().ToMethod(context => new SqlInstaller(Config.ConnectionString)).InRequestScope();
            kernel.Bind<ISubtextContext>().To<SubtextContext>().InRequestScope();
            kernel.Bind<LazyNotNull<HostInfo>>().ToMethod(context => new LazyNotNull<HostInfo>(() => HostInfo.Instance)).InSingletonScope();

            var indexingSettings = FullTextSearchEngineSettings.Settings;
            if (indexingSettings.IsEnabled)
            {
                kernel.Bind<IIndexingService>().To<IndexingService>().InSingletonScope();
                kernel.Bind<ISearchEngineService>().To<SearchEngineService>().InSingletonScope();
            }
            else
            {
                kernel.Bind<IIndexingService>().To<NoOpIndexingService>().InSingletonScope();
                kernel.Bind<ISearchEngineService>().To<NoOpSearchEngineService>().InSingletonScope();
            }
        }

        private static void BindGenericDependencies(IKernel kernel)
        {
            kernel.Bind<NameValueCollection>().ToMethod(c => ConfigurationManager.AppSettings).When(c => c.Target.Name == "appSettings").InRequestScope();
        }

        private static void BindBlogMLDependencies(IKernel kernel)
        {
            kernel.Bind<IBlogImportRepository>().To<BlogImportRepository>().InRequestScope();
            kernel.Bind<IBlogImportService>().To<BlogImportService>().InRequestScope();
            kernel.Bind<IBlogMLImportMapper>().To<BlogMLImportMapper>().InRequestScope();
            kernel.Bind<IBlogMLExportMapper>().To<BlogMLExportMapper>().InRequestScope();
            kernel.Bind<IBlogMLSource>().To<BlogMLSource>().InRequestScope();
        }

        private static void BindHttpModules(IKernel kernel)
        {
            kernel.Bind<IHttpModule>().To<BlogRequestModule>();
            kernel.Bind<IHttpModule>().To<FormToBasicAuthenticationModule>();
            kernel.Bind<IHttpModule>().To<AuthenticationModule>();
            kernel.Bind<IHttpModule>().To<InstallationCheckModule>();
            kernel.Bind<IHttpModule>().To<CompressionModule>();
        }
    }
}

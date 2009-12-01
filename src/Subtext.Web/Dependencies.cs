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
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Subtext.Configuration;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Emoticons;
using Subtext.Framework.Infrastructure;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.HttpModules;
using Subtext.ImportExport;
using Subtext.Infrastructure;

namespace Subtext
{
    public class Dependencies : NinjectModule
    {
        public override void Load()
        {
            // Main Services
            Bind<ITextTransformation>().ToMethod(context =>
                {
                    var transform = new CompositeTextTransformation
                    {
                        context.Kernel.Get<XhtmlConverter>(),
                        context.Kernel.Get<EmoticonsTransformation>(),
                        context.Kernel.Get<KeywordExpander>()
                    };
                    return transform;
                }).InRequestScope();
            Bind<ICommentService>().To<CommentService>().InRequestScope();
            Bind<ICommentFilter>().To<CommentFilter>().InRequestScope();
            Bind<IStatisticsService>().To<StatisticsService>().InRequestScope();
            Bind<ICommentSpamService>().To<AkismetSpamService>()
                .When(r => !String.IsNullOrEmpty(r.ParentContext.Kernel.Get<Blog>().FeedbackSpamServiceKey))
                .InRequestScope()
                .WithConstructorArgument("apiKey", c => c.Kernel.Get<Blog>().FeedbackSpamServiceKey)
                .WithConstructorArgument("akismetClient", c => null);

            // Dependencies you're less likely to change.
            LoadCoreDependencies();
            LoadGenericDependencies();
        }

        private void LoadCoreDependencies()
        {
            BindBlogMLDependencies();

            Bind<IEntryPublisher>().To<EntryPublisher>().InRequestScope();
            Bind<FriendlyUrlSettings>().ToMethod(context => FriendlyUrlSettings.Settings).InRequestScope();
            Bind<ISubtextPageBuilder>().To<SubtextPageBuilder>().InSingletonScope();
            Bind<ISlugGenerator>().To<SlugGenerator>().InRequestScope();
            Bind<FriendlyUrlSettings>().To<FriendlyUrlSettings>().InRequestScope();
            Bind<IPrincipal>().ToMethod(context => context.Kernel.Get<RequestContext>().HttpContext.User).InRequestScope();
            Bind<Blog>().ToMethod(c => BlogRequest.Current.Blog).When(r => BlogRequest.Current.Blog != null).InRequestScope();
            Bind<ObjectProvider>().ToMethod(c => new DatabaseObjectProvider()).InRequestScope();
            Bind<ICache>().To<SubtextCache>().InRequestScope();
            Bind<Cache>().ToMethod(c => HttpContext.Current.Cache).InRequestScope();
            Bind<OpmlWriter>().To<OpmlWriter>().InRequestScope();
            Bind<IKernel>().ToMethod(context => context.Kernel).InSingletonScope();
            Bind<Tracking>().ToMethod(context => Config.Settings.Tracking).InSingletonScope();
            Bind<IInstallationManager>().To<InstallationManager>();
            Bind<IInstaller>().ToMethod(context => new SqlInstaller(Config.ConnectionString));
            Bind<RouteCollection>().ToConstant(RouteTable.Routes).InSingletonScope();
            Bind<HttpContext>().ToMethod(c => HttpContext.Current).InRequestScope();
            Bind<ISubtextContext>().To<SubtextContext>().InRequestScope();
            Bind<RequestContext>().ToMethod(c => Bootstrapper.RequestContext).InRequestScope();
            Bind<IServiceLocator>().To<NinjectServiceLocator>().InSingletonScope();
        }

        private void LoadGenericDependencies()
        {
            Bind<NameValueCollection>().ToMethod(c => ConfigurationManager.AppSettings).When(c => c.Target.Name == "appSettings").InRequestScope();
        }

        private void BindBlogMLDependencies()
        {
            Bind<IBlogImportRepository>().To<BlogImportRepository>().InRequestScope();
            Bind<IBlogImportService>().To<BlogImportService>().InRequestScope();
            Bind<IBlogMLImportMapper>().To<BlogMLImportMapper>().InRequestScope();
            Bind<IBlogMLExportMapper>().To<BlogMLExportMapper>().InRequestScope();
            Bind<IBlogMLSource>().To<BlogMLSource>().InRequestScope();
        }
    }
}
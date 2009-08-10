using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Subtext.Configuration;
using Subtext.Framework;
using Subtext.Framework.Data;
using Subtext.Framework.Emoticons;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;
using Subtext.Infrastructure;
using Subtext.Framework.Syndication;

namespace Subtext
{
    public class Dependencies : Module
    {
        public override void Load()
        {
            // Main Services
            Bind<IEnumerable<ITextTransformation>>().ToMethod(context => new ITextTransformation[] {
                context.Kernel.Get<XhtmlConverter>(),
                context.Kernel.Get<EmoticonsTransformation>(),
                context.Kernel.Get<KeywordExpander>()
            } ).InRequestScope();
            Bind<ICommentService>().To<CommentService>().InRequestScope();
            Bind<ICommentFilter>().To<CommentFilter>().InRequestScope();
            Bind<IStatisticsService>().To<StatisticsService>().InRequestScope();
            Bind<ICommentSpamService>().To<AkismetSpamService>().InRequestScope()
                .WithConstructorArgument("apiKey", c => c.Kernel.Get<Blog>().FeedbackSpamServiceKey)
                .WithConstructorArgument("akismetClient", c => null);

            // Dependencies you're less likely to change.
            LoadCoreDependencies();
        }

        private void LoadCoreDependencies()
        {
            Bind<ISubtextPageBuilder>().To<SubtextPageBuilder>().InSingletonScope();
            Bind<ISlugGenerator>().To<SlugGenerator>().InRequestScope();
            Bind<FriendlyUrlSettings>().To<FriendlyUrlSettings>().InRequestScope();
            Bind<IPrincipal>().ToMethod(context => context.Kernel.Get<RequestContext>().HttpContext.User).InRequestScope();
            Bind<Blog>().ToMethod(c => BlogRequest.Current.Blog).When(r => BlogRequest.Current.Blog != null).InRequestScope();
            Bind<ObjectProvider>().ToMethod(c => new DatabaseObjectProvider()).InRequestScope();
            Bind<Subtext.Infrastructure.ICache>().To<SubtextCache>().InRequestScope();
            Bind<System.Web.Caching.Cache>().ToMethod(c => HttpContext.Current.Cache).InRequestScope();
            Bind<OpmlWriter>().To<OpmlWriter>().InRequestScope();
            Bind<IKernel>().ToMethod(context => context.Kernel).InSingletonScope();

            Bind<RouteCollection>().ToConstant(RouteTable.Routes).InSingletonScope();
            Bind<HttpContext>().ToMethod(c => HttpContext.Current).InRequestScope();
            Bind<ISubtextContext>().To<SubtextContext>().InRequestScope();
            Bind<RequestContext>().ToMethod(c => Bootstrapper.RequestContext).InRequestScope();
        }
    }
}
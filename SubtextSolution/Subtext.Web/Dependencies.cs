using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Subtext.Framework;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;
using Subtext.Infrastructure;
using Ninject.Activation.Caching;

public class Dependencies : Module
{
    public override void Load()
    {
        // Main Services
        Bind<ICommentService>().To<CommentService>().InRequestScope();
        Bind<IFeedbackSpamService>().To<AkismetSpamService>().InRequestScope()
            .WithConstructorArgument("apiKey", c => c.Kernel.Get<Blog>().FeedbackSpamServiceKey)
            .WithConstructorArgument("akismetClient", c => null);
        
        // Dependencies you're less likely to change.
        LoadCoreDependencies();
    }

    private void LoadCoreDependencies() {
        Bind<IPrincipal>().ToMethod(c => c.Kernel.Get<RequestContext>().HttpContext.User);
        Bind<Blog>().ToMethod(c => BlogRequest.Current.Blog);
        Bind<ObjectProvider>().ToMethod(c => new DatabaseObjectProvider()).InRequestScope();
        Bind<Subtext.Framework.Data.ICache>().To<SubtextCache>().InRequestScope();
        Bind<System.Web.Caching.Cache>().ToMethod(c => HttpContext.Current.Cache).InRequestScope();
        
        Bind<RouteCollection>().ToConstant(RouteTable.Routes);
        Bind<HttpContext>().ToMethod(c => HttpContext.Current).InRequestScope();
        Bind<ISubtextContext>().To<SubtextContext>().InRequestScope();
        Bind<RequestContext>().ToMethod(c => Bootstrapper.RequestContext).InRequestScope();
    }
}

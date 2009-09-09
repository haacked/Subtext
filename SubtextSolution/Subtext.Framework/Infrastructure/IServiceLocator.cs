namespace Subtext.Framework.Infrastructure
{
    public interface IServiceLocator
    {
        TService GetService<TService>();
    }
}
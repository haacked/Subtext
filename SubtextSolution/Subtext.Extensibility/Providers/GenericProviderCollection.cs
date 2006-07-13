#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Configuration.Provider;

namespace Subtext.Extensibility.Providers
{
public class GenericProviderCollection<T> 
	: ProviderCollection 
	where T : System.Configuration.Provider.ProviderBase
{

    public new T this[string name]
    {
        get { return (T)base[name]; }
    }

    public override void Add(ProviderBase provider)
    {
        if (provider == null)
            throw new ArgumentNullException("provider");

        if (!(provider is T))
            throw new ArgumentException
                ("Invalid provider type", "provider");

        base.Add(provider);
    }
}
}

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
using System.Configuration.Provider;
using Subtext.Extensibility.Properties;

namespace Subtext.Extensibility.Providers
{
    /// <summary>
    /// Generic collection of Providers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericProviderCollection<T> : ProviderCollection where T : ProviderBase
    {
        /// <summary>
        /// Returns a provider by the specified section key.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new T this[string name]
        {
            get { return (T)base[name]; }
        }

        /// <summary>
        /// Adds a new provider to the collection.
        /// </summary>
        /// <param name="provider"></param>
        public override void Add(ProviderBase provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if(!(provider is T))
            {
                throw new ArgumentException(Resources.Argument_InvalidProviderType, "provider");
            }

            base.Add(provider);
        }
    }
}
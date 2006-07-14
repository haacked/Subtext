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
	/// <summary>
	/// Generic collection of Providers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericProviderCollection<T> : ProviderCollection where T : System.Configuration.Provider.ProviderBase
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
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (!(provider is T))
				throw new ArgumentException
					("Invalid provider type", "provider");

			base.Add(provider);
		}
	}
}

using System;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using Subtext.Framework.Util;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for BaseProvider.
	/// </summary>
	public abstract class BaseProvider
	{
		public BaseProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//public abstract object Instance();

		private string _type;

		[XmlAttribute("type")]
		public string ProviderType
		{
			get	{	return _type;	}
			set	{ _type = value;	}
		}

		public object Instance()
		{
			return Activator.CreateInstance(System.Type.GetType(this.ProviderType));
		}

		public static void SaveCache(string cacheKey, object obj)
		{
			SaveCache(cacheKey,obj,HttpContext.Current);

		}
		public static void SaveCache(string cacheKey, object obj, HttpContext context)
		{
			if(cacheKey != null && obj != null)
			{
				CacheDependency cacheDependency = new CacheDependency(obj.GetType().Assembly.Location);
				BlogCache.Cache(context).Insert(cacheKey,obj,cacheDependency);
			}
		}
	}
}

using System;
using System.Collections.Specialized;
using Subtext.Framework.Util;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Summary description for ExtendedProperties.
	/// </summary>
	public class ExtendedProperties
	{
		private NameValueCollection _nvc;

		public ExtendedProperties():this(new NameValueCollection())
		{

		}

		public ExtendedProperties(NameValueCollection nvc)
		{
			_nvc = nvc;
		}

		public ExtendedProperties(byte[] bytes):this((NameValueCollection)BinarySerializer.Deserializer(bytes))
		{
			
		}

		public byte[] Bytes
		{
			get
			{
				return BinarySerializer.Serialize(_nvc);
			}
		}

		public string this[string key]
		{
			get{ return Get(key);}
			set { Set(key,value);}
		}

		public string Get(string key)
		{
			return _nvc[key];
		}

		public void Set(string key, string text)
		{
			_nvc[key] = text;
		}
	}
}

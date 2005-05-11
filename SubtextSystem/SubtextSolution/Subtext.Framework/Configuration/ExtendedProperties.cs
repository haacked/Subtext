using System;
using System.Collections.Specialized;
using Subtext.Framework.Util;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// A class for dealing with extended pro
	/// </summary>
	public class ExtendedProperties
	{
		private NameValueCollection _nvc;

		/// <summary>
		/// Creates a new <see cref="ExtendedProperties"/> instance.
		/// </summary>
		public ExtendedProperties() : this(new NameValueCollection())
		{

		}

		/// <summary>
		/// Creates a new <see cref="ExtendedProperties"/> instance.
		/// </summary>
		/// <param name="nvc">NVC.</param>
		public ExtendedProperties(NameValueCollection nvc)
		{
			_nvc = nvc;
		}

		/// <summary>
		/// Creates a new <see cref="ExtendedProperties"/> instance.
		/// </summary>
		/// <param name="bytes">Bytes.</param>
		public ExtendedProperties(byte[] bytes) : this((NameValueCollection)BinarySerializer.Deserializer(bytes))
		{
			
		}

		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <value></value>
		public byte[] Bytes
		{
			get
			{
				return BinarySerializer.Serialize(_nvc);
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> with the specified key.
		/// </summary>
		/// <value></value>
		public string this[string key]
		{
			get{ return Get(key);}
			set { Set(key,value);}
		}

		/// <summary>
		/// Gets property value for the specified key.
		/// </summary>
		/// <param name="key">Key of the property to get.</param>
		/// <returns></returns>
		public string Get(string key)
		{
			return _nvc[key];
		}

		/// <summary>
		/// Sets property value specified by the key with the value in text.
		/// </summary>
		/// <param name="key">Key of the property to set.</param>
		/// <param name="text">The value to set the property to.</param>
		public void Set(string key, string text)
		{
			_nvc[key] = text;
		}
	}
}

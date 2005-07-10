using System;
using System.Collections.Specialized;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Custom collection of configured <see cref="ProviderInfo"/> 
	/// instances.
	/// </summary>
	public class ProviderCollection : NameObjectCollectionBase
	{
		ProviderInfo _defaultProvider = null;

		/// <summary>
		/// Gets the <see cref="ProviderInfo">ProviderInfo</see> with the specified name.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ProviderCollection" /> class.
		/// </para>
		/// </summary>
		public ProviderInfo this[string name] 
		{
			get
			{
				return (ProviderInfo)this.BaseGet(name);
			}
		}

		/// <summary>
		/// Gets the <see cref="ProviderInfo">ProviderInfo</see> at the specified index.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ProviderCollection" /> class.
		/// </para>
		/// </summary>
		public ProviderInfo this[int index] 
		{
			get
			{
				return (ProviderInfo)this.BaseGet(index);
			}
		}

		/// <summary>
		/// Gets the values.
		/// </summary>
		/// <value></value>
		public ProviderInfo[] Values
		{
			get
			{
				return (ProviderInfo[])this.BaseGetAllValues(typeof(ProviderInfo));
			}
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public void Add(ProviderInfo value) 
		{
			this.BaseAdd(value.Name, value);
		}

		/// <summary>
		/// Copies the elements of the specified <see cref="ProviderInfo" /> 
		/// array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="ProviderInfo" /> 
		/// containing the Components to add to the collection.</param>
		public void AddRange(ProviderInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}

		/// <summary>
		/// Adds the contents of another <see cref="ProviderCollection" /> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ProviderCollection" /> containing 
		/// the Components to add to the collection. </param>
		public void AddRange(ProviderCollection value) 
		{
			foreach(string key in value.BaseGetAllKeys())
			{
				this.Add(value[key]);
			}
		}
	
		/// <summary>
		/// Removes the specified <see cref="ProviderInfo"/> from 
		/// the collection.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(ProviderInfo value) 
		{
			this.BaseRemove(value.Name);
		}

		/// <summary>
		/// Removes the <see cref="ProviderInfo"/> from the collection 
		/// with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public void Remove(string name) 
		{
			this.BaseRemove(name);
		}

		/// <summary>
		/// Removes the <see cref="ProviderInfo"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveAt(int index)
		{
			this.BaseRemoveAt(index);
		}

		/// <summary>
		/// Clears all entries in this collection.
		/// </summary>
		public void Clear()
		{
			this.BaseClear();
		}

		/// <summary>
		/// Gets or sets the default provider.
		/// </summary>
		/// <value></value>
		public ProviderInfo DefaultProvider
		{
			get { return _defaultProvider; }
			set { _defaultProvider = value; }
		}
	}
}

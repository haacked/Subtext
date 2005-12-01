using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
	/// <summary>
	/// A collection of <see cref="TemplateParameter"/> instances.
	/// </summary>
	public class TemplateParameterCollection : CollectionBase
	{
		StringCollection _names = new StringCollection();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateParameterCollection"/> class.
		/// </summary>
		public TemplateParameterCollection()
		{
		}

		/// <summary>
		/// Gets the <see cref="TemplateParameter"/> at the specified index.
		/// </summary>
		/// <value></value>
		public TemplateParameter this[int index]
		{
			get	{return ((TemplateParameter)(this.List[index]));}
		}

		/// <summary>
		/// Gets the <see cref="TemplateParameter"/> with the specified name.
		/// </summary>
		/// <value></value>
		public TemplateParameter this[string name]
		{
			get
			{
				foreach(TemplateParameter parameter in this.List)
				{
					if(String.Compare(parameter.Name, name, true) == 0)
					{
						return parameter;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Creates a template parameter from a match.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public TemplateParameter Add(Match match)
		{
			if(this[match.Groups["name"].Value] != null)
				return this[match.Groups["name"].Value];

			TemplateParameter parameter = new TemplateParameter(match.Groups["name"].Value, match.Groups["type"].Value, match.Groups["default"].Value);
			this.Add(parameter);
			return parameter;
		}

		/// <summary>
		/// Adds the specified value. If it already exists, returns 
		/// the existing one, otherwise just returns the one you added.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public TemplateParameter Add(TemplateParameter value) 
		{
			if(this[value.Name] != null)
				return this[value.Name];
			List.Add(value);
			return value;
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="TemplateParameter"/>s to add to the collection. </param>
		public void AddRange(TemplateParameterCollection value) 
		{
			foreach(TemplateParameter parameter in value)
				this.Add(parameter);
		}

		/// <summary>
		/// Gets a value indicating whether the collection contains the specified 
		/// <see cref="TemplateParameter">Script</see>.
		/// </summary>
		/// <param name="value">The <see cref="TemplateParameter">Script</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(TemplateParameter value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional 
		/// <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> 
		/// that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(Script[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// Gets the index in the collection of the specified 
		/// <see cref="TemplateParameter">Script</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="TemplateParameter">Script</see> 
		/// to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(TemplateParameter value) 
		{
			return this.List.IndexOf(value);
		}
		
		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void Insert(int index, Script value)	
		{
			List.Insert(index, value);
		}
		
		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(TemplateParameter value) 
		{
			_names.Remove(value.Name);
			List.Remove(value);
		}

		/// <summary>
		/// Provides a shortcut to set a value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void SetValue(string name, string value)
		{
			if(this[name] != null)
				this[name].Value = value;
		}
	}
}

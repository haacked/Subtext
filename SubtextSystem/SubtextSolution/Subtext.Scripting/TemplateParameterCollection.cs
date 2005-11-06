using System;
using System.Collections;
using System.Collections.Specialized;

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
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public int Add(TemplateParameter value) 
		{
			if(!_names.Contains(value.Name))
				return this.List.Add(value);
			return 0;
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="TemplateParameter"/>s to add to the collection. </param>
		public void AddRange(TemplateParameterCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((TemplateParameter)value.List[i]);
			}
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
	}
}

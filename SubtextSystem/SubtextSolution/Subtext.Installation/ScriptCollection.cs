using System;
using System.Collections;

namespace Subtext.Installation
{
	/// <summary>
	/// A collection of <see cref="Script"/>s.
	/// </summary>
	public class ScriptCollection : CollectionBase
	{
		/// <summary>
		/// Gets the <see cref="Script"/> at the specified index.
		/// </summary>
		/// <value></value>
		public Script this[int index]
		{
			get	{return ((Script)(this.List[index]));}
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public int Add(Script value) 
		{
			return this.List.Add(value);
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="Script"/>s to add to the collection. </param>
		public void AddRange(ScriptCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((Script)value.List[i]);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the collection contains the specified 
		/// <see cref="Script">Script</see>.
		/// </summary>
		/// <param name="value">The <see cref="Script">Script</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(Script value) 
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
		/// <see cref="Script">Script</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="Script">Script</see> 
		/// to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(Script value) 
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
		public void Remove(Script value) 
		{
			List.Remove(value);
		}
	}
}

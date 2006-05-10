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
using System.Collections;
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
	/// <summary>
	/// A collection of <see cref="TemplateParameter"/> instances.
	/// </summary>
	public class TemplateParameterCollection : CollectionBase
	{	
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
		/// Determines whether [contains] [the specified name].
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name)
		{
			return this[name] != null;
		}

		/// <summary>
		/// Creates a template parameter from a match.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public TemplateParameter Add(Match match)
		{
			if(match == null)
				throw new ArgumentNullException("match", "Cannot create a template parameter from a null match.");
			
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
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a null template parameter.");
			
			if(Contains(value))
				return this[value.Name];
			List.Add(value);
			value.ValueChanged += new ParameterValueChangedEventHandler(value_ValueChanged);
			return value;
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="TemplateParameter"/>s to add to the collection. </param>
		public void AddRange(TemplateParameterCollection value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range of null.");
			
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
			if(value == null)
				throw new ArgumentNullException("value", "Cannot test whether or not it contains null.");
			
			return Contains(value.Name);
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
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(TemplateParameter value) 
		{
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

		private void value_ValueChanged(object sender, ParameterValueChangedEventArgs args)
		{
			OnValueChanged(args);
		}

		protected void OnValueChanged(ParameterValueChangedEventArgs args)
		{
			ParameterValueChangedEventHandler changeEvent = ValueChanged;
			if(changeEvent != null)	
			{
				changeEvent(this, args);
			}
		}

		/// <summary>
		/// Event raised when any parameter within this collection changes 
		/// its values.
		/// </summary>
		public event ParameterValueChangedEventHandler ValueChanged;
	}
}

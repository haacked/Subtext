using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
	/// <summary>
	/// A collection of <see cref="TemplateParameter"/> instances.
	/// </summary>
	public class TemplateParameterCollection : IEnumerable<TemplateParameter>, ICollection<TemplateParameter>
	{
	    List<TemplateParameter> list = new List<TemplateParameter>();

	    /// <summary>
		/// Initializes a new instance of the <see cref="TemplateParameterCollection"/> class.
		/// </summary>
		public TemplateParameterCollection()
		{
		}
	    
	    private List<TemplateParameter> List
	    {
	        get
	        {
	            return list;
	        }
	    }

		/// <summary>
		/// Gets the <see cref="TemplateParameter"/> at the specified index.
		/// </summary>
		/// <value></value>
		public TemplateParameter this[int index]
		{
            get 
            {
                return this.List[index];
            }
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
            value.ValueChanged += value_ValueChanged;
			return value;
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="TemplateParameter"/>s to add to the collection. </param>
		public void AddRange(IEnumerable<TemplateParameter> value) 
		{
		    foreach(TemplateParameter parameter in value)
		    {
                Add(parameter);
		    }
		}

	    void ICollection<TemplateParameter>.Add(TemplateParameter item)
	    {
            Add(item);
	    }

	    public void Clear()
	    {
            List.Clear();
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

	    public void CopyTo(TemplateParameter[] array, int arrayIndex)
	    {
	        List.CopyTo(array, arrayIndex);
	    }

	    public int Count
	    {
	        get { return List.Count; }
	    }

	    public bool IsReadOnly
	    {
	        get { return false; }
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
		public bool Remove(TemplateParameter value) 
		{
			return List.Remove(value);
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
			EventHandler<ParameterValueChangedEventArgs> changeEvent = ValueChanged;
			if(changeEvent != null)	
			{
				changeEvent(this, args);
			}
		}

		/// <summary>
		/// Event raised when any parameter within this collection changes 
		/// its values.
		/// </summary>
        public event EventHandler<ParameterValueChangedEventArgs> ValueChanged;

	    IEnumerator<TemplateParameter> IEnumerable<TemplateParameter>.GetEnumerator()
	    {
            return List.GetEnumerator();
	    }

	    public IEnumerator GetEnumerator()
	    {
            return List.GetEnumerator();
	    }
	}
}

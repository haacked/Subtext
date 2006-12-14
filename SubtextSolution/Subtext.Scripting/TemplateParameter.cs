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

namespace Subtext.Scripting
{
	/// <summary>
	/// Summary description for TemplateParameter.
	/// </summary>
	[Serializable]
	public class TemplateParameter
	{
		string _name;
		string _type;
		string _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		public TemplateParameter(string name, string type, string defaultValue)
		{
			_name = name;
			_type = type;
			_value = defaultValue;
		}

		/// <summary>
		/// Gets or sets the name of the parameter.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Gets the type of the data.
		/// </summary>
		/// <value>The type of the data.</value>
		public string DataType
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get { return _value; }
			set
			{
				if(value != _value)
					OnValueChanged(_value, value);
				_value = value;
			}
		}

		protected void OnValueChanged(string oldValue, string newValue)
		{
            EventHandler<ParameterValueChangedEventArgs> changeEvent = ValueChanged;
			if(changeEvent != null)	
				changeEvent(this, new ParameterValueChangedEventArgs(this.Name, oldValue, newValue));
		}

		/// <summary>
		/// Event raised when the parameter's value changes.
		/// </summary>
        public event EventHandler<ParameterValueChangedEventArgs> ValueChanged;
	}

    /// <summary>
	/// Contains information about when a template parameter value changes.
	/// </summary>
	public class ParameterValueChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterValueChangedEventArgs"/> class.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public ParameterValueChangedEventArgs(string parameterName, string oldValue, string newValue)
		{
			_oldValue = oldValue;
			_newValue = newValue;
			_parameterName = parameterName;
		}

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		/// <value>The name of the parameter.</value>
		public string ParameterName
		{
			get { return _parameterName; }
		}

		string _parameterName;

		/// <summary>
		/// Gets the old value.
		/// </summary>
		/// <value>The old value.</value>
		public string OldValue
		{
			get { return _oldValue; }
		}

		string _oldValue;

		/// <summary>
		/// Gets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public string NewValue
		{
			get { return _newValue; }
		}

		string _newValue;
	}
}

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
		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="defaultValue">The default value.</param>
		public TemplateParameter(string name, string type, string defaultValue)
		{
			Name = name;
			DataType = type;
			_value = defaultValue;
		}

		/// <summary>
		/// Gets or sets the name of the parameter.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get;
            private set;
		}

		/// <summary>
		/// Gets the type of the data.
		/// </summary>
		/// <value>The type of the data.</value>
		public string DataType
		{
			get;
            private set;
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
        string _value;

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
}

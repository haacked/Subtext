using System;

namespace Subtext.Scripting
{
	/// <summary>
	/// Summary description for TemplateParameter.
	/// </summary>
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
			set { _value = value; }
		}
	}
}

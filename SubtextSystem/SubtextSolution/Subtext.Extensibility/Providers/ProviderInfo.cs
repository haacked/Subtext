using System;
using System.Collections.Specialized;
using System.Xml;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Configuration information about a provider.
	/// </summary>
	public class ProviderInfo 
	{
		string name = string.Empty;
		string providerType = string.Empty;
		string _description = string.Empty;
		NameValueCollection providerAttributes = new NameValueCollection();

		/// <summary>
		/// Creates a new <see cref="ProviderInfo"/> instance.
		/// </summary>
		/// <param name="attributes">Attributes.</param>
		public ProviderInfo(XmlAttributeCollection attributes) 
		{
			// Set the name of the provider
			name = attributes["name"].Value;

			// Set the type of the provider
			providerType = attributes["type"].Value;

			if(attributes["description"] != null)
				_description = attributes["description"].Value;

			// Store all the attributes in the attributes bucket
			foreach (XmlAttribute attribute in attributes) 
			{
				if ((attribute.Name != "name") && (attribute.Name != "type") && (attribute.Name != "description"))
					providerAttributes.Add(attribute.Name, attribute.Value);
			}
		}

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value></value>
		public string Name 
		{
			get 
			{
				return name;
			}
		}

		/// <summary>
		/// Gets the type of the provider.
		/// </summary>
		/// <value></value>
		public string Type 
		{
			get 
			{
				return providerType;
			}
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value></value>
		public string Description
		{
			get
			{
				return _description;
			}
		}

		/// <summary>
		/// Returns a collection of the attribute name/value pairs.
		/// </summary>
		/// <value></value>
		public NameValueCollection Attributes 
		{
			get 
			{
				return providerAttributes;
			}
		}

	}
}

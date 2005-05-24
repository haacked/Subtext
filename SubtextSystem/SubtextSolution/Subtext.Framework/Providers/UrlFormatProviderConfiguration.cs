using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Configuration class for the UrlFormatProvider.
	/// </summary>
	[XmlRoot("UrlFormatProvider")]
	public class UrlFormatProviderConfiguration : BaseProvider
	{
		public UrlFormatProviderConfiguration()
		{
		}

	}
}

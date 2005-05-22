using System;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for DTOProvider.
	/// </summary>
	public sealed class DTOProvider
	{
		private DTOProvider(){}

		static DTOProvider()
		{
			DTOProviderConfiguration dtoPC = Config.Settings.BlogProviders.DTOProvider;
			idto = (IDTOProvider)dtoPC.Instance();
		}

		private static IDTOProvider idto = null;
		public static IDTOProvider Instance()
		{
			return idto;
		}
	}
}

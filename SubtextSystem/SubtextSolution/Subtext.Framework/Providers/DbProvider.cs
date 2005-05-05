using System;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// ConfigProvider loads the Singleton Instance of IDbProvider
	/// </summary>
	public class DbProvider
	{
		private DbProvider()
		{
		}

		static DbProvider()
		{
			DbProviderConfiguration dpc = Config.Settings.BlogProviders.DbProvider;
			dp = (IDbProvider)dpc.Instance();
			dp.ConnectionString = dpc.ConnectionString;
		}

		private static IDbProvider dp = null;

		public static IDbProvider Instance()
		{
			return dp;
		}


	}
}

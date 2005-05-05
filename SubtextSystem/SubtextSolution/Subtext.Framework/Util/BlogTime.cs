using System;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for BlogTime.
	/// </summary>
	public class BlogTime
	{
		private BlogTime()
		{

		}

		public static DateTime ConvertToBloggerTime(DateTime dt, int ClientTimeZone)
		{
			return dt.AddHours(FromClientToServerFactor(ClientTimeZone,Config.Settings.ServerTimeZone));
		}

		public static DateTime ConvertToServerTime(DateTime dt, int ClientTimeZone)
		{
			return dt.AddHours(FromServerToClientFactor(ClientTimeZone,Config.Settings.ServerTimeZone));
		}

		public static DateTime CurrentBloggerTime
		{
			get
			{
				return DateTime.Now.AddHours(ClientToServerTimeZoneFactor);
			}
		}

		
		public static int ClientToServerTimeZoneFactor
		{
			get
			{
				return FromClientToServerFactor(Config.CurrentBlog().TimeZone,Config.Settings.ServerTimeZone);
			}
		}

		public static int FromClientToServerFactor(int Client, int Server)
		{
			return Client - Server;
		}

		public static int FromServerToClientFactor(int Client, int Server)
		{
			return Server - Client;
		}

		public static int ServerToClientTimeZoneFactor
		{
			get
			{
				return FromServerToClientFactor(Config.CurrentBlog().TimeZone,Config.Settings.ServerTimeZone);
			}
		}
	}
}

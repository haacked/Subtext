using System;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Class used to convert server time to blogger time and back.
	/// </summary>
	public sealed class BlogTime
	{
		private BlogTime() {}

		/// <summary>
		/// Converts the specified <see cref="DateTime"/> to 
		/// the blogger's time zone.
		/// </summary>
		/// <param name="dt">Dt.</param>
		/// <param name="ClientTimeZone">Client time zone.</param>
		/// <returns></returns>
		public static DateTime ConvertToBloggerTime(DateTime dt, int ClientTimeZone)
		{
			return dt.AddHours(FromClientToServerFactor(ClientTimeZone, Config.Settings.ServerTimeZone));
		}

		/// <summary>
		/// Converts the specified <see cref="DateTime"/> ot the server's time zone.
		/// </summary>
		/// <param name="dt">Dt.</param>
		/// <param name="ClientTimeZone">Client time zone.</param>
		/// <returns></returns>
		public static DateTime ConvertToServerTime(DateTime dt, int ClientTimeZone)
		{
			return dt.AddHours(FromServerToClientFactor(ClientTimeZone, Config.Settings.ServerTimeZone));
		}

		/// <summary>
		/// Gets the current blogger time.
		/// </summary>
		/// <value></value>
		public static DateTime CurrentBloggerTime
		{
			get
			{
				return DateTime.Now.AddHours(ClientToServerTimeZoneFactor);
			}
		}

		
		/// <summary>
		/// Gets the client to server time zone factor.
		/// </summary>
		/// <value></value>
		public static int ClientToServerTimeZoneFactor
		{
			get
			{
				return FromClientToServerFactor(Config.CurrentBlog.TimeZone,Config.Settings.ServerTimeZone);
			}
		}

		/// <summary>
		/// Gets the client to server factor.
		/// </summary>
		/// <param name="Client">Client.</param>
		/// <param name="Server">Server.</param>
		/// <returns></returns>
		public static int FromClientToServerFactor(int Client, int Server)
		{
			return Client - Server;
		}

		/// <summary>
		/// Gets the server to client factor.
		/// </summary>
		/// <param name="Client">Client.</param>
		/// <param name="Server">Server.</param>
		/// <returns></returns>
		public static int FromServerToClientFactor(int Client, int Server)
		{
			return Server - Client;
		}

		/// <summary>
		/// Gets the server to client time zone factor.
		/// </summary>
		/// <value></value>
		public static int ServerToClientTimeZoneFactor
		{
			get
			{
				return FromServerToClientFactor(Config.CurrentBlog.TimeZone, Config.Settings.ServerTimeZone);
			}
		}
	}
}

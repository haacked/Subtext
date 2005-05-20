using System;
using System.Web;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Static class containing helper methods for various controls 
	/// that can't be placed within the control hierarchy.
	/// </summary>
	public sealed class ControlHelper
	{
		private ControlHelper()
		{}

		/// <summary>
		/// If the URL is is the format ~/SomePath, this 
		/// method expands the tilde using the app path.
		/// </summary>
		/// <param name="path"></param>
		public static string ExpandTildePath(string path)
		{
			string reference = path;
			if(reference.Substring(0, 2) == "~/")
			{
				string appPath = HttpContext.Current.Request.ApplicationPath;
				if(appPath.EndsWith("/"))
				{
					appPath = StringHelper.Left(appPath, appPath.Length - 1);
				}
				return appPath + reference.Substring(1);
			}
			return path;
		}
	}
}

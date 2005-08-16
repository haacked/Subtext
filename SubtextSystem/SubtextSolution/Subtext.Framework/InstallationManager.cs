using System;
using System.Web;
using System.Web.UI;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Text;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to help make determine whether an installation is required or not.
	/// </summary>
	public sealed class InstallationManager
	{
		/// <summary>
		/// Gets a value indicating whether this instance is installation action required.
		/// </summary>
		/// <param name="assemblyVersion">The version of the currently installed assembly.</param>
		/// <value>
		/// 	<c>true</c> if this instance is installation action required; otherwise, <c>false</c>.
		/// </value>
		public static bool IsInstallationActionRequired(Version assemblyVersion)
		{
			if(HttpContext.Current != null && HttpContext.Current.Application["NeedsInstallation"] != null)
			{
				return (bool)HttpContext.Current.Application["NeedsInstallation"];
			}
			
			InstallationState currentState = InstallationProvider.Instance().GetInstallationStatus(assemblyVersion);
			bool needsUpgrade = currentState  == InstallationState.NeedsInstallation 
				|| currentState  == InstallationState.NeedsUpgrade
				|| currentState  == InstallationState.NeedsRepair;

			if(HttpContext.Current != null)
			{
				HttpContext.Current.Application["NeedsInstallation"] = needsUpgrade;
			}
			return needsUpgrade;
		}

		/// <summary>
		/// Gets a value indicating whether a HostInfo record is needed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [host info record needed]; otherwise, <c>false</c>.
		/// </value>
		public static bool HostInfoRecordNeeded
		{
			get
			{
				return null == HostInfo.Instance;
			}
		}

		public static void ResetInstallationStatusCache()
		{
			if(HttpContext.Current != null && HttpContext.Current.Application["NeedsInstallation"] != null)
				HttpContext.Current.Application["NeedsInstallation"] = null;
		}

		/// <summary>
		/// Determines whether an installation action is required by 
		/// examining the specified unhandled Exception.
		/// </summary>
		/// <param name="unhandledException">Unhandled exception.</param>
		/// <param name="assemblyVersion">The version of the currently installed assembly.</param>
		/// <returns>
		/// 	<c>true</c> if an installation action is required; otherwise, <c>false</c>.
		/// </returns>
		public static bool GetIsInstallationActionRequired(Exception unhandledException, Version assemblyVersion)
		{
			if(unhandledException is BlogDoesNotExistException)
				return true;

			if(unhandledException is HostDataDoesNotExistException)
				return true;

			if(unhandledException is HostNotConfiguredException)
				return true;

			if(InstallationProvider.Instance().IsInstallationException(unhandledException))
				return true;

			InstallationState status = InstallationProvider.Instance().GetInstallationStatus(assemblyVersion);
			switch(status)
			{
				case InstallationState.NeedsInstallation:
				case InstallationState.NeedsRepair:
				case InstallationState.NeedsUpgrade:
				{
					return true;		
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the requested page is in the Install directory.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if is in install directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInInstallDirectory
		{
			get
			{
				return IsInDirectory("Install");
			}
		}

		/// <summary>
		/// Determines whether the requested page is in the Install directory.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if is in install directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInHostAdminDirectory
		{
			get
			{
				return IsInDirectory("HostAdmin");
			}
		}

		static bool IsInDirectory(string rootFolderName)
		{
			string appPath = StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
				
			string installPath = appPath;							// ex... "Subtext.Web" or ""
			if(installPath.Length > 0)
				installPath = "/" + installPath;
			String blogAppName;

			try
			{
				blogAppName = Config.CurrentBlog.Application;	// ex... "MyBlog" or ""
			}
			catch (System.Data.SqlClient.SqlException sqlE)
			{
				if(sqlE.Number == (int)Subtext.Framework.Data.SqlErrorMessages.CouldNotFindStoredProcedure &&
					sqlE.Message.IndexOf("'subtext_GetConfig'") > 0)
				{
					//OK, so we blew-up b/c we don't have this SP... probably b/c we've not yet run the install
					//wizard, yet we're trying to access the DB. Lets just redirect to the Install directory.
					blogAppName = String.Empty;
				}
				else
					throw sqlE;
			}
			if(blogAppName.Length > 0)
				installPath = installPath + "/" + blogAppName;		// ex... "/Subtext.Web/MyBlog" or "/MyBlog"

			installPath += "/" + StripSurroundingSlashes(rootFolderName) + "/";		// ex...  "Subtext.Web/MyBlog/Install/" or "/MyBlog/Install/" or "/Install/"

			return StringHelper.IndexOf(HttpContext.Current.Request.Path, installPath, false) >= 0;
		}

		static string StripSurroundingSlashes(string target)
		{
			if(target.EndsWith("/"))
				target = target.Remove(target.Length - 1, 1);
			if(target.StartsWith("/"))
				target = target.Remove(0, 1);

			return target;
		}

		/// <summary>
		/// Gets the installation status.
		/// </summary>
		/// <param name="assemblyVersion">Gets the version of the currently installed assembly.</param>
		/// <returns></returns>
		public static InstallationState GetCurrentInstallationState(Version assemblyVersion)
		{
			return InstallationProvider.Instance().GetInstallationStatus(assemblyVersion);
		}

		/// <summary>
		/// Gets the installation information control.
		/// </summary>
		/// <returns></returns>
		public static Control GetInstallationInformationControl()
		{
			return InstallationProvider.Instance().GatherInstallationInformation();	
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a string with error information.  The string is 
		/// empty if there are no errors.
		/// </summary>
		/// <param name="populatedControl">Information.</param>
		/// <returns></returns>
		public static string ValidateInstallationAnswers(Control populatedControl)
		{
			return InstallationProvider.Instance().ValidateInstallationInformation(populatedControl);
		}

		/// <summary>
		/// Sets the installation question answers.
		/// </summary>
		/// <param name="control">Control containing the user's answers.</param>
		public static void SetInstallationQuestionAnswers(Control control)
		{
			InstallationProvider.Instance().ProvideInstallationInformation(control);
		}
	}
}
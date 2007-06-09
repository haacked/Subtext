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
using System.Web;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Installation;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to help make determine whether an installation is required or not.
	/// </summary>
	public static class InstallationManager
	{
		/// <summary>
		/// Gets a value indicating whether this instance is installation action required.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is installation action required; otherwise, <c>false</c>.
		/// </value>
		public static bool IsInstallationActionRequired()
		{
			if (HttpContext.Current != null && HttpContext.Current.Application["NeedsInstallation"] != null)
			{
				return (bool)HttpContext.Current.Application["NeedsInstallation"];
			}

			InstallationState currentState = Installer.InstallationStatus;
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
				return HostInfo.Instance.DateCreated == NullValue.NullDateTime;
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
		public static bool InstallationActionRequired(Exception unhandledException, Version assemblyVersion)
		{
			if (unhandledException is BlogDoesNotExistException)
				return true;

			if (unhandledException is HostDataDoesNotExistException)
				return true;

			if (unhandledException is HostNotConfiguredException)
				return true;

			if (Installer.IsInstallationException(unhandledException))
				return true;

			switch(Installer.InstallationStatus)
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
				return HttpContext.Current.Request.Path.IndexOf("/Install/", StringComparison.InvariantCultureIgnoreCase) >= 0;
			}
		}

		/// <summary>
		/// Determines whether the requested page is the login page.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if is in install directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsOnLoginPage
		{
			get
			{
				return HttpContext.Current.Request.FilePath.EndsWith("Login.aspx", StringComparison.InvariantCultureIgnoreCase);
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
				return UrlFormats.IsInSpecialDirectory("HostAdmin");
			}
		}

		/// <summary>
		/// Determines whether the requested page is in the Install directory.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if is in install directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInUpgradeDirectory
		{
			get
			{
				return UrlFormats.IsInSpecialDirectory("HostAdmin/Upgrade");
			}
		}

		/// <summary>
		/// Determines whether the requested page is in the System Message directory.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if is in system message directory; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInSystemMessageDirectory
		{
			get
			{
				return UrlFormats.IsInSpecialDirectory("SystemMessages");
			}
		}

		/// <summary>
		/// Gets the installation status.
		/// </summary>
		/// <returns></returns>
		public static InstallationState CurrentInstallationState
		{
			get { return Installer.InstallationStatus; }
		}
	}
}
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
using System.Web.UI;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;

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
			
			InstallationState currentState = Installation.Provider.GetInstallationStatus(assemblyVersion);
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
				try
				{
					return null == HostInfo.Instance;
				}
				catch(HostNotConfiguredException)
				{
					return true;
				}
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
			if(unhandledException is BlogDoesNotExistException)
				return true;

			if(unhandledException is HostDataDoesNotExistException)
				return true;

			if(unhandledException is HostNotConfiguredException)
				return true;

			if(Installation.Provider.IsInstallationException(unhandledException))
				return true;

			InstallationState status = Installation.Provider.GetInstallationStatus(assemblyVersion);
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
		/// <param name="assemblyVersion">Gets the version of the currently installed assembly.</param>
		/// <returns></returns>
		public static InstallationState GetCurrentInstallationState(Version assemblyVersion)
		{
			return Installation.Provider.GetInstallationStatus(assemblyVersion);
		}

		/// <summary>
		/// Gets the installation information control.
		/// </summary>
		/// <returns></returns>
		public static Control GetInstallationInformationControl()
		{
			return Installation.Provider.GatherInstallationInformation();	
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
			return Installation.Provider.ValidateInstallationInformation(populatedControl);
		}

		/// <summary>
		/// Sets the installation question answers.
		/// </summary>
		/// <param name="control">Control containing the user's answers.</param>
		public static void SetInstallationQuestionAnswers(Control control)
		{
			Installation.Provider.ProvideInstallationInformation(control);
		}
	}
}
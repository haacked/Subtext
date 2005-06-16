using System;
using System.Collections.Specialized;
using System.Web;
using Subtext.Extensibility.Providers;
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
		/// Determines whether an installation action is required by 
		/// examining the specified unhandled Exception.
		/// </summary>
		/// <param name="unhandledException">Unhandled exception.</param>
		/// <returns>
		/// 	<c>true</c> if an installation action is required; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInstallationActionRequired(Exception unhandledException)
		{
			if(unhandledException is BlogDoesNotExistException)
				return true;

			if(unhandledException is HostDataDoesNotExistException)
				return true;

			if(unhandledException is HostNotConfiguredException)
				return true;

			if(InstallationProvider.Instance().IsInstallationException(unhandledException))
				return true;

			InstallationState status = InstallationProvider.Instance().GetInstallationStatus();
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
		public static bool IsInInstallDirectory()
		{
			string appPath = HttpContext.Current.Request.ApplicationPath;
			if(appPath.EndsWith("/"))
				appPath.Remove(appPath.Length - 1, 1);
			string installPath = appPath + "/Install/";

			return StringHelper.IndexOf(HttpContext.Current.Request.Path, installPath, false) >= 0;
		}

		/// <summary>
		/// Gets the installation status.
		/// </summary>
		/// <returns></returns>
		public static InstallationState GetInstallationState()
		{
			return InstallationProvider.Instance().GetInstallationStatus();
		}

		/// <summary>
		/// Gets the installation questions.
		/// </summary>
		/// <returns></returns>
		public static NameValueCollection GetInstallationQuestions()
		{
			return InstallationProvider.Instance().QueryInstallationInformation();	
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a NameValueCollection of any fields that are incorrect 
		/// with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="answers">Information.</param>
		/// <returns></returns>
		public static NameValueCollection ValidateInstallationAnswers(NameValueCollection answers)
		{
			return InstallationProvider.Instance().ValidateInstallationInformation(answers);
		}

		/// <summary>
		/// Sets the installation question answers.
		/// </summary>
		/// <param name="answers">Answers.</param>
		public static void SetInstallationQuestionAnswers(NameValueCollection answers)
		{
			InstallationProvider.Instance().ProvideInstallationInformation(answers);
		}
	}
}

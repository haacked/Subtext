using System;
using System.Collections.Specialized;
using System.Web.UI;
using Subtext.Extensibility.Providers;

namespace Subtext.Installation.Import
{
	/// <summary>
	/// Imports blog data from a .TEXT 0.95 database.
	/// </summary>
	public class DotText095ImportProvider : ImportProvider
	{
		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Imports data into this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Import()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// <p>
		/// This method is called by the import engine in order to ask the 
		/// provider what pieces of information it needs from the user in order 
		/// to proceed with the import.
		/// </p>
		/// <p>
		/// This method returns a <see cref="Control"/> used to gather 
		/// the required installation information.  This control will be returned 
		/// back to the provider after the user provides the information.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public override Control GatherImportInformation()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Provides the import information as provided by the user back 
		/// into the import provider. 
		/// The control passed in should be the same as that provided in 
		/// <see cref="GatherImportInformation"/>, but with user values 
		/// supplied within it.
		/// </summary>
		/// <param name="populatedControl">Populated control.</param>
		public override void ProvideImportInformation(Control populatedControl)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a string with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="control">control used to provide information.</param>
		/// <returns></returns>
		public override string ValidateImportInformation(Control control)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get { throw new NotImplementedException(); }
		}
	}
}

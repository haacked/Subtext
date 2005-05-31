using System;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Class used to access the various provider configuration classes 
	/// configured within web.config within the &lt;BlogProviders&gt; 
	/// section.
	/// </summary>
	[Serializable]
	public class BlogProviders
	{
		private EmailProviderConfiguration _emailProvider;
		public EmailProviderConfiguration EmailProvider
		{
			get {return this._emailProvider;}
			set {this._emailProvider = value;}
		}
	}
}

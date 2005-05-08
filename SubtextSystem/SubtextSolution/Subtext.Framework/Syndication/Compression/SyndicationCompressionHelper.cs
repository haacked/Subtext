using System;
using System.IO;
using blowery.Web.HttpModules;

namespace Subtext.Framework.Syndication.Compression
{
	public class SyndicationCompressionHelper
	{
		/*-- Constructors --*/

		#region -- Constructor() --
		private SyndicationCompressionHelper()
		{
			
		}
		#endregion

		/*-- Static Methods --*/

		#region -- GetFilterForScheme(schemes, Stream) Method --
		public static SyndicationCompressionFilter GetFilterForScheme(string schemes, Stream contextFilter) 
		{
			SyndicationCompressionSettings settings;
			SyndicationCompressionFilter filter = null;
			bool foundDeflate = false;
			bool foundGZip = false;

			schemes = schemes.ToLower();
			settings = SyndicationCompressionSettings.GetSettings();

			if(schemes.IndexOf("deflate") >= 0)
			{
				foundDeflate = true;
				
			}
			if(schemes.IndexOf("gzip") >= 0)
			{
				foundGZip = true;
			}

			if(settings.CompressionType == CompressionTypes.Deflate && foundDeflate)
			{
				filter = new SyndicationCompressionFilter(new DeflateFilter(contextFilter, settings.CompressionLevel), "deflate");
			}
			else if(settings.CompressionType == CompressionTypes.GZip && foundGZip)
			{
				filter = new SyndicationCompressionFilter(new GZipFilter(contextFilter), "gzip");
			}
			else if(foundDeflate) //-- If Use Accepts Other Than Configured
			{
				filter = new SyndicationCompressionFilter(new DeflateFilter(contextFilter, settings.CompressionLevel), "deflate");
			}
			else if(foundGZip) //-- If Use Accepts Other Than Configured
			{
				filter = new SyndicationCompressionFilter(new GZipFilter(contextFilter), "gzip");
			}
      
			return filter;
		}
		#endregion
	}
}

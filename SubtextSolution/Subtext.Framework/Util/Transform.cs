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
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Class used to provide various transforms such as the 
	/// Emoticon transforms.
	/// </summary>
	public static class Transform
	{
		private static ILog Log = new Log();

		/// <summary>
		/// Transforms emoticons into image references based on the 
		/// settings within the emoticons.txt file in the webroot.
		/// </summary>
		/// <param name="formattedPost">The formatted post.</param>
		/// <returns></returns>
		public static string EmoticonTransforms(string formattedPost) 
		{
			try
			{
				ArrayList emoticonTxTable = LoadTransformFile("emoticons.txt");
				return PerformUserTransforms(formattedPost, emoticonTxTable);
			}
			catch(System.IO.IOException e)
			{
				Log.Warn("Missing an emoticons.txt file in the webroot. Please download it from <a href=\"http://haacked.com/images/emoticons.zip\" title=\"Emoticons file\">here</a>.", e);
				return formattedPost;
			}
		}

		static string PerformUserTransforms(string stringToTransform, ArrayList userDefinedTransforms) 
		{
			if(userDefinedTransforms == null)
				return stringToTransform;

			int iLoop = 0;	
			string host = Config.CurrentBlog.RootUrl;
			while (iLoop < userDefinedTransforms.Count) 
			{		
				// Special work for anchors
				stringToTransform = Regex.Replace(stringToTransform, userDefinedTransforms[iLoop].ToString(), string.Format(userDefinedTransforms[iLoop+1].ToString(),host), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

				iLoop += 2;
			}

			return stringToTransform;
		}

		private static ArrayList LoadTransformFile(string filename) 
		{
            if (filename == null)
                throw new ArgumentNullException("filename", "The transform filename is null.");
			string cacheKey = "transformTable-" + filename;
			ArrayList tranforms;
			string filenameOfTransformFile;

			HttpContext context = HttpContext.Current;

			// read the transformation hashtable from the cache
			//
			tranforms = (ArrayList) context.Cache[cacheKey];

			if (tranforms == null) 
			{
				tranforms = new ArrayList();

				// Grab the transform file
				if(context == null || context.Request == null)
					return null;

				try
				{
					filenameOfTransformFile = context.Request.MapPath("~/" + filename);
				}
				catch(System.ArgumentNullException)
				{
					//This exception can be thrown from the bowels of MapPath...
					return null;
				}

				if (filenameOfTransformFile.Length > 0) 
				{

					StreamReader sr = File.OpenText( filenameOfTransformFile );

					// Read through each set of lines in the text file
					//
					string line = sr.ReadLine(); 
					string replaceLine = "";

					while (line != null) 
					{

						line = Regex.Escape(line);
						replaceLine = sr.ReadLine();

						// make sure replaceLine != null
						//
						if (replaceLine == null) 
							break;
					
						line = line.Replace("<CONTENTS>", "((.|\n)*?)");
						line = line.Replace("<WORDBOUNDARY>", "\\b");
						line = line.Replace("<", "&lt;");
						line = line.Replace(">", "&gt;");
						line = line.Replace("\"", "&quot;");

						replaceLine = replaceLine.Replace("<CONTENTS>", "$1");					
					
						tranforms.Add(line);
						tranforms.Add(replaceLine);

						line = sr.ReadLine();

					}

					// close the streamreader
					//
					sr.Close();		

					// slap the ArrayList into the cache and set its dependency to the transform file.
					//
					HttpContext.Current.Cache.Insert(cacheKey, tranforms, new CacheDependency(filenameOfTransformFile));
				}
			}
  
			return (ArrayList) HttpContext.Current.Cache[cacheKey];
		}
	}
}

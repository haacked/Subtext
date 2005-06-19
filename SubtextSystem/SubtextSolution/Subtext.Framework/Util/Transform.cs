using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Borrowed From Rob's Forum Code :) <- This is a joke :) <- Again...I will stop now
	/// </summary>
	public class Transform
	{
		public static string EmoticonTransforms (string formattedPost) 
		{
			// Load the emoticon transform table
			//
			ArrayList emoticonTxTable = LoadTransformFile("emoticons.txt");

			// Do the transforms
			//
			return PerformUserTransforms(formattedPost, emoticonTxTable);
		}

		static string PerformUserTransforms(string stringToTransform, ArrayList userDefinedTransforms) 
		{
			int iLoop = 0;	
			string host = Config.CurrentBlog.BlogRootUrl;
			while (iLoop < userDefinedTransforms.Count) 
			{		
				// Special work for anchors
				stringToTransform = Regex.Replace(stringToTransform, userDefinedTransforms[iLoop].ToString(), string.Format(userDefinedTransforms[iLoop+1].ToString(),host), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

				iLoop += 2;
			}

			return stringToTransform;
		}

		private static ArrayList LoadTransformFile (string filename) 
		{
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
				//
				filenameOfTransformFile = context.Request.MapPath("~/" + filename);

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

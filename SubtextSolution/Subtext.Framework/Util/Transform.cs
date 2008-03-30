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
using System.Collections.Generic;
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
				return EmoticonsTransforms(formattedPost, GetTransformFilePath("emoticons.txt"));
			}
			catch(IOException ioe)
			{
				Log.Warn("Problem reading the emoticons.txt file", ioe);
				return formattedPost;
			}
			catch(ArgumentNullException e)
			{
				Log.Warn("Problem reading the emoticons.txt file", e);
				return formattedPost;
			}
		}

		public static string EmoticonsTransforms(string formattedPost, string emoticonsFilePath)
		{
			if (formattedPost == null)
				throw new ArgumentNullException("formattedPost", "Cannot transform a null post");

			if (emoticonsFilePath == null)
				throw new ArgumentNullException("emoticonsFilePath", "Must specify a non-null emoticons file path.");

			if (!File.Exists(emoticonsFilePath))
			{
				Log.Warn("Missing an emoticons.txt file in the webroot. Please download it from <a href=\"http://haacked.com/images/emoticons.zip\" title=\"Emoticons file\">here</a>.");
				return formattedPost;
			}

			List<string> emoticonTxTable = LoadTransformFile(emoticonsFilePath);
			return PerformUserTransforms(formattedPost, emoticonTxTable);
		}

		static string PerformUserTransforms(string stringToTransform, List<string> userDefinedTransforms) 
		{
			if(userDefinedTransforms == null)
				return stringToTransform;

			int iLoop = 0;	
			string host = Config.CurrentBlog.RootUrl.ToString();
			while (iLoop < userDefinedTransforms.Count) 
			{		
				// Special work for anchors
				stringToTransform = Regex.Replace(stringToTransform, userDefinedTransforms[iLoop].ToString(), string.Format(userDefinedTransforms[iLoop+1], host), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

				iLoop += 2;
			}

			return stringToTransform;
		}

		private static string GetTransformFilePath(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename", "The transform filename is null.");

			if (filename.Length == 0)
				throw new ArgumentException("filename", "The transform filename is empty.");

			if (HttpContext.Current == null)
				throw new InvalidOperationException("The HttpContext is null. Cannot get the path to the emoticons transform file.");

			return HttpContext.Current.Request.MapPath("~/" + filename);
		}

		public static List<string> LoadTransformFile(string filePath) 
		{
            if (filePath == null)
                throw new ArgumentNullException("filePath", "The transform filePath is null.");
			
			string cacheKey = "transformTable-" + Path.GetFileName(filePath);

			HttpContext context = HttpContext.Current;
			if(context == null)
				return null;

			// read the transformation hashtable from the cache
			//
			List<string> tranforms = (List<string>)context.Cache[cacheKey];

			if (tranforms == null) 
			{
				tranforms = new List<string>();

				// Grab the transform file
				if(context.Request == null)
					return null;

				if (filePath.Length > 0) 
				{

					using (StreamReader sr = File.OpenText(filePath))
					{
						// Read through each set of lines in the text file
						//
						string line = sr.ReadLine();

						while (line != null)
						{
							line = Regex.Escape(line);
							string replaceLine = sr.ReadLine();

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
					}

					// slap the ArrayList into the cache and set its dependency to the transform file.
					HttpContext.Current.Cache.Insert(cacheKey, tranforms, new CacheDependency(filePath));
				}
			}
  
			return (List<string>)HttpContext.Current.Cache[cacheKey];
		}
	}
}

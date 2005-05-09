#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
	public sealed class Images
	{
		//Static class.
		private Images() {}

		public static string LocalFilePath(HttpContext context)
		{
			return Config.CurrentBlog(context).ImageDirectory;
		}

		public static string LocalGalleryFilePath(HttpContext context, int categoryid)
		{
			return string.Format("{0}\\{1}\\",Config.CurrentBlog(context).ImageDirectory,categoryid);
		}

		public static string HttpGalleryFilePath(HttpContext context, int categoryid)
		{
			return string.Format("{0}{1}/",Config.CurrentBlog(context).ImagePath,categoryid);
		}

		public static string HttpFilePath(HttpContext context)
		{
			return Config.CurrentBlog(context).ImagePath;
		}

		public static byte[] GetFileStream(HttpPostedFile objFile)
		{
			if(objFile != null)
			{
				int len = objFile.ContentLength;
				byte[] input = new byte[len];
				Stream file = objFile.InputStream;
				file.Read(input,0,len);
				return input;
			}
			return null;
		}
		
		public static bool ValidateFile(string filepath)
		{
			if (File.Exists(filepath))
			{
				return false;
			}

			return Regex.IsMatch(filepath,
				"(?:[^\\/\\*\\?\\\"\\<\\>\\|\\n\\r\\t]+)\\.(?:jpg|jpeg|gif|png|bmp)", 
				RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
				);
		}

		public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
		{
			decimal MAX_WIDTH = (decimal)maxWidth;
			decimal MAX_HEIGHT = (decimal)maxHeight;
			decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

			int newWidth, newHeight;

			decimal originalWidth = (decimal)width;
			decimal originalHeight = (decimal)height;
			
			if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT) 
			{
				decimal factor;
				// determine the largest factor 
				if (originalWidth / originalHeight > ASPECT_RATIO) 
				{
					factor = originalWidth / MAX_WIDTH;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				} 
				else 
				{
					factor = originalHeight / MAX_HEIGHT;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				}	  
			} 
			else 
			{
				newWidth = width;
				newHeight = height;
			}

			return new Size(newWidth,newHeight);
			
		}

		public static bool SaveImage(byte[] Buffer, string FileName)
		{
			try
			{
				if (ValidateFile(FileName))
				{
					CheckDirectory(FileName);
					FileStream fs = new FileStream(FileName,FileMode.Create);
					fs.Write(Buffer,0,Buffer.Length);
					fs.Close();	
					return true;
				}
				return false;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public static void MakeAlbumImages(ref Subtext.Framework.Components.Image _image)
		{

			System.Drawing.Image original = System.Drawing.Image.FromFile(_image.OriginalFilePath);

			Size _newSize = ResizeImage(original.Width,original.Height,640,480);
			_image.Height = _newSize.Height;
			_image.Width = _newSize.Width;
			System.Drawing.Image displayImage = new Bitmap(original,_newSize);
			System.Drawing.Image tbimage = new Bitmap(original,ResizeImage(original.Width,original.Height,120,120));
			original.Dispose();
			
			displayImage.Save(_image.ResizedFilePath, GetFormat(_image.File));   
			displayImage.Dispose();
			tbimage.Save(_image.ThumbNailFilePath,ImageFormat.Jpeg);
			tbimage.Dispose();
		}

		public static ImageFormat GetFormat(string name)
		{
			string ext = name.Substring(name.LastIndexOf(".") + 1);
			switch(ext.ToLower())
			{
				case "jpg":
				case "jpeg":
					return ImageFormat.Jpeg;
				case "bmp":
					return ImageFormat.Bmp;
				case "png":
					return ImageFormat.Png;
				case "gif":
					return ImageFormat.Gif;
				default:
					return ImageFormat.Jpeg;
			}
		}

		public static string GetFileName(string filepath)
		{
			if(filepath.IndexOf("\\") == -1)
			{
				return StripUrlCharsFromFileName(filepath);
			}
			else
			{
				int lastindex = filepath.LastIndexOf("\\");
				return StripUrlCharsFromFileName(filepath.Substring(lastindex+1));
			}
		}

      private static string StripUrlCharsFromFileName(string filename)
      {
         const string replacement = "_";

         filename = filename.Replace("#", replacement);
         filename = filename.Replace("&", replacement);
         filename = filename.Replace("%", replacement);

         return filename;
      }

		public static void CheckDirectory(string filepath)
		{
			string dir = filepath.Substring(0,filepath.LastIndexOf("\\"));
			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		#region Data Stuff

		public static ImageCollection GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetImagesByCategoryID(catID,ActiveOnly);
		}

		public static Subtext.Framework.Components.Image GetSingleImage(int imageID, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetSingleImage(imageID,ActiveOnly);
		}

		public static int InsertImage(Subtext.Framework.Components.Image _image,byte[] Buffer)
		{
			if(SaveImage(Buffer,_image.OriginalFilePath))
			{
				MakeAlbumImages(ref _image);
				return DTOProvider.Instance().InsertImage(_image);
			}
			return -1;
		}

		public static bool UpdateImage(Subtext.Framework.Components.Image _image)
		{
			return DTOProvider.Instance().UpdateImage(_image);
		}

		// added
		public static void Update(Subtext.Framework.Components.Image _image, byte[] Buffer)
		{
			if(SaveImage(Buffer, _image.OriginalFilePath))
			{
				MakeAlbumImages(ref _image);
				UpdateImage(_image);
			}
		}

		public static void DeleteImage(Subtext.Framework.Components.Image _image)
		{
			DTOProvider.Instance().DeleteImage(_image.ImageID);
		}

		public static void TryDeleteFile(string file)
		{
			if(File.Exists(file))
			{
				File.Delete(file);
			}
		}
		#endregion
	}
}


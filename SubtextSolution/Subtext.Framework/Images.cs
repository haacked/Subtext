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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Image = Subtext.Framework.Components.Image;

namespace Subtext.Framework
{
	public static class Images
	{
		public static string LocalFilePath(HttpContext context)
		{
			return Config.CurrentBlog.ImageDirectory;
		}

		public static string LocalGalleryFilePath(HttpContext context, int categoryid)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}\\",Config.CurrentBlog.ImageDirectory,categoryid);
		}

		public static string HttpGalleryFilePath(HttpContext context, int categoryid)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}/",Config.CurrentBlog.ImagePath,categoryid);
		}

		public static string HttpFilePath(HttpContext context)
		{
			return Config.CurrentBlog.ImagePath;
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
			decimal MAX_WIDTH = maxWidth;
			decimal MAX_HEIGHT = maxHeight;
			decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

			int newWidth, newHeight;

			decimal originalWidth = width;
			decimal originalHeight = height;
			
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

		/// <summary>
		/// Saves two images. A normal image for the web site and then a thumbnail.
		/// </summary>
		/// <param name="image">Original image to process.</param>
		public static void MakeAlbumImages(Subtext.Framework.Components.Image image)
		{
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(image.OriginalFilePath);

            // Need to load the original image to manipulate. But indexed GIFs can cause issues.
            if ((originalImage.PixelFormat & PixelFormat.Indexed) != 0)
            {
                // Draw the index image to a new bitmap.  It will then be unindexed.
                System.Drawing.Image unindexedImage = new Bitmap(originalImage.Width, originalImage.Height);
                Graphics g = Graphics.FromImage(unindexedImage);
                g.DrawImageUnscaled(originalImage, 0, 0);

                originalImage.Dispose();
                originalImage = unindexedImage;
            }

			// Dispose the original graphic (be kind; clean up)
            using (originalImage)
			{
				/// TODO: make both sizes configurations. 
				// Calculate the new sizes we want (properly scaled) 
				Size displaySize = ResizeImage(originalImage.Width, originalImage.Height, 640,480);
				Size thumbSize = ResizeImage(originalImage.Width, originalImage.Height, 120, 120);

                // Tell the object what its new display size will be
                image.Height = displaySize.Height;
                image.Width = displaySize.Width;

                // Create a mid-size display image by drawing the original image into a smaller area.
                using (System.Drawing.Image displayImage = new Bitmap(displaySize.Width, displaySize.Height, originalImage.PixelFormat))
                {
                    using (Graphics displayGraphic = Graphics.FromImage(displayImage))
                    {
                        displayGraphic.CompositingQuality = CompositingQuality.HighQuality;
                        displayGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        displayGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Rectangle displayRectangle = new Rectangle(0, 0, displaySize.Width, displaySize.Height);
                        displayGraphic.DrawImage(originalImage, displayRectangle);
                        // Save our file
                        displayImage.Save(image.ResizedFilePath, ImageFormat.Jpeg);
                    }
                }

				// Create a small thumbnail
                using (System.Drawing.Image thumbImage = new Bitmap(thumbSize.Width, thumbSize.Height, originalImage.PixelFormat))
                {
                    using (Graphics thumbGraphic = Graphics.FromImage(thumbImage))
                    {
                        thumbGraphic.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Rectangle thumbRectangle = new Rectangle(0, 0, thumbSize.Width, thumbSize.Height);
                        thumbGraphic.DrawImage(originalImage, thumbRectangle);
                        // Save our file
                        thumbImage.Save(image.ThumbNailFilePath, ImageFormat.Jpeg);
                    }
                }
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

		public static ImageCollection GetImagesByCategoryID(int catID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetImagesByCategoryID(catID, activeOnly);
		}

		public static Image GetSingleImage(int imageID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetImage(imageID, activeOnly);
		}

		public static int InsertImage(Image image, byte[] Buffer)
		{
			if(SaveImage(Buffer,image.OriginalFilePath))
			{
				MakeAlbumImages(image);
				return ObjectProvider.Instance().InsertImage(image);
			}
			return -1;
		}

		public static bool UpdateImage(Image _image)
		{
			return ObjectProvider.Instance().UpdateImage(_image);
		}

		// added
		public static void Update(Image image, byte[] Buffer)
		{
			if(SaveImage(Buffer, image.OriginalFilePath))
			{
				MakeAlbumImages(image);
				UpdateImage(image);
			}
		}

		public static void DeleteImage(Image _image)
		{
			ObjectProvider.Instance().DeleteImage(_image.ImageID);
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

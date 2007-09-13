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
using Subtext.Framework.Properties;

namespace Subtext.Framework
{
	public static class Images
	{
		/// <summary>
		/// Returns the physical gallery path for the specified category.
		/// </summary>
		/// <param name="categoryid">The categoryid.</param>
		/// <returns></returns>
		public static string LocalGalleryFilePath(int categoryid)
		{
			return Path.Combine(Config.CurrentBlog.ImageDirectory, categoryid.ToString(CultureInfo.InvariantCulture)) + Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Returns the url path to the gallery for the specified category.
		/// </summary>
		/// <param name="categoryid">The categoryid.</param>
		/// <returns></returns>
		public static string GalleryVirtualUrl(int categoryid)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}/", Config.CurrentBlog.ImagePath, categoryid);
		}

		/// <summary>
		/// gets the bytes for the posted file
		/// </summary>
		/// <param name="objFile">The obj file.</param>
		/// <returns></returns>
		public static byte[] GetFileStream(HttpPostedFile objFile)
		{
			if (objFile != null)
			{
				int len = objFile.ContentLength;
				byte[] input = new byte[len];
				Stream file = objFile.InputStream;
				file.Read(input, 0, len);
				return input;
			}
			return null;
		}

		/// <summary>
		/// Validates that the file is allowed.
		/// </summary>
		/// <param name="filepath">The filepath.</param>
		/// <returns></returns>
		public static bool ValidateFile(string filepath)
		{
			return Regex.IsMatch(filepath,
				"(?:[^\\/\\*\\?\\\"\\<\\>\\|\\n\\r\\t]+)\\.(?:jpg|jpeg|gif|png|bmp)",
				RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
				);
		}

		public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
		{
			decimal aspectRatio = maxWidth / maxHeight;

			int newWidth;
			int newHeight;

			decimal originalWidth = width;
			decimal originalHeight = height;

			if (originalWidth > maxWidth || originalHeight > maxHeight)
			{
				decimal factor;
				// determine the largest factor 
				if (originalWidth / originalHeight > aspectRatio)
				{
					factor = originalWidth / maxWidth;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				}
				else
				{
					factor = originalHeight / maxWidth;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				}
			}
			else
			{
				newWidth = width;
				newHeight = height;
			}

			return new Size(newWidth, newHeight);

		}

		/// <summary>
		/// Saves the image.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static bool SaveImage(byte[] buffer, string fileName)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer", Resources.ArgumentNull_Array);

			if (fileName == null)
				throw new ArgumentNullException("fileName", Resources.ArgumentNull_Generic);

			if (fileName.Length == 0)
			{
				throw new ArgumentException(Resources.Argument_StringZeroLength, "fileName");
			}

			if (ValidateFile(fileName))
			{
				EnsureDirectory(Path.GetDirectoryName(fileName));
				using(FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
				{
					fs.Write(buffer, 0, buffer.Length);
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Saves two images. A normal image for the web site and then a thumbnail.
		/// </summary>
		/// <param name="image">Original image to process.</param>
		public static void MakeAlbumImages(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);

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
				Size displaySize = ResizeImage(originalImage.Width, originalImage.Height, 640, 480);
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

		public static void EnsureDirectory(string directoryPath)
		{
			if (directoryPath == null)
                throw new ArgumentNullException("directoryPath", Resources.ArgumentNull_String);

			if (directoryPath.Length == 0)
                throw new ArgumentException(Resources.Argument_StringZeroLength, "directoryPath");

			string dir = Path.GetFullPath(directoryPath);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		#region Data Stuff

		public static ImageCollection GetImagesByCategoryID(int catID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetImagesByCategoryID(catID, activeOnly);
		}

		public static Image GetSingleImage(int imageId, bool activeOnly)
		{
			return ObjectProvider.Instance().GetImage(imageId, activeOnly);
		}

		/// <summary>
		/// Inserts the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="Buffer">The buffer.</param>
		/// <returns></returns>
		public static int InsertImage(Image image, byte[] Buffer)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);

			if (!File.Exists(image.OriginalFilePath) && SaveImage(Buffer, image.OriginalFilePath))
			{
				MakeAlbumImages(image);
				return ObjectProvider.Instance().InsertImage(image);
			}
			return NullValue.NullInt32;
		}

		/// <summary>
		/// Updates the image.
		/// </summary>
		/// <param name="image">The image.</param>
		public static void UpdateImage(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);
			ObjectProvider.Instance().UpdateImage(image);
		}

		// added
		public static void Update(Image image, byte[] buffer)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);
			
			if (buffer == null)
				throw new ArgumentNullException("buffer", Resources.ArgumentNull_Generic);


			if (SaveImage(buffer, image.OriginalFilePath))
			{
				MakeAlbumImages(image);
				UpdateImage(image);
			}
		}

		public static void DeleteImage(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);

			ObjectProvider.Instance().DeleteImage(image.ImageID);
		}

		#endregion
	}
}

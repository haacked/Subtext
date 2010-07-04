#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Util;
using Image=Subtext.Framework.Components.Image;
using Subtext.Framework.Configuration;

namespace Subtext.Framework
{
    public static class Images
    {
        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static bool SaveImage(byte[] buffer, string fileName)
        {
            if(buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if(string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if(FileHelper.IsValidImageFilePath(fileName))
            {
                FileHelper.EnsureDirectory(Path.GetDirectoryName(fileName));
                FileHelper.WriteBytesToFile(fileName, buffer);
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
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }

            // Indexed GIFs can cause issues.
            using(System.Drawing.Image originalImage = GraphicsHelper.FromFilePathAsUnindexedImage(image.OriginalFilePath))
            {
                var originalSize = new Size(originalImage.Width, originalImage.Height);

                /// TODO: make new sizes configurations. 
                // New Display Size
                Size displaySize = originalSize.ScaleToFit(Config.Settings.GalleryImageMaxWidth, Config.Settings.GalleryImageMaxHeight);
                image.Height = displaySize.Height;
                image.Width = displaySize.Width;
                using(System.Drawing.Image displayImage = originalImage.GetResizedImage(displaySize))
                {
                    displayImage.Save(image.ResizedFilePath, ImageFormat.Jpeg);
                }

                // smaller thumbnail
                Size thumbSize = originalSize.ScaleToFit(Config.Settings.GalleryImageThumbnailWidth, Config.Settings.GalleryImageThumbnailHeight);
                using(System.Drawing.Image thumbnailImage = originalImage.GetResizedImage(thumbSize))
                {
                    thumbnailImage.Save(image.ThumbNailFilePath, ImageFormat.Jpeg);
                }
            }
        }

        public static ImageCollection GetImagesByCategoryId(int categoryId, bool activeOnly)
        {
            return ObjectProvider.Instance().GetImagesByCategoryId(categoryId, activeOnly);
        }

        /// <summary>
        /// Inserts the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static int InsertImage(Image image, byte[] buffer)
        {
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }

            if(!File.Exists(image.OriginalFilePath) && SaveImage(buffer, image.OriginalFilePath))
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
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }
            ObjectProvider.Instance().UpdateImage(image);
        }

        // added
        public static void Update(Image image, byte[] buffer)
        {
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }

            if(buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if(SaveImage(buffer, image.OriginalFilePath))
            {
                MakeAlbumImages(image);
                UpdateImage(image);
            }
        }

        public static void DeleteImage(Image image)
        {
            if(image == null)
            {
                throw new ArgumentNullException("image");
            }

            ObjectProvider.Instance().DeleteImage(image.ImageID);
        }
    }
}
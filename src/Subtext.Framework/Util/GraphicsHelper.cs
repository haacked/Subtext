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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Subtext.Framework.Util
{
    public static class GraphicsHelper
    {
        /// <summary>
        /// Returns an Image resized to the specified size.
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static Image GetResizedImage(this Image originalImage, Size newSize)
        {
            Image resizedImage = new Bitmap(newSize.Width, newSize.Height, originalImage.PixelFormat);
            using (Graphics graphic = Graphics.FromImage(resizedImage))
            {
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var thumbRectangle = new Rectangle(0, 0, newSize.Width, newSize.Height);
                graphic.DrawImage(originalImage, thumbRectangle);
            }
            return resizedImage;
        }

        public static Image FromFilePathAsUnindexedImage(string filePath)
        {
            Image originalImage = Image.FromFile(filePath);

            if ((originalImage.PixelFormat & PixelFormat.Indexed) == 0)
            {
                return originalImage;
            }

            // Draw the index image to a new bitmap.  It will then be unindexed.
            Image unindexedImage = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics graphics = Graphics.FromImage(unindexedImage))
            {
                graphics.DrawImageUnscaled(originalImage, 0, 0);
            }
            originalImage.Dispose();
            return unindexedImage;
        }
    }
}
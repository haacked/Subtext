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

namespace Subtext.Framework.Util
{
    public static class MathHelper
    {
        public static decimal GetAspectRatio(this Size size)
        {
            return GetAspectRatio(size.Width, size.Height);
        }

        public static decimal GetAspectRatio(int width, int height)
        {
            return (decimal)width / (decimal)height;
        }

        /// <summary>
        /// Returns a new size which would scale the original size to fit 
        /// within the specified maximum size while retaining its aspect 
        /// ratio.
        /// </summary>
        public static Size ScaleToFit(this Size original, int maxWidth, int maxHeight)
        {
            decimal aspectRatio = GetAspectRatio(maxWidth, maxHeight);

            decimal originalWidth = original.Width;
            decimal originalHeight = original.Height;

            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
            {
                return original;
            }

            // determine the largest factor 
            decimal factor;
            if (original.GetAspectRatio() > aspectRatio)
            {
                factor = originalWidth / (decimal)maxWidth;
            }
            else
            {
                factor = originalHeight / (decimal)maxHeight;
            }

            int newWidth = Convert.ToInt32(originalWidth / factor);
            int newHeight = Convert.ToInt32(originalHeight / factor);

            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// Returns a new size which would scale the original size to fit 
        /// within the specified maximum size while retaining its aspect 
        /// ratio.
        /// </summary>
        public static Size ScaleToFit(this Size original, Size maxSize)
        {
            return original.ScaleToFit(maxSize.Width, maxSize.Height);
        }
    }
}
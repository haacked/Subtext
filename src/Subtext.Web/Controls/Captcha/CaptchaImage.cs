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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using Subtext.Web.Properties;

namespace Subtext.Web.Controls.Captcha
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    public class CaptchaImage : IDisposable
    {
        #region FontWarpFactor enum

        public enum FontWarpFactor
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        #endregion

        const int MinHeight = 30;
        const int MinWidth = 60;
        private readonly Random random;
        private string fontFamilyName;
        private int height;
        private Bitmap image;
        private int width;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaImage"/> class.
        /// </summary>
        public CaptchaImage()
        {
            random = new Random();
            FontWarp = FontWarpFactor.Low;
            Width = 180;
            Height = 50;
        }

        #region Disposable Pattern

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Disposes the Captcha image.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Image.Dispose();
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="CaptchaImage"/> is reclaimed by garbage collection.
        /// </summary>
        ~CaptchaImage()
        {
            Dispose(false);
        }

        #endregion

        public string Font
        {
            get { return fontFamilyName; }
            set
            {
                try
                {
                    using (var font1 = new Font(value, 12f))
                    {
                        fontFamilyName = value;
                        font1.Dispose();
                    }
                }
                catch (Exception)
                {
                    fontFamilyName = FontFamily.GenericSerif.Name;
                }
            }
        }

        /// <summary>
        /// Amount of random waping to apply to rendered text.
        /// </summary>
        /// <value>The font warp.</value>
        public FontWarpFactor FontWarp { get; set; }

        /// <summary>
        /// Height of the Captcha image in pixels.
        /// </summary>
        public int Height
        {
            get { return height; }
            set
            {
                if (value <= MinHeight)
                {
                    throw new ArgumentOutOfRangeException("height", value,
                                                          String.Format(CultureInfo.InvariantCulture,
                                                                        Resources.ArgumentOutOfRange_Height, MinHeight));
                }
                height = value;
            }
        }

        /// <summary>
        /// Gets the captcha image to display based on the current property 
        /// values.  Will render the image if it hasn't been rendered yet.
        /// </summary>
        /// <value>The image.</value>
        public Bitmap Image
        {
            get
            {
                if (image == null)
                {
                    image = GenerateImagePrivate();
                }
                return image;
            }
        }

        /// <summary>
        /// Width of the Captcha image in pixels.
        /// </summary>
        public int Width
        {
            get { return width; }
            set
            {
                if (value <= MinWidth)
                {
                    throw new ArgumentOutOfRangeException("width", value,
                                                          String.Format(Resources.ArgumentOutOfRange_Width, MinWidth));
                }
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to render (warped of course).
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Generates a new Captcha image.
        /// </summary>
        public void GenerateImage()
        {
            image = GenerateImagePrivate();
        }

        private Bitmap GenerateImagePrivate()
        {
            Font font;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rectF = new RectangleF(0f, 0f, width, height);
                var rect = new Rectangle(0, 0, width, height);
                var smallConfettiBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                graphics.FillRectangle(smallConfettiBrush, rect);
                float previousWidth = 0f;
                float size = Convert.ToInt32((height * 0.8));
                while (true)
                {
                    font = new Font(fontFamilyName, size, FontStyle.Bold);
                    SizeF textSize = graphics.MeasureString(Text, font);
                    if (textSize.Width <= width)
                    {
                        break;
                    }
                    if (previousWidth > 0f)
                    {
                        int estimatedSize =
                            Convert.ToInt32(((textSize.Width - width) / (previousWidth - textSize.Width)));
                        if (estimatedSize > 0)
                        {
                            size -= estimatedSize;
                        }
                        else
                        {
                            size -= 1f;
                        }
                    }
                    else
                    {
                        size -= 1f;
                    }
                    previousWidth = textSize.Width;
                }
                size += 4f;

                font = new Font(fontFamilyName, size, FontStyle.Bold);
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                var textPath = new GraphicsPath();
                textPath.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
                if (FontWarp != FontWarpFactor.None)
                {
                    int warpDivisor = 0;
                    switch (FontWarp)
                    {
                        case FontWarpFactor.Low:
                            warpDivisor = 6;
                            break;

                        case FontWarpFactor.Medium:
                            warpDivisor = 5;
                            break;

                        case FontWarpFactor.High:
                            warpDivisor = 4;
                            break;

                        case FontWarpFactor.Extreme:
                            warpDivisor = 3;
                            break;
                    }
                    int heightRange = Convert.ToInt32(((rect.Height) / ((double)warpDivisor)));
                    int widthRange = Convert.ToInt32(((rect.Width) / ((double)warpDivisor)));
                    PointF point1 = RandomPoint(0, widthRange, 0, heightRange);
                    PointF point2 = RandomPoint(rect.Width - (widthRange - Convert.ToInt32(point1.X)), rect.Width, 0,
                                                heightRange);
                    PointF point3 = RandomPoint(0, widthRange, rect.Height - (heightRange - Convert.ToInt32(point1.Y)),
                                                rect.Height);
                    PointF point4 = RandomPoint(rect.Width - (widthRange - Convert.ToInt32(point3.X)), rect.Width,
                                                rect.Height - (heightRange - Convert.ToInt32(point2.Y)), rect.Height);
                    var points = new[] { point1, point2, point3, point4 };
                    var matrix = new Matrix();
                    matrix.Translate(0f, 0f);
                    textPath.Warp(points, rectF, matrix, WarpMode.Perspective, 0f);
                }
                var largeConfettiBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
                graphics.FillPath(largeConfettiBrush, textPath);

                int maxDimension = Math.Max(rect.Width, rect.Height);
                int steps = Convert.ToInt32((((double)(rect.Width * rect.Height)) / 30));
                for (int i = 0; i <= steps; i++)
                {
                    graphics.FillEllipse(largeConfettiBrush, random.Next(rect.Width), random.Next(rect.Height),
                                         random.Next(Convert.ToInt32((((double)maxDimension) / 50))),
                                         random.Next(Convert.ToInt32((((double)maxDimension) / 50))));
                }
                font.Dispose();
                largeConfettiBrush.Dispose();
                graphics.Dispose();
            }
            return bitmap;
        }

        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(random.Next(xmin, xmax), random.Next(ymin, ymax));
        }
    }
}
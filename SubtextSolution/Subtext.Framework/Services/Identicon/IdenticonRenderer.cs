using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Docuverse.Identicon
{
    /// <summary>
    /// Code borrowed from http://identicon.codeplex.com/
    /// </summary>
    public class IdenticonRenderer
    {
        // Each "patch" in an Identicon is a polygon created from a list of vertices on a 5 by 5 grid.
        // Vertices are numbered from 0 to 24, starting from top-left corner of
        // the grid, moving left to right and top to bottom.
        private const int MaxSize = 128;
        private const int MinSize = 16;
        private const int PatchCells = 4;
        private const int PatchGrids = PatchCells + 1;
        private const byte PatchInverted = 2;
        private const byte PatchSymmetric = 1;
        private static readonly int[] CenterPatchTypes = new[] {0, 4, 8, 15};

        private static readonly byte[] Patch0 = new byte[] {0, 4, 24, 20, 0};
        private static readonly byte[] Patch1 = new byte[] {0, 4, 20, 0};
        private static readonly byte[] Patch10 = new byte[] {0, 2, 12, 10, 0};
        private static readonly byte[] Patch11 = new byte[] {10, 14, 22, 10};
        private static readonly byte[] Patch12 = new byte[] {20, 12, 24, 20};
        private static readonly byte[] Patch13 = new byte[] {10, 2, 12, 10};
        private static readonly byte[] Patch14 = new byte[] {0, 2, 10, 0};
        private static readonly byte[] Patch2 = new byte[] {2, 24, 20, 2};
        private static readonly byte[] Patch3 = new byte[] {2, 10, 14, 22, 2};
        private static readonly byte[] Patch4 = new byte[] {2, 14, 22, 10, 2};
        private static readonly byte[] Patch5 = new byte[] {0, 14, 24, 22, 0};
        private static readonly byte[] Patch6 = new byte[] {2, 24, 22, 13, 11, 22, 20, 2};
        private static readonly byte[] Patch7 = new byte[] {0, 14, 22, 0};
        private static readonly byte[] Patch8 = new byte[] {6, 8, 18, 16, 6};
        private static readonly byte[] Patch9 = new byte[] {4, 20, 10, 12, 2, 4};

        private static readonly byte[] PatchFlags =
            new byte[]
            {
                PatchSymmetric, 0, 0, 0, PatchSymmetric, 0, 0, 0, PatchSymmetric, 0, 0, 0, 0, 0, 0,
                (PatchSymmetric + PatchInverted)
            };

        private static readonly byte[][] PatchTypes =
            new[]
            {
                Patch0, Patch1, Patch2, Patch3, Patch4, Patch5, Patch6, Patch7, Patch8, Patch9, Patch10, Patch11, Patch12, Patch13,
                Patch14, Patch0
            };

        private int _patchOffset; // used to center patch shape at origin because shape rotation works correctly.

        private GraphicsPath[] _patchShapes;

        /// <summary>
        /// The size in pixels at which each patch will be rendered interally before they
        /// are scaled down to the requested identicon size. Default size is 20 pixels
        /// which means, for 9-block identicon, a 60x60 image will be rendered and
        /// scaled down.
        /// </summary>
        public int PatchSize
        {
            get;
            set;
        }

        private IEnumerable<GraphicsPath> CalculatePatchShapes()
        {
            _patchOffset = PatchSize / 2; // used to center patch shape at origin.
            int scale = PatchSize / PatchCells;
            foreach(var patchVertices in PatchTypes)
            {
                var patch = new GraphicsPath();
                foreach(int vertex in patchVertices)
                {
                    int xVertex = (vertex % PatchGrids * scale) - _patchOffset;
                    int yVertex = (vertex / PatchGrids * scale) - _patchOffset;
                    AddPointToGraphicsPath(patch, xVertex, yVertex);
                }
                yield return patch;
            }
        }

        /// <summary>
        /// Adds the X and Y coordinates to the current graphics path.
        /// </summary>
        /// <param name="path"> The current Graphics path</param>
        /// <param name="x">The x coordinate to be added</param>
        /// <param name="y">The y coordinate to be added</param>
        private static void AddPointToGraphicsPath(GraphicsPath path, int x, int y)
        {
            // increment by one.
            var points = new PointF[path.PointCount + 1];
            var pointTypes = new byte[path.PointCount + 1];

            if(path.PointCount == 0)
            {
                points[0] = new PointF(x, y);
                var newPath = new GraphicsPath(points, new[] {(byte)PathPointType.Start});
                path.AddPath(newPath, false);
            }
            else
            {
                path.PathPoints.CopyTo(points, 0);
                points[path.PointCount] = new Point(x, y);

                path.PathTypes.CopyTo(pointTypes, 0);
                pointTypes[path.PointCount] = (byte)PathPointType.Line;

                var tempGraphics = new GraphicsPath(points, pointTypes);
                path.Reset();
                path.AddPath(tempGraphics, false);
            }
        }

        /// <summary>
        /// Returns rendered identicon bitmap for a given identicon code.
        /// </summary>
        /// <param name="code">Identicon code</param>
        /// <param name="size">desired image size</param>
        public Bitmap Render(int code, int size)
        {
            // enforce size limits
            size = Math.Min(size, MaxSize);
            size = Math.Max(size, MinSize);

            // set patch size appropriately to avoid scaling artifacts
            if(size <= 24)
            {
                PatchSize = 16;
            }
            else if(size <= 40)
            {
                PatchSize = 20;
            }
            else if(size <= 64)
            {
                PatchSize = 32;
            }
            else if(size <= 128)
            {
                PatchSize = 48;
            }
            _patchShapes = CalculatePatchShapes().ToArray();

            // decode the code into parts:            
            // bit 0-1: middle patch type
            int centerType = CenterPatchTypes[code & 0x3];
            // bit 2: middle invert
            bool centerInvert = ((code >> 2) & 0x1) != 0;
            // bit 3-6: corner patch type
            int cornerType = (code >> 3) & 0x0f;
            // bit 7: corner invert
            bool cornerInvert = ((code >> 7) & 0x1) != 0;
            // bit 8-9: corner turns
            int cornerTurn = (code >> 8) & 0x3;
            // bit 10-13: side patch type
            int sideType = (code >> 10) & 0x0f;
            // bit 14: side invert
            bool sideInvert = ((code >> 14) & 0x1) != 0;
            // bit 15: corner turns
            int sideTurn = (code >> 15) & 0x3;
            // bit 16-20: blue color component
            int blue = (code >> 16) & 0x01f;
            // bit 21-26: green color component
            int green = (code >> 21) & 0x01f;
            // bit 27-31: red color component
            int red = (code >> 27) & 0x01f;

            // color components are used at top of the range for color difference
            // use white background for now. TODO: support transparency.
            Color foreColor = Color.FromArgb(red << 3, green << 3, blue << 3);
            Color backColor = Color.White;

            // outline shapes with a noticeable color (complementary will do) if
            // shape color and background color are too similar (measured by color
            // distance).
            Color strokeColor = Color.Empty;
            if(ColorDistance(ref foreColor, ref backColor) < 32f)
            {
                strokeColor = ComplementaryColor(ref foreColor);
            }

            // render at larger source size (to be scaled down later)
            int sourceSize = PatchSize * 3;
            using(var sourceImage = new Bitmap(sourceSize, sourceSize, PixelFormat.Format32bppRgb))
            {
                using(Graphics graphics = Graphics.FromImage(sourceImage))
                {
                    // center patch
                    DrawPatch(graphics, PatchSize, PatchSize, centerType, 0, centerInvert, ref foreColor, ref backColor,
                              ref strokeColor);

                    // side patch (top)
                    DrawPatch(graphics, PatchSize, 0, sideType, sideTurn++, sideInvert, ref foreColor, ref backColor,
                              ref strokeColor);
                    // side patch (right)
                    DrawPatch(graphics, PatchSize * 2, PatchSize, sideType, sideTurn++, sideInvert, ref foreColor, ref backColor,
                              ref strokeColor);
                    // side patch (bottom)
                    DrawPatch(graphics, PatchSize, PatchSize * 2, sideType, sideTurn++, sideInvert, ref foreColor, ref backColor,
                              ref strokeColor);
                    // side patch (left)
                    DrawPatch(graphics, 0, PatchSize, sideType, sideTurn, sideInvert, ref foreColor, ref backColor, ref strokeColor);

                    // corner patch (top left)
                    DrawPatch(graphics, 0, 0, cornerType, cornerTurn++, cornerInvert, ref foreColor, ref backColor, ref strokeColor);
                    // corner patch (top right)
                    DrawPatch(graphics, PatchSize * 2, 0, cornerType, cornerTurn++, cornerInvert, ref foreColor, ref backColor,
                              ref strokeColor);
                    // corner patch (bottom right)
                    DrawPatch(graphics, PatchSize * 2, PatchSize * 2, cornerType, cornerTurn++, cornerInvert, ref foreColor,
                              ref backColor, ref strokeColor);
                    // corner patch (bottom left)
                    DrawPatch(graphics, 0, PatchSize * 2, cornerType, cornerTurn, cornerInvert, ref foreColor, ref backColor,
                              ref strokeColor);
                }
                // scale source image to target size with bicubic smoothing
                return ScaleImage(size, sourceImage);
            }
        }

        private static Bitmap ScaleImage(int size, Image sourceImage)
        {
            var bitmap = new Bitmap(size, size, PixelFormat.Format32bppRgb);
            using(Graphics g = Graphics.FromImage(bitmap))
            {
                var fudge = (int)(size * 0.016); // this is necessary to prevent scaling artifacts at larger sizes
                g.DrawImage(sourceImage, 0, 0, size + fudge, size + fudge);
            }
            return bitmap;
        }

        private void DrawPatch(Graphics g, int x, int y, int patch, int turn, bool invert, ref Color fore, ref Color back, ref Color stroke)
        {
            patch %= PatchTypes.Length;
            turn %= 4;
            if((PatchFlags[patch] & PatchInverted) != 0)
            {
                invert = !invert;
            }

            // paint the background
            g.FillRegion(new SolidBrush(invert ? fore : back), new Region(new Rectangle(x, y, PatchSize, PatchSize)));

            // offset and rotate coordinate space by patch position (x, y) and
            // 'turn' before rendering patch shape
            Matrix m = g.Transform;
            g.TranslateTransform((x + _patchOffset), (y + _patchOffset));
            g.RotateTransform(turn * 90);

            // if stroke color was specified, apply stroke
            // stroke color should be specified if fore color is too close to the back color.
            if(!stroke.IsEmpty)
            {
                g.DrawPath(new Pen(stroke), _patchShapes[patch]);
            }

            // render rotated patch using fore color (back color if inverted)
            g.FillPath(new SolidBrush(invert ? back : fore), _patchShapes[patch]);

            // restore previous rotation
            g.Transform = m;
        }

        /// <summary>Returns distance between two colors</summary>		
        private static float ColorDistance(ref Color c1, ref Color c2)
        {
            float dx = c1.R - c2.R;
            float dy = c1.G - c2.G;
            float dz = c1.B - c2.B;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>Returns complementary color</summary>
        private static Color ComplementaryColor(ref Color c)
        {
            return Color.FromArgb(c.ToArgb() ^ 0x00FFFFFF);
        }
    }
}
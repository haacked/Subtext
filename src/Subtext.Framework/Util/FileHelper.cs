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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Util
{
    public static class FileHelper
    {
        static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        public static void EnsureDirectory(string directoryPath)
        {
            if (String.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentNullException("directoryPath");
            }

            string dir = Path.GetFullPath(directoryPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void WriteBytesToFile(string destinationFilePath, byte[] data)
        {
            if (String.IsNullOrEmpty(destinationFilePath))
            {
                throw new ArgumentNullException("destinationFilePath");
            }

            if (!IsValidFilePath(destinationFilePath))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  Resources.InvalidOperation_InvalidCharactersInFileName,
                                                                  destinationFilePath));
            }

            using (var stream = new FileStream(destinationFilePath, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(data);
                }
            }
        }

        public static bool IsValidImageFilePath(string filePath)
        {
            return IsValidFilePath(filePath, ImageExtensions);
        }

        public static bool IsValidFilePath(string filePath)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            return !invalidChars.Any(c => filePath.Contains(c));
        }

        public static bool IsValidFilePath(string filePath, IEnumerable<string> extensions)
        {
            return IsValidFilePath(filePath) &&
                   extensions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Util
{
    public static class FileHelper
    {
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
                throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InvalidCharactersInFileName, destinationFilePath));
            }

            using (FileStream stream = new FileStream(destinationFilePath, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(data);
                }
            }
        }

        static string[] _imageExtensions = { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };
        public static bool IsValidImageFilePath(string filePath)
        {
            return IsValidFilePath(filePath, _imageExtensions);
        }

        public static bool IsValidFilePath(string filePath)
        {
            var invalidChars = Path.GetInvalidPathChars();
            return !invalidChars.Any(c => filePath.Contains(c));
        }

        public static bool IsValidFilePath(string filePath, IEnumerable<string> extensions)
        {
            return IsValidFilePath(filePath) &&
                extensions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}

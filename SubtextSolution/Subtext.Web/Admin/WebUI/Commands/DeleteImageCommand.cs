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
using System.Globalization;
using System.IO;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Commands
{
    [Serializable]
    public class DeleteImageCommand : DeleteTitledTargetCommand
    {
        public DeleteImageCommand(int imageId)
        {
            _targetName = "Image";
            _targetID = imageId;
            itemTitle = "Image " + imageId.ToString(CultureInfo.InvariantCulture);
        }

        public DeleteImageCommand(int imageID, string imageTitle)
        {
            _targetName = "Image";
            _targetID = imageID;
            itemTitle = imageTitle;
        }

        public override string Execute()
        {
            try
            {
                Image currentImage = Images.GetSingleImage(_targetID, false);

                // The following line should be fully encapsulated and handle files + data
                // For now, I commented out the file trys in the the object so it can do just
                // data without exception. I'll do the files locally until we decide to really
                // do the framework class

                Images.DeleteImage(currentImage);

                // now delete the associated files if they exist
                string galleryFolder = Images.LocalGalleryFilePath(currentImage.CategoryID);
                if (Directory.Exists(galleryFolder))
                {
                    DeleteFile(galleryFolder, currentImage.OriginalFile);
                    DeleteFile(galleryFolder, currentImage.ResizedFile);
                    DeleteFile(galleryFolder, currentImage.ThumbNailFile);
                }

                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
            }
        }

        private void DeleteFile(string path, string filename)
        {
            string localPath = Path.Combine(path, filename);
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }
    }
}

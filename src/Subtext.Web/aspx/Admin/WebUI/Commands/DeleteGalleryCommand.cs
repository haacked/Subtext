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
using System.IO;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Web.Admin.Commands
{
    // TODO: derivation is fine, but expose the prompts so you can add a WARNING WILL DELETE 
    // FILES TOO message for this one (makes general sense as well)
    [Serializable]
    public class DeleteGalleryCommand : DeleteTitledTargetCommand
    {
        HttpServerUtilityBase _server;

        public DeleteGalleryCommand(ObjectRepository repository, HttpServerUtilityBase server, string galleryDirectoryPath, int galleryId, string galleryTitle)
        {
            _server = server;
            _targetID = galleryId;
            itemTitle = galleryTitle;
            GalleryDirectoryPath = galleryDirectoryPath;
            Repository = repository;
        }

        protected ObjectRepository Repository
        {
            get;
            private set;
        }

        public string GalleryDirectoryPath { get; private set; }

        public override string Execute()
        {
            try
            {
                ICollection<Image> imageList = Repository.GetImagesByCategory(_targetID, false);

                // delete the folder
                string galleryFolder = _server.MapPath(GalleryDirectoryPath);
                if (Directory.Exists(galleryFolder))
                {
                    Directory.Delete(galleryFolder, true);
                }

                if (imageList.Count > 0)
                {
                    // delete from data provider
                    foreach (Image currentImage in imageList)
                    {
                        Repository.Delete(currentImage);
                    }
                }

                // finally, delete the gallery (category) itself from the data provider
                Repository.DeleteLinkCategory(_targetID);
                return FormatMessage(ExecuteSuccessMessage, _targetName, itemTitle);
            }
            catch (Exception ex)
            {
                return FormatMessage(ExecuteFailureMessage, _targetName, _targetID, ex.Message);
            }
        }
    }
}
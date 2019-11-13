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
using System.IO;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Admin
{
    /// <summary>
    /// Implements the FreeTextBox image gallery.
    /// </summary>
    public class ftb_imagegallery : SubtextPage
    {
        protected PlaceHolder errorMsg;
        protected Label folderName;
        protected ImageGallery imageGallery;

        protected override void OnLoad(EventArgs e)
        {
            string imageDirectoryPath = Url.ImageDirectoryPath(Blog);
            try
            {
                if (!Directory.Exists(imageDirectoryPath))
                {
                    Directory.CreateDirectory(imageDirectoryPath);
                }
            }
            catch (Exception)
            {
                imageGallery.Visible = false;
                errorMsg.Visible = true;
                folderName.Text = imageDirectoryPath;
            }

            //TODO: Fix this up....
            /*
			string currentFolder = imageGallery.CurrentImagesFolder;
	
			// modify the directories allowed
			if (currentFolder == "~/images") 
			{

				// these are the default directories FTB:ImageGallery will find
				string[] defaultDirectories = System.IO.Directory.GetDirectories(Server.MapPath(currentFolder),"*");
		
				// user defined custom directories
				string[] customDirectories = new string[] {"folder1","folder2"};
		
				// the gallery will use these images in this instance
				imageGallery.CurrentDirectories = customDirectories;
			}
	
	
			// modify the images allowed
			if (currentFolder == "~/images") 
			{

				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Server.MapPath(currentFolder));

				// these are the default images FTB:ImageGallery will find
				System.IO.FileInfo[] defaultImages = directoryInfo.GetFiles("*");
		
				// user defined custom images (here, we're just allowing the first two)
				System.IO.FileInfo[] customImages = new System.IO.FileInfo[2] {defaultImages[0], defaultImages[1]};
		
				// the gallery will use these images in this instance
				imageGallery.CurrentImages = customImages;
			}	
			*/
            base.OnLoad(e);
        }
    }
}
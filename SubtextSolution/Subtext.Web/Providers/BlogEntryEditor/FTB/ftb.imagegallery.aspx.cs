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
using System.IO;

namespace Subtext.Web.Admin
{
	/// <summary>
	/// Implements the FreeTextBox image gallery.
	/// </summary>
	public class ftb_imagegallery : System.Web.UI.Page
	{
		protected FreeTextBoxControls.ImageGallery imageGallery;
        protected System.Web.UI.WebControls.PlaceHolder errorMsg;
        protected System.Web.UI.WebControls.Label folderName;

		/// <summary>
		/// Method called when the page loads.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void Page_Load(object sender, System.EventArgs e)
		{
            string blogImageRootPath = Subtext.Framework.Format.UrlFormats.StripHostFromUrl(Subtext.Framework.Configuration.Config.CurrentBlog.ImagePath);

            string phisicalImageRootPath = Server.MapPath(blogImageRootPath);
            try
            {
                if (!Directory.Exists(phisicalImageRootPath))
                {
                    Directory.CreateDirectory(phisicalImageRootPath);
                }
            }
            catch (Exception)
            {
                imageGallery.Visible = false;
                errorMsg.Visible = true;
                folderName.Text = phisicalImageRootPath;
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
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}

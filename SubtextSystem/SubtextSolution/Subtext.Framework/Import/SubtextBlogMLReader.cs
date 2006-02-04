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
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BlogML;
using BlogML.Xml;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;
using Image = System.Drawing.Image;

namespace Subtext.Framework.Import
{
	/// <summary>
	/// Reads and imports a BlogML file.  This is based on the BlogML standard 
	/// proposed by Darren Neimke in 
	/// <see href="http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236">http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236</see>
	/// </summary>
	public sealed class SubtextBlogMLReader
	{
		private readonly static ILog log = new Log();

		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextBlogMLReader"/> class.
		/// </summary>
		public SubtextBlogMLReader()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Reads a BlogML file and imports the data into a subText blog. This method 
		/// can import the data into an existing subText blog, or create a new one.
		/// </summary>
		/// <param name="blogMLFile">the BlogML file to import</param>
		/// <param name="createNewBlog">if TRUE, a new blog will be created, FALSE indicates 
		/// this method is being called from an existing blog.</param>
		public void ReadBlog(string blogMLFile, bool createNewBlog)
		{
			BlogMLBlog bmlBlog = null;
			Hashtable categoryMap = new Hashtable(); // will be used to map the blogML catID to real catID
			int[] catIDs = null;
			int count = 0;
			bool duplicateComments = true;
			string fileName = null;
			string imgDirPath = null;
			string imgDirUrl = null;
			string imgFilePath = null;
			string imgFileUrl = null;
			BlogInfo info = null;
			Entry newComment = null;
			Entry newEntry = null;
			Entry newPingTrack = null;
			int newEntryID = NullValue.NullInt32;

			try
			{
				bmlBlog = BlogMLSerializer.Deserialize(new StringReader(blogMLFile));
			}
			catch(Exception e)
			{
				string msg = (blogMLFile.Length == 0) ? 
					"The specified BlogML file could not be found or does not exist." :
					"There was an error trying to read the given BlogML file.";

				throw new BlogImportException(msg, e);
			}

			/* 
			 *	1) Add a new blog (subtext_Config)
			 *	2) Add categories and setup categoryMap w/ {blogMLCatID = subtextCatID}
			 *	3) Add all content (post, story, comment, track/pingbacks, undeclared) as defined 
			 *		by PostType.cs
			 *	4) 
			 */ 
			
			// 1) This certainly needs to be beefed up to calculate the correct _host & app values
			if(createNewBlog)
			{
				Config.CreateBlog(bmlBlog.Title, bmlBlog.Author.Name, "password", bmlBlog.RootUrl, string.Empty);
				info = Config.GetBlogInfo(bmlBlog.RootUrl, string.Empty);

				info.SubTitle = StringHelper.ReturnCheckForNull(bmlBlog.SubTitle);
				info.Email = StringHelper.ReturnCheckForNull(bmlBlog.Author.Email);
				Config.UpdateConfigData(info);
			}
			else
			{
				info = Config.CurrentBlog;
			}

			// get the current settings for Duplicate Comments so we can restore it later.
			duplicateComments = info.DuplicateCommentsEnabled;
			info.DuplicateCommentsEnabled = true;
			Config.UpdateConfigData(info);

			// get the needed paths for our image attachments
			imgDirPath = info.ImageDirectory;
			imgDirUrl = info.ImagePath;
			
			// 2)
			foreach(BlogMLCategory bmlCat in bmlBlog.Categories)
			{
				LinkCategory category = new LinkCategory();
				category.BlogID = info.BlogID;
				category.Title = bmlCat.Title;
				category.Description = bmlCat.Description;
				category.IsActive = bmlCat.Approved;
				category.CategoryType = CategoryType.PostCollection;

				// now add the category and map its ID
				categoryMap.Add(bmlCat.ID, Links.CreateLinkCategory(category));
			}

			// 3)

			foreach(BlogMLPost bmlPost in bmlBlog.Posts)
			{
				string postContent = bmlPost.Content.UncodedText;

				if(bmlPost.Attachments.Count > 0)
				{
					if(!Directory.Exists(imgDirPath))
					{
						Directory.CreateDirectory(imgDirPath);
					}

					foreach(BlogMLAttachment bmlAttachment in bmlPost.Attachments)
					{
						fileName = Path.GetFileName(bmlAttachment.Url);
						imgFilePath = imgDirPath + fileName;
						imgFileUrl = imgDirUrl + fileName;

						postContent = BlogMLWriterBase.SgmlUtil.CleanAttachmentUrls(
										postContent, 
										bmlAttachment.Url,
										imgFileUrl);

						if(bmlAttachment.Embedded == true) 
						{
							MemoryStream memStream = new MemoryStream(bmlAttachment.Data);
							Bitmap image = (Bitmap)Image.FromStream(memStream);

							try
							{
								using(FileStream fStream = new FileStream(imgFilePath, FileMode.CreateNew)) 
								{
									image.Save(fStream, ImageFormat.Jpeg);
								}
							}
							catch(IOException ioE)
							{
								log.Warn("It looks like this image already exists. "
									+ "We'll use the existing one, so you may need to fix it later. The Error Message:",
									ioE);
							}
						}
					}
				} // End Attachments for Post
                
				newEntry = new Entry(PostType.BlogPost);
				newEntry.BlogID = info.BlogID;
				newEntry.Title = bmlPost.Title;
				newEntry.DateCreated = bmlPost.DateCreated;
				newEntry.DateUpdated = bmlPost.DateModified;
				newEntry.DateSyndicated = bmlPost.DateModified;  // is this really the best thing to do?
				newEntry.Body = postContent;
				newEntry.IsActive = bmlPost.Approved;
				newEntry.DisplayOnHomePage = bmlPost.Approved;
				newEntry.IncludeInMainSyndication = bmlPost.Approved;
				newEntry.IsAggregated = bmlPost.Approved;
				newEntry.AllowComments = true;
				newEntryID = Entries.Create(newEntry);

				count = bmlPost.Categories.Count;
				if(count > 0)
				{
					catIDs = new int[count];
					for(int j = 0; j < count; j++)
					{
						catIDs[j] = (int) categoryMap[bmlPost.Categories[j].Ref];
					}
					Entries.SetEntryCategoryList(newEntryID, catIDs);
				}

				if(bmlPost.Comments.Count > 0)
				{
					foreach(BlogMLComment bmlComment in bmlPost.Comments)
					{
						newComment = new Entry(PostType.Comment);
						newComment.BlogID = info.BlogID;
						newComment.ParentID = newEntryID;
						newComment.Title = StringHelper.ReturnCheckForNull(bmlComment.Title);
						newComment.DateCreated = bmlComment.DateCreated;
						newComment.DateUpdated = bmlComment.DateModified;
						newComment.DateSyndicated = bmlComment.DateCreated;
						newComment.Body = StringHelper.ReturnCheckForNull(bmlComment.Content.UncodedText);
						newComment.IsActive = bmlComment.Approved;
						newComment.Author = StringHelper.ReturnCheckForNull(bmlComment.UserName);
						newComment.TitleUrl = StringHelper.ReturnCheckForNull(bmlComment.UserUrl);
						newComment.Email = bmlComment.UserEMail; // not sure where/if we use this?

						try
						{
							Entries.InsertComment(newComment);
						}
						catch(CommentDuplicateException cDE)
						{
							log.Warn(
								string.Format(
									"Attempted to import a duplicate comment. Comment ID from BlogML file: {0}",
									bmlComment.ID), cDE);
						}
					}
				}

				if(bmlPost.Trackbacks.Count > 0)
				{
					foreach(BlogMLTrackback bmlPingTrack in bmlPost.Trackbacks)
					{
						newPingTrack = new Entry(PostType.PingTrack);
						newPingTrack.BlogID = info.BlogID;
						newPingTrack.ParentID = newEntryID;
						newPingTrack.Title = bmlPingTrack.Title;
						newPingTrack.TitleUrl = bmlPingTrack.Url;
						newPingTrack.IsActive = bmlPingTrack.Approved;
						newPingTrack.DateCreated = bmlPingTrack.DateCreated;
						newPingTrack.DateUpdated = bmlPingTrack.DateModified;
						newPingTrack.DateSyndicated = bmlPingTrack.DateCreated;
						// we use an actual name here, but BlogML doesn't support this, so let's try  
						// to parse the url's host out of the url.
						newPingTrack.Author = UrlFormats.GetHostFromExternalUrl(bmlPingTrack.Url);
						// so the duplicate Comment Filter doesn't break when computing the checksum
						newPingTrack.Body = string.Empty; 

						Entries.Create(newPingTrack);
					}
				}

			} // End Posts

			//restore the Dup Comments settings
			info.DuplicateCommentsEnabled = duplicateComments;
			Config.UpdateConfigData(info);
		}
	}
}

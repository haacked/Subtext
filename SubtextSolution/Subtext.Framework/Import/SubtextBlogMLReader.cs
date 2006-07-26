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
using System.Collections.Generic;
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
		}

	    /// <summary>
	    /// Reads in a BlogML Stream and creates the appropriate blog posts, 
	    /// 
	    /// </summary>
	    /// <param name="blogMlStream"></param>
	    /// <param name="option"></param>
        public void ReadBlog(Stream blogMlStream, BlogMlReaderOption option)
	    {
            BlogMLBlog blogMlBlog = DeserializeBlogMlStream(blogMlStream);

            BlogInfo info;
            
	        if(option == BlogMlReaderOption.CreateNewBlog)
                info = CreateNewBlog(blogMlBlog);
            else
                info = Config.CurrentBlog;

	        //Bookmarking the current setting.
            bool duplicateCommentsEnabled = info.DuplicateCommentsEnabled;
            if (!duplicateCommentsEnabled)
            {
                // Allow duplicate comments temporarily.
                info.DuplicateCommentsEnabled = true;
                Config.UpdateConfigData(info);
            }

            IDictionary<string, int> categoryMap = CreateCategoriesFromBlogMl(blogMlBlog, info);

            foreach (BlogMLPost bmlPost in blogMlBlog.Posts)
            {
                string postContent = bmlPost.Content.UncodedText;

                if (bmlPost.Attachments.Count > 0)
                {
                    if (info.ImagePath != null && info.ImageDirectory != null)
                    {
                        //Updates the post content with new attachment urls.
                        postContent = CreateFilesFromAttachments(bmlPost, info.ImageDirectory, info.ImagePath, postContent);
                    }
                }

                int newEntryID = CreateBlogEntry(bmlPost, info, postContent);

                if (bmlPost.Categories.Count > 0)
                {
                    SetPostCategories(bmlPost, categoryMap, newEntryID);
                }

                if (bmlPost.Comments.Count > 0)
                {
                    foreach (BlogMLComment bmlComment in bmlPost.Comments)
                    {
                        CreateNewCommentFromBlogMlComment(bmlComment, info, newEntryID);
                    }
                }

                if (bmlPost.Trackbacks.Count > 0)
                {
                    foreach (BlogMLTrackback bmlPingTrack in bmlPost.Trackbacks)
                    {
                        CreateTrackbackFromBlogMlPingTrack(bmlPingTrack, info, newEntryID);
                    }
                }

            } // End Posts

            //restore the Dup Comments settings
            if (info.DuplicateCommentsEnabled != duplicateCommentsEnabled)
            {
                info.DuplicateCommentsEnabled = duplicateCommentsEnabled;
                Config.UpdateConfigData(info);
            }
	    }

	    BlogMLBlog DeserializeBlogMlStream(Stream stream)
	    {
	        try
	        {
                return BlogMLSerializer.Deserialize(stream);
	        }
	        catch(Exception e)
	        {
                throw new BlogImportException("Error occurred while reading the specified blogML file.", e);
	        }
	    }

        private static BlogInfo CreateNewBlog(BlogMLBlog blogMlBlog)
        {
            //TODO: This certainly needs to be beefed up to calculate the correct _host & app values
            Config.CreateBlog(blogMlBlog.Title, blogMlBlog.Author.Name, "password", blogMlBlog.RootUrl, string.Empty);
            BlogInfo info = Config.GetBlogInfo(blogMlBlog.RootUrl, string.Empty);

            info.SubTitle = StringHelper.ReturnCheckForNull(blogMlBlog.SubTitle);
            info.Email = StringHelper.ReturnCheckForNull(blogMlBlog.Author.Email);
            Config.UpdateConfigData(info);
            return info;
        }

        private static IDictionary<string, int> CreateCategoriesFromBlogMl(BlogMLBlog blogMlBlog, BlogInfo info)
        {
            Dictionary<string, int> categoryMap = new Dictionary<string, int>();
            foreach (BlogMLCategory bmlCat in blogMlBlog.Categories)
            {
                LinkCategory category = new LinkCategory();
                category.BlogId = info.Id;
                category.Title = bmlCat.Title;
                category.Description = bmlCat.Description;
                category.IsActive = bmlCat.Approved;
                category.CategoryType = CategoryType.PostCollection;

                // now add the category and map its ID
                categoryMap.Add(bmlCat.ID, Links.CreateLinkCategory(category));
            }
            return categoryMap;
        }


	    private static void CreateTrackbackFromBlogMlPingTrack(BlogMLTrackback bmlPingTrack, BlogInfo info, int newEntryID)
	    {
	        Entry newPingTrack;
	        newPingTrack = new Entry(PostType.PingTrack);
	        newPingTrack.BlogId = info.Id;
	        newPingTrack.ParentID = newEntryID;
	        newPingTrack.Title = bmlPingTrack.Title;
	        newPingTrack.AlternativeTitleUrl = bmlPingTrack.Url;
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

	    private static void CreateNewCommentFromBlogMlComment(BlogMLComment bmlComment, BlogInfo info, int newEntryID)
	    {
	        Entry newComment = new Entry(PostType.Comment);
	        newComment.BlogId = info.Id;
	        newComment.ParentID = newEntryID;
	        newComment.Title = StringHelper.ReturnCheckForNull(bmlComment.Title);
	        newComment.DateCreated = bmlComment.DateCreated;
	        newComment.DateUpdated = bmlComment.DateModified;
	        newComment.DateSyndicated = bmlComment.DateCreated;
	        newComment.Body = StringHelper.ReturnCheckForNull(bmlComment.Content.UncodedText);
	        newComment.IsActive = bmlComment.Approved;
	        newComment.Author = StringHelper.ReturnCheckForNull(bmlComment.UserName);
	        newComment.AlternativeTitleUrl = StringHelper.ReturnCheckForNull(bmlComment.UserUrl);
	        newComment.Email = bmlComment.UserEMail; // not sure where/if we use this?

	        try
	        {
	            Entries.InsertComment(newComment);
	        }
	        catch (CommentDuplicateException cDE)
	        {
	            log.Warn(
	                string.Format(
	                    "Attempted to import a duplicate comment. Comment ID from BlogML file: {0}",
	                    bmlComment.ID), cDE);
	        }
	    }

	    private static string CreateFilesFromAttachments(BlogMLPost bmlPost, string imgDirPath, string imgDirUrl, string postContent)
	    {
            if (imgDirPath == null)
                throw new ArgumentNullException("imgDirPath", "The blog's image directory path is null.");

            if (imgDirUrl == null)
                throw new ArgumentNullException("imgDirUrl", "The blog's image directory url is null.");
	        
	        if (!Directory.Exists(imgDirPath))
	        {
	            Directory.CreateDirectory(imgDirPath);
	        }

	        foreach (BlogMLAttachment bmlAttachment in bmlPost.Attachments)
	        {
	            postContent = CreateFileFromAttachment(bmlAttachment, imgDirPath, imgDirUrl, postContent);
	        }
	        return postContent;
	    }

	    private static string CreateFileFromAttachment(BlogMLAttachment bmlAttachment, string attachmentDirectoryPath, string attachmentDirectoryUrl, string postContent)
	    {
	        string fileName = Path.GetFileName(bmlAttachment.Url);
	        string attachmentPath = Path.Combine(attachmentDirectoryPath, fileName);
	        string attachmentUrl = attachmentDirectoryUrl + fileName;

	        postContent = BlogMLWriterBase.SgmlUtil.CleanAttachmentUrls(
	            postContent, 
	            bmlAttachment.Url,
	            attachmentUrl);

	        if(bmlAttachment.Embedded) 
	        {
	            try
	            {
	                //Can't assume it's an image.
	                using(FileStream fStream = new FileStream(attachmentPath, FileMode.CreateNew)) 
	                {
	                    using(BinaryWriter writer = new BinaryWriter(fStream))
	                    {
	                        writer.Write(bmlAttachment.Data);
	                    }
	                }
	            }
	            catch(FileNotFoundException fnfE)
	            {
                    log.Warn("It looks like this image already exists. "
                             + "We'll use the existing one, so you may need to fix it later. The Error Message:", fnfE);
	            }
	            catch (IOException ioE)
	            {
	                log.Warn(string.Format("Hmmm, some other error occured while writing the file '{0}'", bmlAttachment.Path), ioE);
	            }
	        }
	        return postContent;
	    }

	    private static int CreateBlogEntry(BlogMLPost bmlPost, BlogInfo info, string postContent)
	    {
	        Entry newEntry;
	        int newEntryID;
	        newEntry = new Entry(PostType.BlogPost);
	        newEntry.BlogId = info.Id;
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
	        return newEntryID;
	    }

        private static void SetPostCategories(BlogMLPost bmlPost, IDictionary<string, int> categoryMap, int newEntryID)
        {
            int[] catIDs = new int[bmlPost.Categories.Count];
            for (int j = 0; j < bmlPost.Categories.Count; j++)
            {
                catIDs[j] = categoryMap[bmlPost.Categories[j].Ref];
            }
            Entries.SetEntryCategoryList(newEntryID, catIDs);
        }
	}
    
    public enum BlogMlReaderOption
    {
        None = 0,
        CreateNewBlog = 1,
    }
}

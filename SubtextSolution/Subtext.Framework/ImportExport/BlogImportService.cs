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
using System.Web;
using BlogML;
using BlogML.Xml;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;

namespace Subtext.ImportExport
{
    public class BlogImportService : IBlogImportService
    {
        private readonly static ILog Log = new Log();

        public BlogImportService(IBlogImportRepository repository)
        {
            Repository = repository;
        }

        public IBlogImportRepository Repository { get; private set; }

        public void ImportBlog(Stream stream)
        {
            var importedBlog = BlogMLSerializer.Deserialize(stream);
            ImportBlog(importedBlog);
        }

        public void ImportBlog(BlogMLBlog blog)
        {
            using(Repository.SetupBlogForImport())
            {
                Import(blog);
            }
        }

        public void Import(BlogMLBlog blog)
        {
            Repository.SetExtendedProperties(blog.ExtendedProperties);

            Repository.CreateCategories(blog);

            foreach(BlogMLPost bmlPost in blog.Posts)
            {
                ImportBlogPost(blog, bmlPost);
            }

        }

        private void ImportBlogPost(BlogMLBlog blog, BlogMLPost bmlPost)
        {
            if(bmlPost.Attachments.Count > 0)
            {
                //Updates the post content with new attachment urls.
                bmlPost.Content = BlogMLContent.Create(CreateFilesFromAttachments(bmlPost), true /*base64*/);
            }

            string newEntryId = Repository.CreateBlogPost(blog, bmlPost);

            foreach(BlogMLComment bmlComment in bmlPost.Comments)
            {
                try
                {
                    Repository.CreateComment(bmlComment, newEntryId);
                }
                catch(Exception e)
                {
                    LogError(Resources.Import_ErrorWhileImportingComment, e);
                }
            }

            foreach(BlogMLTrackback bmlPingTrack in bmlPost.Trackbacks)
            {
                try
                {
                    Repository.CreateTrackback(bmlPingTrack, newEntryId);
                }
                catch(Exception e)
                {
                    LogError(Resources.Import_ErrorWhileImportingComment, e);
                }
            }
        }

        /// <summary>
        /// Lets the provider decide how to log errors.
        /// </summary>
        public void LogError(string message, Exception exception)
        {
            Log.Error(message, exception);
        }

        public string CreateFilesFromAttachments(BlogMLPost post)
        {
            string postContent = post.Content.UncodedText;
            foreach(BlogMLAttachment bmlAttachment in post.Attachments)
            {
                string assetDirPath = Repository.GetAttachmentDirectoryPath();
                string assetDirUrl = Repository.GetAttachmentDirectoryUrl();

                if(!String.IsNullOrEmpty(assetDirPath) && !String.IsNullOrEmpty(assetDirUrl))
                {
                    if(!Directory.Exists(assetDirPath))
                    {
                        Directory.CreateDirectory(assetDirPath);
                    }
                    postContent = CreateFileFromAttachment(bmlAttachment, assetDirPath, assetDirUrl, postContent);
                }
            }
            return postContent;
        }

        public static string CreateFileFromAttachment(BlogMLAttachment attachment, string attachmentDirectoryPath,
                                                       string attachmentDirectoryUrl, string postContent)
        {
            string fileName = Path.GetFileName(attachment.Url);
            string attachmentPath = HttpUtility.UrlDecode(Path.Combine(attachmentDirectoryPath, fileName));
            string newAttachmentUrl = attachmentDirectoryUrl + fileName;

            postContent = BlogMLWriterBase.SgmlUtil.CleanAttachmentUrls(
                postContent,
                attachment.Url,
                newAttachmentUrl);

            if(attachment.Embedded)
            {
                if(!File.Exists(attachmentPath))
                {
                    using(var fStream = new FileStream(attachmentPath, FileMode.CreateNew))
                    {
                        using(var writer = new BinaryWriter(fStream))
                        {
                            writer.Write(attachment.Data);
                        }
                    }
                }
            }
            return postContent;
        }
    }
}
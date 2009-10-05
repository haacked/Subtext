#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using BlogML;
using BlogML.Xml;
using Subtext.Framework.Properties;

namespace Subtext.ImportExport
{
    public class BlogMLReader
    {
        private static BlogMLBlog DeserializeBlogMlStream(Stream stream)
        {
            return BlogMLSerializer.Deserialize(stream);
        }

        /// <summary>
        /// Reads in a BlogML Stream and creates the appropriate blog posts, 
        /// </summary>
        public virtual void ReadBlog(IBlogMlImportService importService, Stream blogMLStream)
        {
            if(blogMLStream == null)
            {
                throw new ArgumentNullException("blogMLStream");
            }

            BlogMLBlog blog = DeserializeBlogMlStream(blogMLStream);
            ImportBlog(importService, blog);
        }

        public void ImportBlog(IBlogMlImportService importService, BlogMLBlog blog)
        {
            importService.SetBlogMLExtendedProperties(blog.ExtendedProperties);

            IDictionary<string, string> categoryIdMap = importService.CreateCategories(blog);

            foreach(BlogMLPost bmlPost in blog.Posts)
            {
                if(bmlPost.Attachments.Count > 0)
                {
                    //Updates the post content with new attachment urls.
                    bmlPost.Content.Text = CreateFilesFromAttachments(importService, bmlPost, bmlPost.Content.Text);
                }

                string newEntryId = importService.CreateBlogPost(blog, bmlPost, categoryIdMap);

                foreach(BlogMLComment bmlComment in bmlPost.Comments)
                {
                    try
                    {
                        importService.CreatePostComment(bmlComment, newEntryId);
                    }
                    catch(Exception e)
                    {
                        importService.LogError(Resources.Import_ErrorWhileImportingComment, e);
                    }
                }

                foreach(BlogMLTrackback bmlPingTrack in bmlPost.Trackbacks)
                {
                    try
                    {
                        importService.CreatePostTrackback(bmlPingTrack, newEntryId);
                    }
                    catch(Exception e)
                    {
                        importService.LogError(Resources.Import_ErrorWhileImportingComment, e);
                    }
                }
            } // End Posts
        }

        private static string CreateFilesFromAttachments(IBlogMlImportService importService, BlogMLPost bmlPost, string postContent)
        {
            foreach(BlogMLAttachment bmlAttachment in bmlPost.Attachments)
            {
                string assetDirPath = importService.GetAttachmentDirectoryPath(bmlAttachment);

                string assetDirUrl = importService.GetAttachmentDirectoryUrl(bmlAttachment);

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

        private static string CreateFileFromAttachment(BlogMLAttachment bmlAttachment, string attachmentDirectoryPath,
                                                       string attachmentDirectoryUrl, string postContent)
        {
            string fileName = Path.GetFileName(bmlAttachment.Url);
            string attachmentPath = HttpUtility.UrlDecode(Path.Combine(attachmentDirectoryPath, fileName));
            string attachmentUrl = attachmentDirectoryUrl + fileName;

            if(bmlAttachment.Embedded)
            {
                postContent = BlogMLWriterBase.SgmlUtil.CleanAttachmentUrls(
                    postContent,
                    bmlAttachment.Url,
                    attachmentUrl);

                if(!File.Exists(attachmentPath))
                {
                    using(var fStream = new FileStream(attachmentPath, FileMode.CreateNew))
                    {
                        using(var writer = new BinaryWriter(fStream))
                        {
                            writer.Write(bmlAttachment.Data);
                        }
                    }
                }
            }
            return postContent;
        }
    }
}
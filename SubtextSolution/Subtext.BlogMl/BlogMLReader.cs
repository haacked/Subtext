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
using Subtext.BlogML.Interfaces;
using Subtext.BlogML.Properties;

namespace Subtext.BlogML
{
    public class BlogMLReader
    {
        IBlogMLProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogMLReader" /> class.
        /// </summary>
        private BlogMLReader(IBlogMLProvider provider)
        {
            this.provider = provider;
        }

        public static BlogMLReader Create(IBlogMLProvider provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            return new BlogMLReader(provider);
        }

        private static BlogMLBlog DeserializeBlogMlStream(Stream stream)
        {
            return BlogMLSerializer.Deserialize(stream);
        }

        /// <summary>
        /// Reads in a BlogML Stream and creates the appropriate blog posts, 
        /// </summary>
        /// <param name="blogMlStream"></param>
        public void ReadBlog(Stream blogMLStream)
        {
            if(blogMLStream == null)
            {
                throw new ArgumentNullException("blogMLStream");
            }

            BlogMLBlog blog = DeserializeBlogMlStream(blogMLStream);

            provider.PreImport();

            provider.SetBlogMLExtendedProperties(blog.ExtendedProperties);

            IDictionary<string, string> categoryIdMap = provider.CreateCategories(blog);

            foreach(BlogMLPost bmlPost in blog.Posts)
            {
                if(bmlPost.Attachments.Count > 0)
                {
                    //Updates the post content with new attachment urls.
                    bmlPost.Content.Text = CreateFilesFromAttachments(bmlPost, bmlPost.Content.Text);
                }

                string newEntryID = provider.CreateBlogPost(blog, bmlPost, categoryIdMap);

                if(bmlPost.Comments.Count > 0)
                {
                    foreach(BlogMLComment bmlComment in bmlPost.Comments)
                    {
                        try
                        {
                            provider.CreatePostComment(bmlComment, newEntryID);
                        }
                        catch(Exception e)
                        {
                            provider.LogError(Resources.Log_ErrorWhileImportingComment, e);
                        }
                    }
                }

                if(bmlPost.Trackbacks.Count > 0)
                {
                    foreach(BlogMLTrackback bmlPingTrack in bmlPost.Trackbacks)
                    {
                        try
                        {
                            provider.CreatePostTrackback(bmlPingTrack, newEntryID);
                        }
                        catch(Exception e)
                        {
                            provider.LogError(Resources.Log_ErrorWhileImportingComment, e);
                        }
                    }
                }
            } // End Posts

            provider.ImportComplete();
        }

        private string CreateFilesFromAttachments(BlogMLPost bmlPost, string postContent)
        {
            foreach(BlogMLAttachment bmlAttachment in bmlPost.Attachments)
            {
                string assetDirPath = provider.GetAttachmentDirectoryPath(bmlAttachment);

                string assetDirUrl = provider.GetAttachmentDirectoryUrl(bmlAttachment);

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
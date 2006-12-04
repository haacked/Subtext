#region Copyright (c) 2003, newtelligence AG. All rights reserved.

/*
// Copyright (c) 2003, newtelligence AG. (http://www.newtelligence.com)
// Original BlogX Source Code: Copyright (c) 2003, Chris Anderson (http://simplegeek.com)
// All rights reserved.
//  
// Redistribution and use in source and binary forms, with or without modification, are permitted 
// provided that the following conditions are met: 
//  
// (1) Redistributions of source code must retain the above copyright notice, this list of 
// conditions and the following disclaimer. 
// (2) Redistributions in binary form must reproduce the above copyright notice, this list of 
// conditions and the following disclaimer in the documentation and/or other materials 
// provided with the distribution. 
// (3) Neither the name of the newtelligence AG nor the names of its contributors may be used 
// to endorse or promote products derived from this software without specific prior 
// written permission.
//      
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
// IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// -------------------------------------------------------------------------
//
// Original BlogX source code (c) 2003 by Chris Anderson (http://simplegeek.com)
// 
// newtelligence is a registered trademark of newtelligence Aktiengesellschaft.
// 
// For portions of this software, the some additional copyright notices may apply 
// which can either be found in the license.txt file included in the source distribution
// or following this notice. 
//
*/

#endregion

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

/*Gurkan Yeniceri
This code is heavily relying on DasBlog's MailToWebLog function.
Modificatios made by Gurkan Yeniceri and implemented for SubText.
For the MailToWebLog feature to function properly, here are the changes and things that you need to do.

 **FILE(S) CHANGED**
1-Global.asax.cs
2-BlogInfo.cs
3-Configure.aspx
4-Configure.aspx.cs
5-DataHelper.cs
6-SqlDataProvider.cs
7-web.config
8-Subtext.DotTextUpgrader/Scripts/Installation.01.00.00.sql
9-Subtext.DotTextUpgrader/Scripts/StoredProcedures.sql
10-Subtext.Installation/Scripts/Installation.01.00.00.sql
11-Subtext.Installation/Scripts/StoredProcedures.sql

**FILE(S) ADDED**
1-MailToWebLog.cs
2-Lesnikowski.Pawel.Mail.Pop3.dll (Under External Dependencies folder)

**STORED PROCEDURE(S) CHANGED**
1-subtext_GetBlogById
2-subtext_GetBlogsByHost
3-subtext_GetConfig
4-subtext_GetPageableBlogs
5-subtext_UpdateConfig

**TABLE(S) CHANGED**
1-subtext_Config

**FOLDER(S) SETTINGS CHANGED**
1-"Image" folder needs write access by the aspnet user (also Network Services user if it is on Win2003 Server). On a hosted 
web site, you need to arrange write access through your management console.

**PROBLEM(S)**
1-How can I secure the "Image" folder from browsing?
2-Do not send an e-mail with a subject that is already used as a title for an entry before. -This type of e-mail will not be published
3-What if the image file name in the e-mail is already existing in the Image folder? - Check StoreAttachment function
4-What if the message is containing a zip attachment? - Currently it will not publish that e-mail

**SUGGESTION(S)**
1-Zip attachment feature. publish the e-mail and give a link to zip file.
2-Categories are dropped in this release. User can select categories in square brackets in a semi colon seperated list like [CSharp;DotNet] on the subject of the e-mail

**MANDATORY**
1- AggregateHost in web.config should be set to a correct value and no "http://" in the beginning.
2- AggregateUrl must be set to a correct value to identify the root SubText installation.
3- MailThreadSleep may be changed.

**HOW IT WORKS**
Mail to Weblog runs on a different thread. A simple flow is:
1-Get active blogs
2-Get blog's pop3 parameters
3-Connect to pop3 and process e-mails
4-Continue with the next blog
5-If all blogs are processed then sleep
6-Wake up and continue from step 1

*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Lesnikowski.Client;
using Lesnikowski.Mail;

namespace Subtext.Framework.Services
{

    /// <summary>
    /// This is the handler class for the Mail-To-Weblog functionality.
    /// </summary>
    [SerializableAttribute]
    public class MailToWeblog
    {
        string binariesPath = HttpContext.Current.Server.MapPath("~/Images/"); //give write access for aspnet user (+Network Services user on win2003 server)

        //for test purposes--remove it later
        //string binariesPath = @"C:\Inetpub\wwwroot\Subtext.Web\Images";

        //Uri binariesBaseUri = new Uri(string.Concat(ConfigurationManager.AppSettings.Get("AggregateUrl"), "/Images"));
        //string baseUri = ConfigurationManager.AppSettings.Get("AggregateUrl") + "/Images";
        Uri binariesBaseUri;


        Entry entry = new Entry(PostType.BlogPost);


        /// <summary>
        /// Mail-To-Weblog runs in background thread and this is the thread function.
        /// Check Global.asax for the thread thingy
        /// </summary>
        public void Run()
        {
            binariesBaseUri = new Uri(ConfigurationManager.AppSettings.Get("AggregateUrl") + "/Images");
            //To go through the Blogs in subtext_config table
            //TODO: We may have multiple domains and multiple blogs for each domain.
            //This method won't work if this is the case

            IList<BlogInfo> activeBlogs = BlogInfo.GetBlogs(0, 2, ConfigurationFlag.IsActive);

            //Framework.Components.BlogInfoCollection activeBlogs = new Framework.Components.BlogInfoCollection();
            //string host = ConfigurationManager.AppSettings.Get("AggregateHost"); //it is important to set up the AggregateHost in web.config
            //activeBlogs = Framework.BlogInfo.GetBlogsByHost(host); //get the active blogs !!! IS THERE A BETTER WAY WITHOUT USING AGGREGATEHOST???

            //this is the main loop where all the blogs are checked
            //TODO: Interval to check the blogs will be read from the web.config
            foreach (BlogInfo activeblog in activeBlogs)
            {
                //for the email stripping
                //because of if statements
                //this declarion had to be moved here
                Regex bodyExtractor;

                #region Main POP3 Check Loop

                try
                {

                    //check if necessary properties are set for each blog
                    if (activeblog.pop3MTBEnable &&
                        activeblog.pop3Server != null && activeblog.pop3Server.Length > 0 &&
                        activeblog.pop3User != null && activeblog.pop3User.Length > 0)
                    {
                        //Pop3 pop3 = new Pop3();
                        Pop3 pop3 = new Pop3();

                        //Open connection with the pop3 server
                        pop3.User = activeblog.pop3User;
                        pop3.Password = activeblog.pop3Pass;

                        try
                        {
                            pop3.Connect(activeblog.pop3Server);

                            //if (pop3.HasTimeStamp == true)
                            //    pop3.APOPLogin();
                            //else
                            pop3.Login();

                            pop3.GetAccountStat();

                            //Go thourhg each message on pop3 server
                            for (int j = pop3.MessageCount; j >= 1; j--)
                            {
                                #region for loop for the messages

                                //Pop3Message message = pop3.GetMessage(j);
                                //string message = pop3.GetMessage(j);
                                SimpleMailMessage message;
                                message = SimpleMailMessage.Parse(pop3.GetMessage(j));

                                string messageFrom;
                                // luke@jurasource.co.uk 1-MAR-04
                                // only delete those messages that are processed
                                bool messageWasProcessed = false;

                                // E-Mail addresses look usually like this:
                                // My Name <myname@example.com> or simply
                                // myname@example.com. This block handles 
                                // both variants.
                                Regex getEmail = new Regex(".*\\<(?<email>.*?)\\>.*");
                                Match matchEmail = getEmail.Match(message.From[0].ToString());


                                if (matchEmail.Success)
                                {
                                    messageFrom = matchEmail.Groups["email"].Value;
                                }
                                else
                                {
                                    messageFrom = message.From[0].ToString();
                                }

                                // Only if the subject of the message is prefixed (case-sensitive) with
                                // the configured subject prefix, we accept the message
                                if (message.Subject.StartsWith(activeblog.pop3SubjectPrefix))
                                {
                                    //Prepare the entry 
                                    entry.Title = message.Subject.Substring(activeblog.pop3SubjectPrefix.Length);
                                    entry.BlogId = activeblog.Id;
                                    entry.DisplayOnHomePage = true;
                                    entry.Body = "";
                                    entry.Author = activeblog.Owner;
                                    entry.AllowComments = true;
                                    entry.SyndicateDescriptionOnly = false;
                                    entry.IsAggregated = true;
                                    //entry.EntryName = string.Empty;
                                    entry.IncludeInMainSyndication = true;
                                    entry.IsActive = true;
                                    entry.DateSyndicated = message.Date;
                                    //activeblog.TimeZone.Now;

                                    //TODO: Change this to DateTime.Now if it doesn't work properly
                                    //entry.DateCreated = DateTime.Parse(message.date); //may not be best date
                                    entry.DateCreated = message.Date;

                                    //UNNECESSARY PROPERTIES, maybe we will need them later
                                    //entry.SourceUrl = string.Empty;
                                    //entry.Description = string.Empty;
                                    //entry.TitleUrl = string.Empty;
                                    //entry.SourceName = string.Empty;
                                    //entry.ParentID = NullValue.NullInt32;

                                    // Grab the categories. Categories are defined in square brackets 
                                    // in the subject line.
                                    //TODO: not implemented yet
                                    Regex categoriesRegex = new Regex("(?<exp>\\[(?<cat>.*?)\\])");
                                    foreach (Match match in categoriesRegex.Matches(entry.Title))
                                    {
                                        entry.Title = entry.Title.Replace(match.Groups["exp"].Value, "");
                                        //entry.Categories += match.Groups["cat"].Value+";";
                                    }
                                    entry.Title = entry.Title.Trim();

                                    //TODO: Categories will be added later
                                    //string categories = "";
                                    //                                    string[] splitted = entry.Categories.Split(';');
                                    //                                    for( int i=0;i<splitted.Length;i++)
                                    //                                    {
                                    //                                        categories += splitted[i].Trim()+";";
                                    //                                    }
                                    //entry.Categories = categories.TrimEnd(';');
                                }

                                //entry.DateCreated = RFC2822Date.Parse(message.date);


                                if (message.Mail.Mime.ContentType.ToString() == "text/plain")
                                {
                                    entry.Body += message.Mail.Mime.Body;
                                }

                                // Luke Latimer 16-FEB-2004 (luke@jurasource.co.uk)
                                // HTML only emails were not appearing
                                //else if (message.Mail.Mime.ContentType.TypeName.StartsWith("text/html"))
                                else if (message.Mail.Mime.ContentType.ToString() == "text/html")
                                {
                                    string messageText;


                                    // Note the email may still be encoded
                                    //messageText = QuotedCoding.DecodeOne(message.charset, "Q", message.body);										
                                    messageText = message.Mail.Mime.Body;

                                    /*
                                    * CHANGE: I have changed the e-mail content sripping
                                    * to not to include message disclaimer on the blog entry
                                    */
                                    // Strip the <body> out of the message (using code from below)
									if (String.IsNullOrEmpty(activeblog.pop3StartTag) )
									{
										bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
									}
									else
									{
										bodyExtractor = new Regex(activeblog.pop3StartTag + "(?<content>.*)" + activeblog.pop3EndTag, RegexOptions.IgnoreCase | RegexOptions.Singleline);
									}

                                    Match match = bodyExtractor.Match(messageText);
                                    if (match != null && match.Success && match.Groups["content"] != null)
                                    {
                                        entry.Body += match.Groups["content"].Value;
                                    }
                                    else
                                    {
                                        entry.Body += messageText;
                                    }

                                }
                                // HTML/Text with attachments ?
                                else if (message.Mail.Mime.ContentType.TypeName == ("multipart"))
                                {
                                    Hashtable embeddedFiles = new Hashtable();
                                    ArrayList attachedFiles = new ArrayList();

                                    foreach (MimeData attachment in message.Attachments)
                                    {
                                        // just plain text?
                                        if (attachment.ContentType.ToString() == ("text/plain"))
                                        {
                                            entry.Body += attachment.Body;
                                        }

                                        // Luke Latimer 16-FEB-2004 (luke@jurasource.co.uk)
                                        // Allow for html-only attachments
                                        else if (attachment.ContentType.ToString() == ("text/html"))
                                        {
                                            /*
                                            * CHANGE: I have changed the e-mail content sripping
                                            * to not to include message disclaimer on the blog entry
                                            */
                                            // Strip the <body> out of the message (using code from below)		
                                            if (String.IsNullOrEmpty(activeblog.pop3StartTag))
                                            {
                                                bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                            }
                                            else
                                            {
                                                bodyExtractor = new Regex(activeblog.pop3StartTag + "(?<content>.*)" + activeblog.pop3EndTag, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                            }

                                            string htmlString = attachment.Body;
                                            Match match = bodyExtractor.Match(htmlString);

                                            // NOTE: We will BLOW AWAY any previous content in this case.
                                            // This is because most mail clients like Outlook include
                                            // plain, then HTML. We will grab plain, then blow it away if 
                                            // HTML is included later.
                                            if (match != null && match.Success && match.Groups["content"] != null)
                                            {
                                                entry.Body = match.Groups["content"].Value;
                                            }
                                            else
                                            {
                                                entry.Body = htmlString;
                                            }
                                        }

                                        // or alternative text ?
                                        #region Unused at the moment

                                        else if (attachment.ContentType.ToString() == ("multipart/alternative"))
                                        {
                                            bool contentSet = false;
                                            string textContent = null;
                                            foreach (MimeData inner_attachment in message.Attachments)
                                            {
                                                // we prefer HTML
                                                if (inner_attachment.ContentType.ToString() == ("text/plain"))
                                                {
                                                    //textContent = StringOperations.GetString(inner_attachment.Data);
                                                }
                                                else if (inner_attachment.ContentType.ToString() == ("text/html"))
                                                {
                                                    /*
                                                        * CHANGE: I have changed the e-mail content sripping
                                                        * to not to include message disclaimer on the blog entry
                                                        */
                                                    if (activeblog.pop3StartTag == string.Empty)
                                                        bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                                    else
                                                        bodyExtractor = new Regex(activeblog.pop3StartTag + "(?<content>.*)" + activeblog.pop3EndTag, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                                                    //string htmlString = StringOperations.GetString(inner_attachment.Data);
                                                    //Match match = bodyExtractor.Match(htmlString);
                                                    //if (match != null && match.Success && match.Groups["content"] != null)
                                                    //{
                                                    //    entry.Body += match.Groups["content"].Value;
                                                    //}
                                                    //else
                                                    //{
                                                    //    entry.Body += htmlString;
                                                    //}
                                                    contentSet = true;
                                                }
                                            }
                                            if (!contentSet)
                                            {
                                                entry.Body += textContent;
                                            }
                                        }


                                             //or text with embeddedFiles (in a mixed message only)
                                        else if ((message.Mail.Mime.ContentType.ToString() == ("multipart/mixed") || message.Mail.Mime.ContentType.ToString() == ("multipart/alternative"))
                                       && attachment.ContentType.ToString() == ("multipart/related"))
                                        {
                                            foreach (MimeData inner_attachment in message.Attachments)
                                            {
                                                // just plain text?
                                                if (inner_attachment.ContentType.ToString() == ("text/plain"))
                                                {
                                                    //entry.Body += StringOperations.GetString(inner_attachment.Data);
                                                }

                                                else if (inner_attachment.ContentType.ToString() == ("text/html"))
                                                {
                                                    /*
                                                        * CHANGE: I have changed the e-mail content sripping
                                                        * to not to include message disclaimer on the blog entry
                                                        */
                                                    if (activeblog.pop3StartTag == string.Empty)
                                                        bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                                    else
                                                        bodyExtractor = new Regex(activeblog.pop3StartTag + "(?<content>.*)" + activeblog.pop3EndTag, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                                                    //string htmlString = StringOperations.GetString(inner_attachment.Data);
                                                    //Match match = bodyExtractor.Match(htmlString);
                                                    //if (match != null && match.Success && match.Groups["content"] != null)
                                                    //{
                                                    //    entry.Body += match.Groups["content"].Value;
                                                    //}
                                                    //else
                                                    //{
                                                    //    entry.Body += htmlString;
                                                    //}
                                                }

                                                    // or alternative text ?
                                                else if (inner_attachment.ContentType.ToString() == ("multipart/alternative"))
                                                {
                                                    bool contentSet = false;
                                                    string textContent = null;
                                                    //foreach (MimeData inner_inner_attachment in inner_attachment.Attachments)
                                                    //{
                                                    //    // we prefer HTML
                                                    //    if (inner_inner_attachment.ContentType.StartsWith("text/plain"))
                                                    //    {
                                                    //        textContent = StringOperations.GetString(inner_inner_attachment.Data);
                                                    //    }
                                                    //    else if (inner_inner_attachment.ContentType.StartsWith("text/html"))
                                                    //    {
                                                    //        /*
                                                    //            * CHANGE: I have changed the e-mail content sripping
                                                    //            * to not to include message disclaimer on the blog entry
                                                    //            */
                                                    //        if (activeblog.pop3StartTag == string.Empty)
                                                    //            bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                                    //        else
                                                    //            bodyExtractor = new Regex(activeblog.pop3StartTag + "(?<content>.*)" + activeblog.pop3EndTag, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                                                    //        string htmlString = StringOperations.GetString(inner_inner_attachment.Data);
                                                    //        Match match = bodyExtractor.Match(htmlString);
                                                    //        if (match != null && match.Success && match.Groups["content"] != null)
                                                    //        {
                                                    //            entry.Body += match.Groups["content"].Value;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            entry.Body += htmlString;
                                                    //        }
                                                    //        contentSet = true;
                                                    //    }
                                                    //}
                                                    if (!contentSet)
                                                    {
                                                        entry.Body += textContent;
                                                    }
                                                }
                                                // any other inner_attachment
                                                else if (inner_attachment.Data != null &&
                                                    inner_attachment.FileName != null &&
                                                    inner_attachment.FileName.Length > 0)
                                                {
                                                    if (inner_attachment.ContentId.Length > 0)
                                                    {
                                                        embeddedFiles.Add(inner_attachment.ContentId, StoreAttachment(inner_attachment));
                                                    }
                                                    else
                                                    {
                                                        attachedFiles.Add(StoreAttachment(inner_attachment));
                                                    }
                                                }
                                            }
                                        }
                                        #endregion Unused at the moment
                                        // any other attachment
                                        else if (attachment.Data != null &&
                                            attachment.FileName != null &&
                                            attachment.FileName.Length > 0)
                                        {
                                            if (attachment.ContentId.Length > 0 && message.Mail.Mime.ContentType.TypeName.StartsWith("multipart/related"))
                                            {
                                                embeddedFiles.Add(attachment.ContentId, StoreAttachment(attachment));
                                            }
                                            else
                                            {
                                                attachedFiles.Add(StoreAttachment(attachment));
                                            }

                                        }
                                    }

                                    // check for orphaned embeddings
                                    string[] embeddedKeys = new string[embeddedFiles.Keys.Count];
                                    embeddedFiles.Keys.CopyTo(embeddedKeys, 0);
                                    foreach (string key in embeddedKeys)
                                    {
                                        if (entry.Body.IndexOf("cid:" + key.Trim('<', '>')) == -1)
                                        {
                                            object file = embeddedFiles[key];
                                            embeddedFiles.Remove(key);
                                            attachedFiles.Add(file);
                                        }
                                    }

                                    // now fix up the URIs

                                    if (activeblog.pop3InlineAttachedPictures)
                                    {
                                        foreach (string fileName in attachedFiles)
                                        {
                                            string fileNameU = fileName.ToUpper();
                                            if (fileNameU.EndsWith(".JPG") || fileNameU.EndsWith(".JPEG") ||
                                                fileNameU.EndsWith(".GIF") || fileNameU.EndsWith(".PNG") ||
                                                fileNameU.EndsWith(".BMP"))
                                            {
                                                bool scalingSucceeded = false;

                                                if (activeblog.pop3HeightForThumbs > 0)
                                                {
                                                    try
                                                    {
                                                        string absoluteFileName = Path.Combine(binariesPath, fileName);
                                                        string thumbBaseFileName = Path.GetFileNameWithoutExtension(fileName) + "-thumb.subtext.JPG";
                                                        string thumbFileName = Path.Combine(binariesPath, thumbBaseFileName);
                                                        Bitmap sourceBmp = new Bitmap(absoluteFileName);
                                                        if (sourceBmp.Height > activeblog.pop3HeightForThumbs)
                                                        {
                                                            Bitmap targetBmp = new Bitmap(sourceBmp, new Size(
                                                                Convert.ToInt32(Math.Round((sourceBmp.Width * (((double)activeblog.pop3HeightForThumbs) / ((double)sourceBmp.Height))), 0)),
                                                                activeblog.pop3HeightForThumbs));

                                                            ImageCodecInfo codecInfo = GetEncoderInfo("image/jpeg");
                                                            Encoder encoder = Encoder.Quality;
                                                            EncoderParameters encoderParams = new EncoderParameters(1);
                                                            long compression = 75;
                                                            EncoderParameter encoderParam = new EncoderParameter(encoder, compression);
                                                            encoderParams.Param[0] = encoderParam;
                                                            targetBmp.Save(thumbFileName, codecInfo, encoderParams);

                                                            string absoluteUri = new Uri(binariesBaseUri, fileName).AbsoluteUri;
                                                            string absoluteThumbUri = new Uri(binariesBaseUri, thumbBaseFileName).AbsoluteUri;
                                                            entry.Body += String.Format("<div class=\"inlinedMailPictureBox\"><a href=\"{0}\"><img border=\"0\" class=\"inlinedMailPicture\" src=\"{2}\"></a><br><a class=\"inlinedMailPictureLink\" href=\"{0}\">{1}</a></div>", absoluteUri, fileName, absoluteThumbUri);
                                                            scalingSucceeded = true;

                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (!scalingSucceeded)
                                                {
                                                    string absoluteUri = new Uri(binariesBaseUri, fileName).AbsoluteUri;
                                                    entry.Body += String.Format("<div class=\"inlinedMailPictureBox\"><img class=\"inlinedMailPicture\" src=\"{0}\"><br><a class=\"inlinedMailPictureLink\" href=\"{0}\">{1}</a></div>", absoluteUri, fileName);
                                                }
                                            }
                                        }
                                    }

                                    if (attachedFiles.Count > 0)
                                    {
                                        entry.Body += "<p>";
                                    }

                                    foreach (string fileName in attachedFiles)
                                    {
                                        string fileNameU = fileName.ToUpper();
                                        if (!activeblog.pop3InlineAttachedPictures ||
                                            (!fileNameU.EndsWith(".JPG") && !fileNameU.EndsWith(".JPEG") &&
                                                !fileNameU.EndsWith(".GIF") && !fileNameU.EndsWith(".PNG") &&
                                                !fileNameU.EndsWith(".BMP")))
                                        {
                                            string absoluteUri = new Uri(binariesBaseUri, fileName).AbsoluteUri;
                                            entry.Body += String.Format("Download: <a href=\"{0}\">{1}</a><br>", absoluteUri, fileName);
                                        }
                                    }
                                    if (attachedFiles.Count > 0)
                                    {
                                        entry.Body += "</p>";
                                    }

                                    foreach (string key in embeddedFiles.Keys)
                                    {
                                        //This was working a minute ago, but broken for some unknown reason
                                        //the "images" directory is missing at the end of the img src link. It is there actually?????? Puzzled
                                        //entry.Body = entry.Body.Replace("cid:"+key.Trim('<','>'), new Uri( binariesBaseUri, (string)embeddedFiles[key] ).AbsoluteUri );

                                        entry.Body = entry.Body.Replace("cid:" + key.Trim('<', '>'), binariesBaseUri.AbsoluteUri + "/" + embeddedFiles[key]);

                                    }
                                }

                                //everything is good, create the entry
                                if (entry.Body != "")
                                    entry.Id = DatabaseObjectProvider.Instance().InsertEntry(entry); //If blogid is null, we should not create an entry.

                                //Raise event before creating a post
                                //SubtextEvents.OnEntryUpdating(entry, new SubtextEventArgs(ObjectState.Create));

                                //int PostId = Entries.Create(entry);

                                //Raise event after creating a post
                                //SubtextEvents.OnEntryUpdated(entry, new SubtextEventArgs(ObjectState.Create));

                                //TODO: Refactor CommunityCredits to be a plugin
                                //AddCommunityCredits(entry);

                                if (entry.Id > 0)
                                    messageWasProcessed = true;
                                else
                                    messageWasProcessed = false;

                                // luke@jurasource.co.uk (01-MAR-04)
                                if (activeblog.pop3DeleteOnlyProcessed || messageWasProcessed)
                                {
                                    pop3.DeleteMessage(j);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        finally
                        {
                            pop3.Close();
                        }
                    }

                                #endregion for loop for mails

                    //either active blog is not setup for mail to weblog functionality or 
                    //all the emails are processed for this blog; in both cases
                    //continue with the next blog
                    continue;
                }

                catch
                {
                    throw;
                }
                //TODO:Logging can be done here
                #endregion Main Mail check loop
            }
            //if all the blogs and e-mails are processed thread may sleep here
            //Sleep time comes from web.config
            Thread.Sleep(TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings.Get("MTWThreadSleep"))));
        }

        /// <summary>
        /// Compares two binary buffers up to a certain length.
        /// </summary>
        /// <param name="buf1">First buffer</param>
        /// <param name="buf2">Second buffer</param>
        /// <param name="len">Length</param>
        /// <returns>true or false indicator about the equality of the buffers</returns>
        private bool CompareBuffers(byte[] buf1, byte[] buf2, int len)
        {
            if (buf1.Length >= len && buf2.Length >= len)
            {
                for (int l = 0; l < len; l++)
                {
                    if (buf1[l] != buf2[l])
                        return false;
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Stores an attachment to disk.
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public string StoreAttachment(MimeData attachment)
        {
            bool alreadyUploaded = false;
            string baseFileName = attachment.FileName;
            string targetFileName = Path.Combine(binariesPath, baseFileName);
            int numSuffix = 1;

            // if the target filename already exists, we check whether we already 
            // have that file stored by comparing the first 2048 bytes of the incoming
            // date to the target file (creating a hash would be better, but this is 
            // "good enough" for the time being)
            while (File.Exists(targetFileName))
            {
                byte[] targetBuffer = new byte[Math.Min(2048, attachment.Data.Length)];
                int targetBytesRead;

                using (FileStream targetFile = new FileStream(targetFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long numBytes = targetFile.Length;
                    if (numBytes == attachment.Data.Length)
                    {
                        targetBytesRead = targetFile.Read(targetBuffer, 0, targetBuffer.Length);
                        if (targetBytesRead == targetBuffer.Length)
                        {
                            if (CompareBuffers(targetBuffer, attachment.Data, targetBuffer.Length))
                            {
                                alreadyUploaded = true;
                            }
                        }
                    }
                }

                // If the file names are equal, but it's not considered the same file,
                // we append an incrementing numeric suffix to the file name and retry.
                if (!alreadyUploaded)
                {
                    string ext = Path.GetExtension(baseFileName);
                    string file = Path.GetFileNameWithoutExtension(baseFileName);
                    string newFileName = file + (numSuffix++).ToString();
                    baseFileName = newFileName + ext;
                    targetFileName = Path.Combine(binariesPath, baseFileName);
                }
                else
                {
                    break;
                }
            }

            // now we've got a unique file name or the file is already stored. If it's
            // not stored, write it to disk.
            if (!alreadyUploaded)
            {
                using (FileStream fileStream = new FileStream(targetFileName, FileMode.Create))
                {
                    fileStream.Write(attachment.Data, 0, attachment.Data.Length);
                    fileStream.Flush();
                }
            }
            return baseFileName;
        }


        /// <summary>
        /// This function is used for thumbnailing and gets an image encoder
        /// for a given mime type, such as image/jpeg
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }

        //TODO: Refactor CommunityCredits to be a plugin
        //private string AddCommunityCredits(Entry entry)
        //{
        //    string result = string.Empty;

        //    bool commCreditsEnabled;
        //    try
        //    {
        //        commCreditsEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CommCreditEnabled"]);
        //    }
        //    catch (Exception)
        //    {
        //        commCreditsEnabled = false;
        //    }

        //    if (commCreditsEnabled.Equals("true"))
        //    {
        //        com.community_credit.www.AffiliateServices wsCommunityCredit = new com.community_credit.www.AffiliateServices();
        //        string url = entry.FullyQualifiedUrl.ToString();
        //        string category = String.Empty;
        //        if (entry.PostType == PostType.BlogPost)
        //            category = "Blog";
        //        else if (entry.PostType == PostType.Story)
        //            category = "Article";
        //        string description = "Blogged about: " + entry.Title;
        //        BlogInfo info = Config.CurrentBlog;
        //        string firstName = string.Empty;
        //        string lastName = info.Author;
        //        string email = info.Email;
        //        string affiliateCode = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateCode"];
        //        string affiliateKey = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateKey"];

        //        try
        //        {
        //            result = wsCommunityCredit.AddCommunityCredit(email, firstName, lastName, description, url, category, affiliateCode, affiliateKey);
        //        }
        //        catch (Exception ex)
        //        {
        //            this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "Error during Community Credits submission (your post has been saved)", ex.Message));
        //        }
        //    }
        //    return result;
        //}

    }
}

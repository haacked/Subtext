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
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.Controls;
using Subtext.Web.Controls.Captcha;
using Subtext.Web.UI.ViewModels;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Summary description for BaseControl.
    /// </summary>
    public class BaseControl : UserControl, ISkinControlContainer
    {
        AdminUrlHelper _adminUrlHelper;

        public RouteValueDictionary RouteValues
        {
            get { return SubtextContext.RequestContext.RouteData.Values; }
        }

        public BlogUrlHelper Url
        {
            get
            {
                if (SubtextDependencies != null)
                {
                    return SubtextDependencies.Url;
                }
                return null;
            }
        }

        public AdminUrlHelper AdminUrl
        {
            get
            {
                if (_adminUrlHelper == null)
                {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }

        public Blog Blog
        {
            get
            {
                return SubtextDependencies.Blog;
            }
        }

        public ISearchEngineService SearchEngineService
        {
            get
            {
                return (Page as SubtextPage).SearchEngineService;
            }
        }

        protected ISubtextDependencies SubtextDependencies
        {
            get
            {
                var dependencies = Page as ISubtextDependencies;
                if (dependencies == null)
                {
                    dependencies = DependencyResolver.Current.GetService<SubtextDependencies>();
                }
                return dependencies;
            }
        }

        protected ISubtextContext SubtextContext
        {
            get { return SubtextDependencies.SubtextContext; }
        }

        protected ObjectRepository Repository
        {
            get { return SubtextContext.Repository; }
        }

        protected IPrincipal User
        {
            get { return SubtextContext.User; }
        }

        protected virtual string ControlCacheKey
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", GetType(), Blog.Id); }
        }

        public string SkinFilePath { get; set; }

        protected static string Format(string format, params object[] arguments)
        {
            return String.Format(format, arguments);
        }

        /// <summary>
        /// Url encodes the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        protected static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        /// <summary>
        /// Url encodes the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        protected static string UrlEncode(Uri s)
        {
            return HttpUtility.UrlEncode(s.ToString());
        }

        /// <summary>
        /// Url encodes the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        protected static string UrlEncode(object s)
        {
            return HttpUtility.UrlEncode(s.ToString());
        }

        /// <summary>
        /// Url decodes the string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        protected string H(string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        protected string H(object value)
        {
            return HttpUtility.HtmlEncode(value.ToString());
        }

        protected static string UrlDecode(object s)
        {
            return HttpUtility.UrlDecode(s.ToString());
        }

        protected void BindCurrentEntryControls(Entry entry, Control root)
        {
            foreach (Control control in root.Controls)
            {
                var currentEntryControl = control as CurrentEntryControl;
                if (currentEntryControl != null)
                {
                    currentEntryControl.Entry = new EntryViewModel(entry, SubtextContext);
                    currentEntryControl.DataBind();
                }
            }
        }

        /// <summary>
        /// Adds the captcha if necessary.
        /// </summary>
        /// <param name="captcha">The captcha.</param>
        /// <param name="invisibleCaptchaValidator">The invisible captcha validator.</param>
        /// <param name="btnIndex">Index of the BTN.</param>
        protected void AddCaptchaIfNecessary(ref CaptchaControl captcha, ref InvisibleCaptcha invisibleCaptchaValidator,
                                             int btnIndex)
        {
            if (Config.CurrentBlog.CaptchaEnabled)
            {
                captcha = new CaptchaControl { ID = "captcha" };
                Control preExisting = ControlHelper.FindControlRecursively(this, "captcha");
                if (preExisting == null)
                // && !Config.CurrentBlog.FeedbackSpamServiceEnabled) Experimental code for improved UI. Will put back in later. - Phil Haack 10/09/2006
                {
                    Controls.AddAt(btnIndex, captcha);
                }
            }
            else
            {
                RemoveCaptcha();
            }

            if (Config.Settings.InvisibleCaptchaEnabled)
            {
                invisibleCaptchaValidator = new InvisibleCaptcha
                {
                    ErrorMessage = "Please enter the answer to the supplied question."
                };

                Controls.AddAt(btnIndex, invisibleCaptchaValidator);
            }
        }

        /// <summary>
        /// Removes the captcha if necessary.
        /// </summary>
        protected void RemoveCaptcha()
        {
            Control preExisting = ControlHelper.FindControlRecursively(this, "captcha");
            if (preExisting != null)
            {
                Controls.Remove(preExisting);
            }
        }

        public ISkinControlLoader SkinControlLoader
        {
            get;
            set;
        }
    }
}
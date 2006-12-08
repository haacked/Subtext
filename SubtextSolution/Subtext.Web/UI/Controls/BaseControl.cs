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
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Web.Controls;
using Subtext.Web.Controls.Captcha;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for BaseControl.
	/// </summary>
	public class BaseControl : System.Web.UI.UserControl
	{
		public BaseControl()
		{
			
		}

		private BlogInfo _config;
		protected BlogInfo CurrentBlog
		{
			get
			{
				return _config;
			}
		}

		protected virtual string ControlCacheKey
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}",this.GetType(),CurrentBlog.Id);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			_config = Config.CurrentBlog;
			base.OnInit (e);
		}

		private string skinFilePath;
		public string SkinFilePath
		{
			get
			{return skinFilePath;}
			set{skinFilePath = value;}
		}
		
		protected static void BindCurrentEntryControls(Entry entry, Control root)
		{
			foreach(Control control in root.Controls)
			{
				CurrentEntryControl currentEntryControl = control as CurrentEntryControl;
				if(currentEntryControl != null)
				{
					currentEntryControl.Entry = entry;
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
		protected void AddCaptchaIfNecessary(ref CaptchaControl captcha, ref InvisibleCaptcha invisibleCaptchaValidator, int btnIndex)
		{				
			if (Config.CurrentBlog.CaptchaEnabled)
			{
				captcha = new CaptchaControl();
				captcha.ID = "captcha";
				Control preExisting = ControlHelper.FindControlRecursively(this, "captcha");
				if (preExisting == null) // && !Config.CurrentBlog.FeedbackSpamServiceEnabled) Experimental code for improved UI. Will put back in later. - Phil Haack 10/09/2006
				{
					Controls.AddAt(btnIndex, captcha);
				}
			}
			else
			{
				RemoveCaptcha();
			}

            if (BlogConfigurationSettings.InvisibleCaptchaEnabled)
			{
				invisibleCaptchaValidator = new InvisibleCaptcha();
				invisibleCaptchaValidator.ErrorMessage = "Please enter the answer to the supplied question.";

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
	}
}


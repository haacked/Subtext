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


//		protected override void OnLoad(EventArgs e)
//		{
//			_config = Config.CurrentBlog;
//			base.OnLoad(e);
//		}

		private string skinFilePath;
		public string SkinFilePath
		{
			get
			{return skinFilePath;}
			set{skinFilePath = value;}
		}
		
		protected void BindCurrentEntryControls(Entry entry, Control root)
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
				
				/*
				* Experimental code for improved UI. Will put back in later.
				*		- Phil Haack Removed 10/09/2006
				if(Config.CurrentBlog.FeedbackSpamServiceEnabled)
				{
					// Set up this button just in case we need to show it.
					if (btnConfirm == null)
						btnConfirm = new Button();
					btnConfirm.ID = "btnConfirm";
					btnConfirm.Text = "Confirm";
					btnConfirm.Visible = true;
					Controls.AddAt(btnIndex, btnConfirm);
					btnConfirm.Click += new EventHandler(btnConfirm_Click);
				}*/
			}

			invisibleCaptchaValidator = new InvisibleCaptcha();
			invisibleCaptchaValidator.ErrorMessage = "Please enter the answer to the supplied question.";

			Controls.AddAt(btnIndex, invisibleCaptchaValidator);
		}
	}
}


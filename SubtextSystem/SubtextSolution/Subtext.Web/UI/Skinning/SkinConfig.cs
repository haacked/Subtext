#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml.Serialization;
using Subtext.Framework;

namespace Subtext.Web.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinConfig.
	/// </summary>
	[Serializable]
	public class SkinConfig
	{

		public SkinConfig()
		{
		}

		private SkinItem[] common;
		public SkinItem[] Common
		{
			get{return common;}
			set{common = value;}
		}

		private SkinPage[] pages;
		[XmlArray("Pages")]
		public SkinPage[] Pages
		{
			get{return pages;}
			set{pages = value;}
		}

		private Hashtable blogpages;

		public SkinPage GetPage(string name)
		{
			if(blogpages == null)
			{
				LoadPages();
			}

			if(blogpages.Contains(name.ToLower()))
			{
				return (SkinPage)blogpages[name.ToLower()];
			}
			else
			{
				return null;
			}
		}

		private void LoadPages()
		{
			blogpages = new Hashtable();
			if(Pages != null)
			{
				int count = Pages.Length;
				for(int i = 0; i<count;i++)
				{
					blogpages.Add(Pages[i].Name.ToLower(),Pages[i]);
				}
			}
		}

		public static string SkinPath(string skin)
		{
			
			return SkinPath(skin,HttpContext.Current);
		}

		public static string SkinPath(string skin, HttpContext context)
		{
			try
			{
				return Path.Combine(context.Request.ApplicationPath, "Skins/" + SkinTemplates.Instance(context).GetTemplate(skin).Skin + "/");
			}
			catch
			{
				throw new BlogSkinException(string.Format("The Skin {0} could not be found in the SkinTemplates file",skin));
			}
		}


		public static string SkinControlPath(string skin, string control)
		{
			return Path.Combine(SkinPath(skin), "Controls/" + control);
		}

		public static string ControlString(Control control)
		{
			StringWriter sw = new StringWriter();
			HtmlTextWriter htw = new HtmlTextWriter(sw);
			control.RenderControl(htw);
			return sw.ToString();		

		}

	}


}


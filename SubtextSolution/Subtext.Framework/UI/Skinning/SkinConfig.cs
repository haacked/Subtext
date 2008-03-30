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
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml.Serialization;
using Subtext.Framework;

namespace Subtext.Framework.UI.Skinning
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

			if(blogpages.Contains(name.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
			{
				return (SkinPage)blogpages[name.ToLower(System.Globalization.CultureInfo.InvariantCulture)];
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
					blogpages.Add(Pages[i].Name.ToLower(System.Globalization.CultureInfo.InvariantCulture),Pages[i]);
				}
			}
		}

		public static string SkinPath(string skin)
		{
			try
			{
				return Path.Combine(HttpContext.Current.Request.ApplicationPath, "Skins/" + SkinTemplates.Instance().GetTemplate(skin).Skin + "/");
			}
			catch
			{
				throw new BlogSkinException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "The Skin {0} could not be found in the SkinTemplates file",skin));
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


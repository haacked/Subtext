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
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinTemplates.
	/// </summary>
	[Serializable]
	public class SkinTemplates
	{
		public static SkinTemplates Instance()
		{
			return Instance(HttpContext.Current);
		}
		public static SkinTemplates Instance(HttpContext context)
		{
			SkinTemplates st = (SkinTemplates)context.Cache["SkinTemplates"];
			if(st == null)
			{
				string filename = context.Request.MapPath("~/Admin/Skins.config");
				st = (SkinTemplates)SerializationHelper.Load(typeof(SkinTemplates),filename);
				if(st != null)
				{
					context.Cache.Insert("SkinTemplates",st,new CacheDependency(filename));
				}
			}
			return st;
		}

		public SkinTemplates()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private Hashtable _ht;
		public SkinTemplate GetTemplate(string id)
		{
			if(_ht == null)
			{
				_ht = new Hashtable();
				for(int i = 0; i<Templates.Length; i++)
				{
					_ht.Add(Templates[i].SkinID.ToLower(System.Globalization.CultureInfo.InvariantCulture),Templates[i]);
				}
			}

			if(_ht.Contains(id.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
			{
				return (SkinTemplate)_ht[id.ToLower(System.Globalization.CultureInfo.InvariantCulture)];
			}
			return null;

		}

		private SkinTemplate[] _skinTemplates;
		[XmlArray("Skins")]
		public SkinTemplate[] Templates
		{
			get {return this._skinTemplates;}
			set {this._skinTemplates = value;}
		}
	}
}


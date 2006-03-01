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
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using Subtext.Framework.Util;

namespace Subtext.Framework.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinTemplates.
	/// </summary>
	[Serializable]
	public class SkinTemplates
	{
		public static SkinTemplates Instance()
		{
			SkinTemplates st = (SkinTemplates)HttpContext.Current.Cache["SkinTemplates"];
			if(st == null)
			{
				string filename = HttpContext.Current.Request.MapPath("~/Admin/Skins.config");
				st = (SkinTemplates)SerializationHelper.Load(typeof(SkinTemplates),filename);
				if(st != null)
				{
					HttpContext.Current.Cache.Insert("SkinTemplates",st,new CacheDependency(filename));
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


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
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
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
                VirtualPathProvider vpathProvider = HostingEnvironment.VirtualPathProvider;
                VirtualFile virtualConfigFile = vpathProvider.GetFile("~/Admin/Skins.config");
                using (Stream configStream = virtualConfigFile.Open())
                {
                    st = (SkinTemplates)SerializationHelper.Load(typeof(SkinTemplates), configStream);
                }
				if(st != null)
				{
					HttpContext.Current.Cache.Insert("SkinTemplates", st, vpathProvider.GetCacheDependency("~/Admin/Skins.config", null, DateTime.Now.ToUniversalTime()));
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

		private Dictionary<string, SkinTemplate> _ht;

		/// <summary>
		/// Gets the template based on the skin id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public SkinTemplate GetTemplate(string id)
		{
			if(_ht == null)
			{
				_ht = new Dictionary<string, SkinTemplate>();
				for(int i = 0; i < Templates.Length; i++)
				{
					_ht.Add(Templates[i].SkinKey, Templates[i]);
				}
			}

			if(_ht.ContainsKey(id.ToUpper(System.Globalization.CultureInfo.InvariantCulture)))
			{
				return _ht[id.ToUpper(System.Globalization.CultureInfo.InvariantCulture)];
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


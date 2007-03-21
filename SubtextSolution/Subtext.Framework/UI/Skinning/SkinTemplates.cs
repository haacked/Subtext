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
	/// Represents skin templates configured within the Skins.config file.
	/// </summary>
	[Serializable]
	public class SkinTemplates
	{
		/// <summary>
		/// Instantiates an instance of <see cref="SkinTemplates" /> from the Admin/Skins.config file 
		/// as well as the Admin/Skins.User.config file.
		/// </summary>
		/// <returns></returns>
		public static SkinTemplates Instance()
		{
			SkinTemplates skinTemplates = (SkinTemplates)HttpContext.Current.Cache["SkinTemplates"];
			if (skinTemplates == null)
			{
				skinTemplates = Instance(HostingEnvironment.VirtualPathProvider);

				if (skinTemplates != null)
				{
					HttpContext.Current.Cache.Insert("SkinTemplates", skinTemplates, HostingEnvironment.VirtualPathProvider.GetCacheDependency("~/Admin/Skins.config", null, DateTime.Now.ToUniversalTime()));
				}
			}
			return skinTemplates;
		}

		/// <summary>
		/// Instantiates an instance of <see cref="SkinTemplates" /> from the Admin/Skins.config file 
		/// as well as the Admin/Skins.User.config file using the specified <see cref="VirtualPathProvider" />
		/// </summary>
		/// <param name="vpathProvider"></param>
		/// <returns></returns>
		public static SkinTemplates Instance(VirtualPathProvider vpathProvider)
		{
			SkinTemplates skinTemplates = GetSkinTemplates(vpathProvider, "~/Admin/Skins.config");

			if (vpathProvider.FileExists("~/Admin/Skins.User.config"))
			{
				SkinTemplates userSpecificTemplates = GetSkinTemplates(vpathProvider, "~/Admin/Skins.User.config");
				if (userSpecificTemplates != null)
				{
					foreach (SkinTemplate template in userSpecificTemplates.Templates)
					{
						skinTemplates.Templates.Add(template);
					}
				}
			}
			return skinTemplates;
		}

		private static SkinTemplates GetSkinTemplates(VirtualPathProvider virtualPathProvider, string path)
		{
			VirtualFile virtualConfigFile = virtualPathProvider.GetFile(path);

			using (Stream configStream = virtualConfigFile.Open())
			{
				return SerializationHelper.Load<SkinTemplates>(configStream);
			}
		}

		public SkinTemplates()
		{
		}

		private Dictionary<string, SkinTemplate> _ht;

		/// <summary>
		/// Gets the template based on the skin id.
		/// </summary>
		/// <param name="skinKey">The id.</param>
		/// <returns></returns>
		public SkinTemplate GetTemplate(string skinKey)
		{
			if (_ht == null)
			{
				_ht = new Dictionary<string, SkinTemplate>();
				for (int i = 0; i < Templates.Count; i++)
				{
					_ht.Add(Templates[i].SkinKey, Templates[i]);
				}
			}

			if (_ht.ContainsKey(skinKey.ToUpper(System.Globalization.CultureInfo.InvariantCulture)))
			{
				return _ht[skinKey.ToUpper(System.Globalization.CultureInfo.InvariantCulture)];
			}
			return null;

		}

		[XmlArray("Skins")]
		public List<SkinTemplate> Templates
		{
			get { return this._skinTemplates; }
			set { this._skinTemplates = value; }
		}

		private List<SkinTemplate> _skinTemplates;
	}
}


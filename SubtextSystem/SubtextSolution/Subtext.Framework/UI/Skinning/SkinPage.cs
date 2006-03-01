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
using System.Xml.Serialization;

namespace Subtext.Framework.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinPage.
	/// </summary>
	[Serializable]
	public class SkinPage
	{

		[XmlAttribute]
		public string Name;

		private SkinItem[] controls;
		public SkinItem[] Controls
		{
			get{return controls;}
			set{controls = value;}
		}

	}
}


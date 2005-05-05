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
using System.Xml.Serialization;

namespace Subtext.Web.UI.Skinning
{
	/// <summary>
	/// Summary description for SkinTemplate.
	/// </summary>
	[Serializable]
	public class SkinTemplate
	{
		public SkinTemplate()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string _skin;
		[XmlAttribute]
		public string Skin
		{
			get {return this._skin;}
			set {this._skin = value;}
		}

		private string _secondaryCss;
		[XmlAttribute]
		public string SecondaryCss
		{
			get {return this._secondaryCss;}
			set {this._secondaryCss = value;}
		}

		public bool UseSecondaryCss
		{
			get{return (this.SecondaryCss != null && this.SecondaryCss.Trim().Length > 0);}
		}

		private string _skinID;
		[XmlAttribute]
		public string SkinID
		{
			get {return this._skinID;}
			set {this._skinID = value;}
		}

		
	}
}


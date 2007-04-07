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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for KeyWord.
	/// </summary>
	[Serializable]
	public class KeyWord
	{
		private string _word;
		/// <summary>
		/// The word or words to look for. Could also be short hand. ie, ScottW would end up <a href = "http://scottwater.com/blog">Scott Watermasysk</a>
		/// </summary>
		public string Word
		{
			get {return this._word;}
			set {this._word = value;}
		}

		private string _text;
		/// <summary>
		/// The text value of an anchor tag: <a href="#">TEXT</a>
		/// </summary>
		public string Text
		{
			get {return this._text;}
			set {this._text = value;}
		}

		private bool _replaceFirstTimeOnly;
		public bool ReplaceFirstTimeOnly
		{
			get {return this._replaceFirstTimeOnly;}
			set {this._replaceFirstTimeOnly = value;}
		}

		private bool _caseSensitive;
		public bool CaseSensitive
		{
			get {return this._caseSensitive;}
			set {this._caseSensitive = value;}
		}

		private bool _openInNewWindow = true;
		public bool OpenInNewWindow
		{
			get {return this._openInNewWindow;}
			set {this._openInNewWindow = value;}
		}

		private string _url;
		public string Url
		{
			get {return this._url;}
			set {this._url = value;}
		}

		private string _title;
		public string Title
		{
			get {return this._title;}
			set {this._title = value;}
		}

		private string _rel;
		/// <summary>
		/// Rel entries for a keyword. For instance, you could add "Friend" to a keyword, and XFN would pick it up as a Friend link
		/// </summary>
		public string Rel
		{
			get { return this._rel; }
			set { this._rel = value; }
		}

		private int _blogID;
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		[XmlAttribute]
		public int Id
		{
			get {return this.id;}
			set {this.id = value;}
		}
		private int id;

		private string _format;
		public string GetFormat
		{
			get
			{
				if(_format == null)
				{
					ProcessFormat();
				}
				return _format;
			}
		}

		private void ProcessFormat()
		{
			_format = string.Format(System.Globalization.CultureInfo.InvariantCulture, "<a {0} href=\"{1}\" {2} {3}>{4}</a>", this.Title != null ? "title=\"" + Title + "\"" : String.Empty, Url, this.Rel != null ? "rel=\"" + Rel + "\"" : string.Empty, this.OpenInNewWindow ? "target=\"_blank\"" : string.Empty, Text);
		}
	}
}

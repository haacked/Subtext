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

		private int _blogID;
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _keyWordID;
		[XmlAttribute]
		public int KeyWordID
		{
			get {return this._keyWordID;}
			set {this._keyWordID = value;}
		}

		private string _format = null;
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
			_format = string.Format(System.Globalization.CultureInfo.InvariantCulture, "<a {0} href=\"{1}\" {2}>{3}</a>",this.Title != null ? "title=\"" + Title + "\"" : string.Empty,Url,this.OpenInNewWindow ? "target=\"_blank\"" : string.Empty, Text);
		}
	}
}

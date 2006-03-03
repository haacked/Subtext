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
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;

// TODO: Expose Ancestors for editing locally? N2H

namespace Subtext.Web.Admin.WebUI
{
	[
		ToolboxData("<{0}:BreadCrumbs runat=\"server\" />")
	]
	public class BreadCrumbs : Control
	{
		protected const string PREFIXTEXT_DEFAULT = "Location:";
		protected const string SEPARATOR_DEFAULT = "&nbsp;&#187;&nbsp;";
		protected const string SPACER_DEFAULT = "&nbsp;";
		protected Control container;
		protected PageLocation _lastItem;
		protected string _lastItemText;
		protected bool _lastItemOverride = false;

		protected string _pageID = String.Empty;		
		protected bool _isPanel = true;
		protected bool _includeRoot = true;
		protected bool _includeSelf = true;
		protected bool _lastItemStatic;
		
		protected bool _usePrefixText = false;
		protected string _prefixText = PREFIXTEXT_DEFAULT;
		protected bool _useSpacers = true;
		protected string _spacer = SPACER_DEFAULT;
		protected string _separator = SEPARATOR_DEFAULT;
		
		protected string _cssClass = String.Empty;
		protected string _linkCssClass = String.Empty;
		protected string _lastCssClass = String.Empty;
		protected NameValueCollection _linkStyle = new NameValueCollection();
		protected NameValueCollection _lastStyle = new NameValueCollection();

		public BreadCrumbs()
		{
			SetContainer();
		}

		#region Accessors
		public string PageID
		{
			get 
			{ 
				if (_pageID.Length > 0)
					return _pageID;
				else
					return this.Page.GetType().BaseType.FullName;
			}
			set { _pageID = value; }
		}

		public bool IsPanel
		{
			get { return _isPanel; }
			set 
			{
				if (_isPanel != value)
				{
					_isPanel = value;
					SetContainer();
				}			
			}
		}

		public bool IncludeRoot
		{
			get { return _includeRoot; }
			set { _includeRoot = value; }
		}

		public bool IncludeSelf
		{
			get { return _includeSelf; }
			set { _includeSelf = value; }
		}

		public bool UsePrefixText
		{
			get { return _usePrefixText; }
			set { _usePrefixText = value; }
		}

		public string PrefixText
		{
			get { return _prefixText; }
			set { _prefixText = value; }
		}

		public bool UseSpacers
		{
			get { return _useSpacers; }
			set { _useSpacers = value; }
		}

		public string Spacer
		{
			get { return _spacer; }
			set { _spacer = value; }
		}

		public string Separator
		{
			get { return _separator; }
			set { _separator = value; }
		}

		public bool LastItemStatic
		{
			get { return _lastItemStatic; }
			set { _lastItemStatic = value; }
		}

		public string CssClass
		{
			get { return _cssClass; }
			set { _cssClass = value; }
		}

		public string LinkCssClass
		{
			get { return _linkCssClass; }
			set { _linkCssClass = value; }
		}

		public string LastCssClass
		{
			get { return _lastCssClass; }
			set { _lastCssClass = value; }
		}

		public NameValueCollection LinkStyle
		{
			get { return _linkStyle; }
		}

		public NameValueCollection LastStyle
		{
			get { return _lastStyle; }
		}

		#endregion

		protected void SetContainer()
		{
			if (_isPanel)
			{
				container = new Panel();
				this.Controls.Clear();
				this.Controls.Add(container);
			}
			else
			{
				this.Controls.Clear();
				container = this;
			}
		}

		public void AddLastItem(string title)
		{
			_lastItemStatic = true;
			_lastItemOverride = true;
			_lastItemText = title;
		}

		public void AddLastItem(string title, string url)
		{
			AddLastItem(title, url, String.Empty);
		}

		public void AddLastItem(string title, string url, string description)
		{
			_lastItem = new PageLocation(String.Empty, title, url, description);			
			_lastItemOverride = true;
		}

		private string _fullyQualifiedBaseUrl;
		public string FullyQualifiedBaseUrl
		{
			get 
			{
				if( this._fullyQualifiedBaseUrl == null)
				{
					this._fullyQualifiedBaseUrl = Config.CurrentBlog.RootUrl + "admin/";

				}
				return this._fullyQualifiedBaseUrl;
			}
			set {this._fullyQualifiedBaseUrl = value;}
		}

		protected HyperLink CreateHyperLinkFromLocation(PageLocation value, bool isLastItem)
		{
			HyperLink result = new HyperLink();
			result.Text = value.Title;
			result.NavigateUrl = FullyQualifiedBaseUrl + value.Url;
			result.CssClass = isLastItem ? _lastCssClass : _linkCssClass;
			result = (HyperLink)ApplyStyles(result, isLastItem);
			result.Target = value.Target;

			return result;
		}

		protected WebControl ApplyStyles(WebControl control, bool isLastItem)
		{
			if (isLastItem && _lastStyle.Count > 0)
				return Utilities.CopyStyles(control, _lastStyle);
			else if (_linkStyle.Count > 0)
				return Utilities.CopyStyles(control, _linkStyle);
			else		
				return control;
		}

		protected PageLocation[] GetSampleAncestors()
		{
			PageLocation[] results = new PageLocation[3];
			results[0] = new PageLocation("", "Level 3", "#");
			results[1] = new PageLocation("", "Level 2", "#");
			results[2] = new PageLocation("", "Level 1", "#");
			return results;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			container.Controls.Clear();

			PageLocation[] lineage;
			if (null != HttpContext.Current)
				lineage = SiteMap.Instance.GetAncestors(this.PageID, _includeSelf);
			else
				lineage = GetSampleAncestors();

			if (null != lineage && lineage.Length > 0)
			{
				if (_usePrefixText && _prefixText.Length > 0)
				{
					container.Controls.Add(new LiteralControl(_prefixText));
					if (_useSpacers) 
						container.Controls.Add(new LiteralControl(_spacer));
				}

				int startAt = _includeRoot ? lineage.Length - 1 : lineage.Length - 2;
				for (int i = startAt; i >= 0; i--)
				{
					if (i > 0 || _lastItemOverride || !_lastItemStatic)
					{
						container.Controls.Add(CreateHyperLinkFromLocation(lineage[i], i > 0 || _lastItemOverride));
					}
					else //if (!_lastItemOverride && _lastItemStatic) (redundant)
					{
						container.Controls.Add(new LiteralControl(lineage[i].Title));
					}

					if (_useSpacers && (i > 0 || _lastItemOverride))
						container.Controls.Add(new LiteralControl(_separator));
				}

				if (_lastItemOverride)
				{
					if (null != _lastItem)
					{
						if (!_lastItemStatic)
							container.Controls.Add(CreateHyperLinkFromLocation(_lastItem, true));
						else
							container.Controls.Add(new LiteralControl(_lastItem.Title));
					}
					else if (_lastItemText.Length > 0)
						container.Controls.Add(new LiteralControl(_lastItemText));
				}
			}

			if (_isPanel && _cssClass.Length > 0)
				(container as Panel).CssClass = _cssClass;

			base.Render(writer);
		}
	}
}


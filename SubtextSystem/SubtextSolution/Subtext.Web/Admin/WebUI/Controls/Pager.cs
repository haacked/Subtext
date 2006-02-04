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

#undef Diagnostic

using System;
using System.Globalization;
using System.Web.UI;

namespace Subtext.Web.Admin.WebUI
{
	public enum DisplayType
	{
		Block,
		Inline
	}

	[
	System.ComponentModel.Designer(typeof(PagerDesigner)),
		ToolboxData("<{0}:Pager runat=\"server\" />")
	]
	public class Pager : Control
	{
		protected const string VSKEY_ITEMCOUNT = "ItemCount";
		protected const string VSKEY_PAGEINDEX = "PageIndex";
		protected const string VSKEY_PAGESIZE = "PageSize";
		protected const string VSKEY_DISPLAYPAGES = "DisplayPages";

		protected const int FIRST_PAGE_INDEX = 1;
		protected const int PAGESIZE_MIN = 1;
		protected const int PAGESIZE_DEFAULT = 20;
		protected const int DISPLAYPAGES_MIN = 3;
		protected const int DISPLAYPAGES_DEFAULT = 9;

		protected const string SPACER_DEFAULT = "&nbsp;";
		protected const string FIRSTTEXT_DEFAULT = "First";
		protected const string LASTTEXT_DEFAULT = "Last";
		protected const string PREFIXTEXT_DEFAULT = "Goto page&nbsp;";
		protected const string SUFFIXTEXT_DEFAULT = "";
		protected const string URLFORMAT_DEFAULT = "/?pageid={0}";
		protected const string LINKFORMAT_DEFAULT = "<a href=\"{0}\">{1}</a>";
		protected const string LINKFORMAT_ACTIVE_DEFAULT = "<a href=\"{0}\" class=\"Current\">&#187;{1}&#171;</a>";		

		protected DisplayType _displayMode = DisplayType.Block;

		protected string _cssClass;

		protected bool _useSpacer = true;
		protected string _spacer;

		protected string _urlFormat = URLFORMAT_DEFAULT;
		protected string _linkFormat = LINKFORMAT_DEFAULT;
		protected string _linkFormatActive = LINKFORMAT_ACTIVE_DEFAULT;

		protected bool _useFirstLast = true;
		protected string _firstText = FIRSTTEXT_DEFAULT;
		protected string _lastText = LASTTEXT_DEFAULT;

		protected bool _usePrefixSuffix = true;
		protected string _prefixText = PREFIXTEXT_DEFAULT;
		protected string _suffixText = SUFFIXTEXT_DEFAULT;

		protected int _padLeft;
		protected int _padRight;

		public Pager()
		{
			ViewState[VSKEY_ITEMCOUNT] = 0;
			ViewState[VSKEY_PAGEINDEX] = FIRST_PAGE_INDEX;
			ViewState[VSKEY_PAGESIZE] = PAGESIZE_DEFAULT;
			this.DisplayPages = DISPLAYPAGES_DEFAULT;
		}

		#region Accessors
		public DisplayType DisplayMode
		{
			get { return _displayMode; }
			set { _displayMode = value; }
		}

		public string CssClass
		{
			get { return _cssClass; }
			set { _cssClass = value; }
		}

		public int ItemCount
		{
			get 
			{ 				
				return (int)ViewState[VSKEY_ITEMCOUNT];
			}
			set 
			{ 
				if (value < 0)
					ViewState[VSKEY_ITEMCOUNT] = 0;
				else
					ViewState[VSKEY_ITEMCOUNT] = value;
			}
		}

		public int PageIndex
		{
			get 
			{ 
				return (int)ViewState[VSKEY_PAGEINDEX]; 
			}
			set 
			{ 
				if (value >= FIRST_PAGE_INDEX)
					ViewState[VSKEY_PAGEINDEX] = value; 
				else
					ViewState[VSKEY_PAGEINDEX] = FIRST_PAGE_INDEX;
			}
		}

		public int PageSize
		{
			get 
			{ 
				return (int)ViewState[VSKEY_PAGESIZE];
			}
			set 
			{ 
				if (value >= PAGESIZE_MIN)
					ViewState[VSKEY_PAGESIZE] = value; 
				else
					ViewState[VSKEY_PAGESIZE] = PAGESIZE_MIN;
			}
		}

		public int DisplayPages
		{
			get 
			{ 
				return (int)ViewState[VSKEY_DISPLAYPAGES];
			}
			set
			{
				int displayPages = value;
				if (displayPages < DISPLAYPAGES_MIN)
					displayPages = DISPLAYPAGES_MIN;
				
				ViewState[VSKEY_DISPLAYPAGES] = displayPages;			
			}
		}

		public int MaxPages
		{
			get 
			{ 
				if (PageSize > 0)
					return (int)Math.Ceiling((double)ItemCount/PageSize);
				else
					return 0;
			}
		}

		public string Spacer
		{	
			get 
			{ 
				if (null == _spacer || _spacer.Length == 0) 
					_spacer = SPACER_DEFAULT;

				return _spacer; 
			}
			set { _spacer = value; }
		}

		public bool UseSpacer
		{
			get { return _useSpacer; }
			set { _useSpacer = value; }
		}

		public string UrlFormat
		{	
			get { return _urlFormat; }
			set { _urlFormat = value; }
		}

		public string LinkFormat
		{
			get { return _linkFormat; }
			set { _linkFormat = value; }
		}

		public string LinkFormatActive
		{
			get { return _linkFormatActive; }
			set { _linkFormatActive = value; }
		}

		public bool UseFirstLast
		{
			get { return _useFirstLast; }
			set { _useFirstLast = value; }
		}

		public string FirstText
		{
			get { return _firstText; }
			set { _firstText = value; }
		}

		public string LastText
		{
			get { return _lastText; }
			set { _lastText = value; }
		}

		public bool UsePrefixSuffix
		{
			get { return _usePrefixSuffix; }
			set { _usePrefixSuffix = value; }
		}

		public string SuffixText
		{
			get { return _suffixText; }
			set { _suffixText = value; }
		}

		public string PrefixText
		{
			get { return _prefixText; }
			set { _prefixText = value; }
		}


		#endregion

		// TODO: linkcss
		// TODO: linkherecss

		protected void CalcPadding(int displayPages)
		{
			// want even padding if we can have it
			_padLeft = displayPages / 2;
			_padRight = _padLeft;

			// but if PageSize is even, shift current over one slot to the left by reducing _padLeft
			if (displayPages % 2 == 0)
				_padLeft--;
		}
		
		protected string RenderLink(int pageid, bool isCurrent)
		{
			return RenderLink(pageid, pageid.ToString(CultureInfo.InvariantCulture), isCurrent);
		}

		protected string RenderLink(int linkIndex, string display)
		{
			return RenderLink(linkIndex, display, false);
		}

		protected string RenderLink(int linkIndex, string display, bool isCurrent)
		{
			string url = String.Format(_urlFormat, linkIndex);
			return String.Format(isCurrent ? _linkFormatActive : _linkFormat,
				url, display);
		}

		protected virtual void WriteConditional(HtmlTextWriter writer, string value, bool condition)
		{
			if (condition) writer.Write(value);
		}

		#region Render
		protected override void Render(HtmlTextWriter writer)
		{	
			// there's only 1 page, a pager is useless so render nothing
			if (FIRST_PAGE_INDEX == MaxPages) return;

			if (_cssClass.Length > 0)
				writer.AddAttribute("class", _cssClass);

			if (_displayMode == DisplayType.Block)
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
			else
				writer.RenderBeginTag(HtmlTextWriterTag.Span);

			// write prepended label if appropriate and an optional spacer literal
			WriteConditional(writer, _prefixText, _usePrefixSuffix);
			WriteConditional(writer, Spacer, _useSpacer);

			CalcPadding(DisplayPages);

			// there may be a more straightforward way of dealing with padding edge cases,
			// we thought we had one but it was not catching the very first use case (selected
			// index = first page). This seems more kludgy, but it works.
			//
			// we can't have overflow and underflow at the same time if we have more possible 
			// pages to display than we've allowed for. if we can display all the possible pages
			// without scrolling left and right, then we only need to worry about the left
			// padding and keeping the initial counter in the right place (1).
			//
			// what we'll do is see if our current selected index is with in range (amount of 
			// padding) of either the first or last pages. if it is, then we'll shift the 
			// amount of padding slots that we can't use to the other end.
			if ((PageIndex - _padLeft) <= 0)
			{
				// our current index falls inside the padded beginning: underflow
				_padRight += _padLeft - PageIndex + 1;
				_padLeft = PageIndex - 1;
			}
			else if ((PageIndex + _padRight) > MaxPages)
			{
				// our current index falls inside the padded end: overflow
				_padLeft += _padRight - (MaxPages - PageIndex);
				_padRight = MaxPages - PageIndex;
			}

			// walk the counter backwards to the first item we're going to display from the 
			// currently selected item. _padLeft will put as at the right place to start.
			int counter = _padLeft;
			int idx = 1;
			while (counter > 0)
			{	
				idx = PageIndex - counter;
				if (idx >= FIRST_PAGE_INDEX)
					break;				
				counter--;
			}

			// if we specified including 'First' link back to page 1, write it plus an 
			// optional spacer
			if (idx > FIRST_PAGE_INDEX && _useFirstLast) 
			{
				writer.Write(RenderLink(FIRST_PAGE_INDEX, _firstText));
				WriteConditional(writer, Spacer, _useSpacer);
			}

			// starting at the place where we walked the counter back to, draw N links
			// as long as we're in the allowable bounds
			for (int i = idx; i < idx + DisplayPages; i++)
			{
				if (i >= FIRST_PAGE_INDEX && i <= MaxPages)
				{
					writer.Write(RenderLink(i, i == PageIndex));					
					WriteConditional(writer, Spacer, _useSpacer);
				}
			}

			// if we specified including 'Last' link back to the last page, write it plus 
			// an optional spacer
			if (!(idx + DisplayPages > MaxPages) && _useFirstLast) 
			{
				writer.Write(RenderLink(MaxPages, _lastText));
				WriteConditional(writer, Spacer, _useSpacer);
			}

			WriteConditional(writer, _suffixText, _usePrefixSuffix);

			writer.RenderEndTag();

			#if Diagnostic
			writer.Write("<br />PageIndex={0}, _padLeft={1}, _padRight={2}, MaxPages={3}, DisplayPages={4}, ItemCount={5}<br />", 
				PageIndex, _padLeft, _padRight, MaxPages, DisplayPages, ItemCount);
			#endif
			
		}
		#endregion
	}

	public class PagerDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{			
			return base.GetDesignTimeHtml();
		}

		public override void Initialize(System.ComponentModel.IComponent component)
		{
			if (component is Pager)
			{
				Pager context = component as Pager;
				context.PageSize = 10;
				context.ItemCount = 120;
				context.PageIndex++;
			}

			base.Initialize(component);
		}

	}
}


#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

#undef Diagnostic

using System;
using System.Globalization;
using System.Web.UI;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Control used to render paging through records.
    /// </summary>
    [ToolboxData("<{0}:PagingControl runat=\"server\" />")]
    public class PagingControl : Control
    {
        protected const string DefaultActiveLinkFormat =
            @"<a href=""{0}"" title=""page"" class=""Current"">&#187;{1}&#171;</a>";

        protected const int DefaultDisplayPageCount = 9;

        protected const string DefaultFirstPageText = "First";
        protected const string DefaultGoToPageText = "Goto page&nbsp;";
        protected const string DefaultLastPageText = "Last";
        protected const string DefaultLinkFormat = @"<a href=""{0}"" title=""Page"">{1}</a>";
        protected const int DefaultPageSize = 10;
        protected const string DefaultPageUrlFormat = "/?pageid={0}";
        protected const string DefaultSuffixText = "";
        protected const int FirstPageIndex = 0;
        protected const int MinDisplayPageCount = 3;
        protected const int MinPageSize = 1;
        protected const string SpacerDefault = "&nbsp;";

        protected string _cssClass;

        protected string _firstText = DefaultFirstPageText;
        protected string _lastText = DefaultLastPageText;
        protected string _linkFormat = DefaultLinkFormat;
        protected string _linkFormatActive = DefaultActiveLinkFormat;

        protected string _prefixText = DefaultGoToPageText;
        protected string _spacer;
        protected string _suffixText = DefaultSuffixText;
        protected string _urlFormat = DefaultPageUrlFormat;
        protected bool _usePrefixSuffix = true;
        protected bool _useSpacer = true;
        protected bool displayFirstLastPageLinks = true;

        /// <summary>
        /// Constructs an instance of this control.
        /// </summary>
        public PagingControl()
        {
            DisplayPageCount = DefaultDisplayPageCount;
        }

        protected override void OnInit(EventArgs args)
        {
        }

        /// <summary>
        /// Renders the link for the specified page index in the paging control.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="isCurrent">Whether or not the pageindex to render is the current page.</param>
        /// <returns></returns>
        protected string RenderLink(int pageIndex, bool isCurrent)
        {
            return RenderLink(pageIndex, (pageIndex + 1).ToString(CultureInfo.InvariantCulture), isCurrent);
        }

        /// <summary>
        /// Renders the link for the specified page index in the paging control.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="display">The display text of the link</param>
        /// <returns></returns>
        protected string RenderLink(int pageIndex, string display)
        {
            return RenderLink(pageIndex, display, false);
        }

        /// <summary>
        /// Renders the link for the specified page index in the paging control.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="display">The display text of the link</param>
        /// <param name="isCurrent">Whether or not the pageindex to render is the current page.</param>
        /// <returns></returns>
        protected string RenderLink(int pageIndex, string display, bool isCurrent)
        {
            string url = String.Format(CultureInfo.InvariantCulture, _urlFormat, pageIndex);
            return String.Format(CultureInfo.InvariantCulture, isCurrent ? _linkFormatActive : _linkFormat, url, display);
        }

        protected virtual void WriteConditional(HtmlTextWriter writer, string value, bool condition)
        {
            if (condition)
            {
                writer.Write(value);
            }
        }

        #region Render

        protected override void Render(HtmlTextWriter writer)
        {
            // there's only 1 page, a pager is useless so render nothing
            if (TotalPageCount == 0 || FirstPageIndex == (TotalPageCount - 1))
            {
                return;
            }

            var currentPage = (int)ViewState[ViewStateKeys.PageIndex];

            if (_cssClass.Length > 0)
            {
                writer.AddAttribute("class", _cssClass);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // write prepended label if appropriate and an optional spacer literal
            WriteConditional(writer, _prefixText, _usePrefixSuffix);

            // determine the start and end pages
            int startPage = currentPage - DisplayPageCount / 2 < 0 ? 0 : currentPage - DisplayPageCount / 2;

            int endPage = startPage + DisplayPageCount > LastPageIndex
                              ? LastPageIndex + 1
                              : startPage + DisplayPageCount;

            // if the start page isn't the first, then we display << to allow
            // paging backwards DisplayCountPage
            if (startPage != 0)
            {
                // if we specified including 'First' link back to pageindex 0, 
                // write it plus an optional spacer
                if (displayFirstLastPageLinks)
                {
                    writer.Write(RenderLink(FirstPageIndex, _firstText));
                }
                //we will page back DisplayPageCount unless that number is less than 0
                //since you can't page less than that.
                writer.Write(RenderLink(currentPage - DisplayPageCount <= 0 ? 0 : currentPage - DisplayPageCount - 1,
                                        "<<"));
            }
            //Now, loop through start to end and display all the links.
            for (int i = startPage; i < endPage; i++)
            {
                writer.Write(RenderLink(i, i == PageIndex));
            }

            // if we're already displaying the last page, no need for paging or Last Page link
            if (endPage - 1 != LastPageIndex)
            {
                writer.Write(
                    RenderLink(
                        currentPage + DisplayPageCount + 1 < LastPageIndex
                            ? currentPage + DisplayPageCount + 1
                            : LastPageIndex, ">>"));
                // if we specified including 'Last' link back to the last page, write it plus 
                // an optional spacer
                if (displayFirstLastPageLinks && PageIndex < LastPageIndex)
                {
                    writer.Write(RenderLink(LastPageIndex, _lastText));
                }
            }

            WriteConditional(writer, _suffixText, _usePrefixSuffix);

            writer.RenderEndTag();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets and sets the CssClass to use for this control.
        /// </summary>
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        /// <summary>
        /// ?
        /// </summary>
        public int ItemCount
        {
            get { return ViewState[ViewStateKeys.ItemCount] == null ? 0 : (int)ViewState[ViewStateKeys.ItemCount]; }
            set
            {
                ViewState[ViewStateKeys.ItemCount] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// The current page index.
        /// </summary>
        public int PageIndex
        {
            get
            {
                return ViewState[ViewStateKeys.PageIndex] == null
                           ? FirstPageIndex
                           : (int)ViewState[ViewStateKeys.PageIndex];
            }
            set
            {
                ViewState[ViewStateKeys.PageIndex] = value >= FirstPageIndex ? value : FirstPageIndex;
            }
        }

        /// <summary>
        /// Number of allowed records in a page.
        /// </summary>
        public int PageSize
        {
            get { return ViewState[ViewStateKeys.PageSize] == null ? 10 : (int)ViewState[ViewStateKeys.PageSize]; }
            set
            {
                ViewState[ViewStateKeys.PageSize] = value >= MinPageSize ? value : MinPageSize;
            }
        }

        /// <summary>
        /// The number of pages to display at a time.
        /// </summary>
        public int DisplayPageCount
        {
            get
            {
                return ViewState[ViewStateKeys.DisplayPageCount] == null
                           ? 1
                           : (int)ViewState[ViewStateKeys.DisplayPageCount];
            }
            set
            {
                int dislpayPageCount = value;
                if (dislpayPageCount < MinDisplayPageCount)
                {
                    dislpayPageCount = MinDisplayPageCount;
                }

                ViewState[ViewStateKeys.DisplayPageCount] = dislpayPageCount;
            }
        }

        /// <summary>
        /// The total number of pages in the set we are 
        /// pagingi through.
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                if (PageSize > 0)
                {
                    return (int)Math.Ceiling((double)ItemCount / PageSize);
                }
                return 0;
            }
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
            get { return displayFirstLastPageLinks; }
            set { displayFirstLastPageLinks = value; }
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

        /// <summary>
        /// Returns the index of the very last page in the set.
        /// </summary>
        public int LastPageIndex
        {
            get { return TotalPageCount - 1; }
        }

        #endregion

        #region Nested type: ViewStateKeys

        internal sealed class ViewStateKeys
        {
            internal const string DisplayPageCount = "DisplayPages";
            internal const string ItemCount = "ItemCount";
            internal const string PageIndex = "PageIndex";
            internal const string PageSize = "PageSize";
        }

        #endregion
    }
}
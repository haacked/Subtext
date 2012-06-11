<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Footer" %>
<%@ Register TagName="RecentComments" TagPrefix="Subtext" Src="RecentComments.ascx" %>
<div id="footer">
    <Subtext:RecentComments ID="recentComments" runat="server" />
    <div class="footer-about">
        <h4>
            About</h4>
        <p>
            This template is built with validated CSS and XHTML, by <a href="http://www.ndesign-studio.com">
                N.Design Studio</a>. Icons used here are from <a href="http://www.ndesign-studio.com/stock-icons/web-2-mini">
                    Web 2 Mini</a> pack.</p>
    </div>
    <hr class="clear" />
</div>

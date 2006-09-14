<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div id="myLinks">
	<div class="links">
		<asp:hyperlink cssclass="Home" ToolTip="Home" runat="server" navigateurl="~/Default.aspx" text="&nbsp;&nbsp;&nbsp;&nbsp;" id="HomeLink" />&nbsp;|&nbsp;
		<asp:hyperlink cssclass="Contact" ToolTip="Contact" runat="server" navigateurl="~/Contact.aspx" text="&nbsp;&nbsp;&nbsp;&nbsp;" id="ContactLink" />&nbsp;|&nbsp;
		<asp:hyperlink cssclass="Syndication" ToolTip="RSS 2.0 Syndication" runat="server" navigateurl="~/Rss.aspx" text="&nbsp;&nbsp;&nbsp;&nbsp;" id="Syndication" />&nbsp;|&nbsp;
		<asp:hyperlink cssclass="atom" ToolTip="Atom Syndication" runat="server" navigateurl="~/Atom.aspx" text="&nbsp;&nbsp;&nbsp;&nbsp;" id="AtomLink" />&nbsp;|&nbsp;
		<asp:HyperLink cssclass="Admin" Runat="server" ToolTip="Login" ID="Admin" ImageUrl="~/skins/curvy/Images/key_go.gif" />&nbsp;
	</div>
</div>

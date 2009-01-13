<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<dl class="Navigation">
	<dt>
		Site Sections
	</dt>
	<dd>
		<ul>
			<li><asp:hyperlink cssclass="Home" runat="server" navigateurl="~/Default.aspx" text="Home" id="HomeLink" /></li>
			<li><asp:hyperlink cssclass="Contact" runat="server" navigateurl="~/Contact.aspx" text="Contact" id="ContactLink" /></li>
			<li><asp:hyperlink cssclass="Syndication" runat="server" text="Syndication" id="Syndication" /></li>
			<li><asp:hyperlink cssclass="Admin" runat="server" text="Admin" id="Admin" /></li>
		</ul>
	</dd>
</dl>
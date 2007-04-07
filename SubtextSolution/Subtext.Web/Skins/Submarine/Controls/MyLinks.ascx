<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>

<div id="myLinks">
	<div class="title">
		
	</div>
	<div class="links">
		<asp:hyperlink cssclass="home" runat="server" navigateurl="~/Default.aspx" ImageUrl="../images/home.gif" id="HomeLink" />&nbsp;&nbsp;&nbsp;
		<asp:hyperlink cssclass="contact" runat="server" navigateurl="~/Contact.aspx" text="Contact" id="ContactLink" />&nbsp;&nbsp;&nbsp;
		<asp:hyperlink cssclass="archives" runat="server" navigateurl="~/Archives.aspx" text="Archives"	id="Archives" />&nbsp;&nbsp;&nbsp;
		<asp:hyperlink cssclass="syndication" runat="server" text="Syndication" id="Syndication" />
	</div>
	<asp:hyperlink runat="server" Visible="False" id="XMLLink" />

</div>

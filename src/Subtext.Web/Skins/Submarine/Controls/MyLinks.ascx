<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>

<div id="myLinks">
	<div class="title">
		
	</div>
	<div class="links">
		<asp:hyperlink cssclass="home" runat="server" navigateurl="~/Default.aspx" id="HomeLink"><div id="homeIcon"></div></asp:hyperlink>
		<asp:hyperlink cssclass="contact" runat="server" navigateurl="~/Contact.aspx" text="Contact" id="ContactLink" />
		<asp:hyperlink cssclass="archives" runat="server" navigateurl="~/Archives.aspx" text="Archives"	id="Archives" />
		<asp:hyperlink cssclass="syndication" runat="server" text="Syndication" id="Syndication" />
	</div>
</div>

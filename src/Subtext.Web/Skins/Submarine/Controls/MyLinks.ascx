<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>

<div id="myLinks">
	<div class="title">
		
	</div>
	<div class="links">
		<st:NavigationLink ActiveCssClass="active" cssclass="home" runat="server" navigateurl="~/Default.aspx" id="HomeLink"><div id="homeIcon"></div></st:NavigationLink>
		<st:NavigationLink ActiveCssClass="active" cssclass="contact" runat="server" navigateurl="~/Contact.aspx" text="Contact" id="ContactLink" />
		<st:NavigationLink ActiveCssClass="active" cssclass="archives" runat="server" navigateurl="~/Archives.aspx" text="Archives"	id="Archives" />
		<asp:HyperLink cssclass="syndication" runat="server" text="Syndication" id="Syndication" />
	</div>
</div>

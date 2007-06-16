<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<div id="header-links">
	<p>	
		<asp:hyperlink cssclass="Syndication" runat="server" id="Syndication"><asp:Image runat="server" ID="rssIcon" ImageUrl="../images/feed-icon16x16.png" Width="16" Height="16" /></asp:hyperlink><asp:hyperlink runat="server" id="XMLLink" Text="Subscribe!" />
	</p>		
</div>

<!-- Menu Tabs -->
<ul>
	<li><asp:hyperlink cssclass="Home" runat="server" navigateurl="~/Default.aspx" text="Home" id="Home2" /></li>
	<li><asp:hyperlink cssclass="archives" runat="server" navigateurl="~/Archives.aspx" text="Archives"	id="Archives" /></li>
	<li><asp:hyperlink cssclass="Contact" runat="server" navigateurl="~/Contact.aspx" text="Contact" id="Hyperlink2" /></li>
	<li><asp:hyperlink cssclass="Admin" runat="server" text="Admin" id="Admin" /></li>
	<!-- User can link to custom articles here.
	<li><a href="index.html">Downloads</a></li>
	<li><a href="index.html">Support</a></li>
	<li><a href="index.html">About</a></li>-->	
</ul>




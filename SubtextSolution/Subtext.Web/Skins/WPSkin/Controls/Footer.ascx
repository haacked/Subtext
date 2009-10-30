<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Footer" %>
<p id="footer">
	Powered by: 
	<br />
	<a href="http://subtextproject.com/" title="Powered By Subtext"><img src="<%= Url.ImageUrl("PoweredBySubtext85x33.png") %>" width="85" height="33" alt="Powered By Subtext" /></a>
	<a href="http://asp.net/" title="Powered By ASP.NET"><img src="<%= Url.ImageUrl("PoweredByAsp.Net.gif") %>" width="85" height="33" alt="Powered By ASP.NET" /></a>
	<br />
	Copyright &copy; <asp:Literal id="FooterText" runat="server" />
</p>
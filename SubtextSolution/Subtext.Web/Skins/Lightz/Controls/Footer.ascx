<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Footer" %>
<div class="footer">
	Powered by: 
	<br />
	<asp:HyperLink ImageUrl="~/images/PoweredBySubtext85x33.png" NavigateUrl="http://sourceforge.net/projects/subtext/" Runat="server" ID="Hyperlink2" NAME="Hyperlink1" ToolTip="Powered By Subtext" />
	<asp:HyperLink ImageUrl="~/images/PoweredByAsp.Net.gif" NavigateUrl="http://ASP.NET" Runat="server" ID="Hyperlink3" ToolTip="Powered By ASP.NET" />
	<br />
	Copyright &copy; <asp:Literal id="FooterText" runat="server" />
</div>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<div id="top">
<table width="100%" cellpadding="0" cellspacing="0">
	<tr>
		<td width="398"><img src="~/Skins/AnotherEon001/images/header.jpg" width="398" height="75" runat="server" ID="Img1"></td>
		<td align="right" valign="middle" style="padding-right:8px;"><a href="http://www.webhost4life.com/default.asp?refid=chrisndotnet" target="_blank" title="ASP.NET, SQLServer, 150MB - $9.95/month!"><img src="~/Skins/AnotherEon001/images/wh4l_banner001.gif" width="211" height="60" alt="ASP.NET, SQLServer, 150MB - $9.95/month!" runat="server"></a></td>
	</tr>
</table>
</div>
<div id="sub"><uc1:BlogStats id="BlogStats1" runat="server"></uc1:BlogStats></div>
<asp:HyperLink id="HeaderTitle" CssClass="headermaintitle" runat="server" Visible="False" />
<asp:Literal id="HeaderSubTitle" runat="server" Visible="False" />

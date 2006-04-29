<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<div id="MyLinks">
	<table  style="border: 1px solid #ccc; border-collapse:collapse" id="tblinks" cellspacing="0" cellpadding="0" width="100%">
		<tr>
			<td height="25" width="80">
				<asp:HyperLink Runat="server" Text="Home" title="Home" ID="HomeLink" NavigateUrl="~/Default.aspx" Visible="True" />
			</td>
			<td width="80" style="border: 1px solid #ccc">
				<asp:HyperLink Runat="server" Text="Contact" title="Contact" ID="ContactLink" NavigateUrl="~/Contact.aspx" AccessKey="9" />
			</td>
			<td width="80" style="border: 1px solid #ccc">
				<asp:HyperLink Runat="server" Text="Login" title="Login" ID="Admin" />
			</td>
			<td width="90" style="border: 1px solid #ccc">
				<asp:HyperLink Runat="server" Text="RSS 2.0" title="RSS 2.0" ID="Syndication" NavigateUrl="~/Rss.aspx" />
			</td>
			<td align="right" style="border: 1px solid #ccc">
				<uc1:blogstats id="BlogStats1" runat="server" />
			</td>
		</tr>
	</table>
</div>

<!-- Not Visible -->

<asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" NavigateUrl="~/Rss.aspx" ID="XMLLink" Visible="False"></asp:HyperLink>


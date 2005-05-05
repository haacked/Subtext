<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>
<uc1:MyLinks id="MyLinks1" runat="server" />
<span id="blogstats"><uc1:BlogStats id="BlogStats1" runat="server" /></span>

<!-- Not Visible -->
<asp:HyperLink id="HeaderTitle" Visible="False" Width="772" Height="174" runat="server" />
<asp:Literal id="HeaderSubTitle" runat="server" Visible="False" />
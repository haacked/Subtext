<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>
<h1>
    <span id="star" title="DESiGN 2.0"></span><a href="index.htm"><asp:HyperLink id="HeaderTitle" runat="server" CssClass="blogTitle" title="Blog Title" /></a>
</h1>

<!-- Not Visible -->
<asp:Literal id="HeaderSubTitle" runat="server" Visible="False" />
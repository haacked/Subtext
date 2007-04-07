<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>
<h1 id="header">
    <asp:HyperLink id="HeaderTitle" runat="server" CssClass="blogTitle" title="Blog Title" />
</h1>

<!-- Not Visible -->
<asp:Literal id="HeaderSubTitle" runat="server" Visible="False" />
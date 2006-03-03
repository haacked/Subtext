<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>
<div id="logo">
	<h1><asp:HyperLink id="HeaderTitle" runat="server" CssClass="blogTitle" title="Blog Title" /></h1>
</div> <!-- #end logo -->
<uc1:MyLinks id="MyLinks1" runat="server" />
<span id="blogstats"><uc1:BlogStats id="BlogStats1" runat="server" /></span>

<!-- Not Visible -->

<asp:Literal id="HeaderSubTitle" runat="server" Visible="False" />
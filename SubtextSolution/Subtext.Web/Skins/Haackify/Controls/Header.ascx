<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>
<div id="logo">
	<h1><asp:HyperLink id="HeaderTitle" CssClass="maintitle" runat="server" title="Blog Title" /></h1>
	<div id="subtitle"><h2><asp:Literal id="HeaderSubTitle" runat="server" /></h2></div>
</div> <!-- #end logo -->
<div id="nav">
	<uc1:MyLinks id="MyLinks1" runat="server" />
	<span id="blogstats"><uc1:BlogStats id="BlogStats1" runat="server" /></span>
</div><!-- end #nav -->


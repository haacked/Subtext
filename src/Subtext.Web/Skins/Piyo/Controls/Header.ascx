<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="SubtextSearch.ascx" %>
<div id="name">
	<h1><asp:HyperLink id="HeaderTitle" runat="server" /></h1>
	<h2><asp:Literal id="HeaderSubTitle" runat="server" /></h2>
</div>
<div id="controls">
    <uc1:Search ID="search" runat="server" />
	<div style="float:right;"><uc1:BlogStats id="stats" runat="server" /></div>
	<div id="switcher">
	    <a id="switchlink" href="#" title="Click here to change the content width" style="display:none;"><span></span></a>
	</div>
</div>

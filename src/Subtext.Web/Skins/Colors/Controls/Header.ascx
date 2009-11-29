<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="SubtextSearch.ascx" %>

<div id="header">
	<div id="title">
		<h1><asp:HyperLink id="HeaderTitle" CssClass="headermaintitle" runat="server" /></h1>
		<h2><asp:Literal id="HeaderSubTitle" runat="server" /></h2>
	</div>
	
	<uc1:SubtextSearch id="search" runat="server" />
</div>
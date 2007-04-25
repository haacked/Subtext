<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="SubtextSearch.ascx" %>

	<div class="title">
		<asp:HyperLink id="HeaderTitle" runat="server" />
	</div>
	<div class="subtitle">
		<asp:Literal id="HeaderSubTitle" runat="server" />
	</div>
        <uc1:SubtextSearch id="search" runat="server" />


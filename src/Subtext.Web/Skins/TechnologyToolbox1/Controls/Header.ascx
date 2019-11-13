<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="SubtextSearch" Src="SubtextSearch.ascx" %>
<!-- Begin: Header.ascx -->
<div id="header">
    <div id="headerimg">
        <h1>
            <asp:HyperLink ID="HeaderTitle" runat="server" /></h1>
        <div id="description">
            <asp:Literal ID="HeaderSubTitle" runat="server" /></div>
    </div>
</div>
<uc1:SubtextSearch runat="server" ID="search" Visible="false" />
<!-- End: Header.ascx -->
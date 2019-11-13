<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="MyLinks.ascx" %>

<div id="header">
    <div id="headerimg">
        <h1><asp:HyperLink id="HeaderTitle" runat="server" /></h1>
        <div class="description"><asp:Literal id="HeaderSubTitle" runat="server" /></div>
    </div>
    <uc1:MyLinks ID="MyLinks" runat="server" />
</div>
<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.SubtextSearch" %>
<!-- Begin: SubtextSearch.ascx -->
<div id="nav" class="clearfix">
    <div id="nav-search">
        <div>
            <label class="screen-reader-text" for="s">
                Search for:</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="searchterm" />
            <asp:Button ID="btnSearch" runat="server" CssClass="searchButton" Text="Search" CausesValidation="False"
                OnClick="btnSearch_Click" />
        </div>
    </div>
</div>
<!-- End: SubtextSearch.ascx -->

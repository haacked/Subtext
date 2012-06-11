<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - FullText Search" Codebehind="FullTextSearch.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.FullTextSearch" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />
<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ID="passwordContent" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<h2>FullText Search</h2>
	<div class="section">
	
	<fieldset title="index stats">
	    <legend>Index stats</legend>
	    <p>Your blog currently has <asp:Literal runat="server" id="ltrPostIndexedCount" /> posts in the fulltext index</p>
        <p>
            <asp:Button id="btnRebuild" runat="server" Text="Rebuild Index" CssClass="buttonSubmit" ValidationGroup="importGroup" OnClick="btnRebuild_Click" />
        </p>
	</fieldset>
	
	</div>
</asp:Content>
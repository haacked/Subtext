<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Preferences" Codebehind="Preferences.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditPreferences" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="preferencesContent" ContentPlaceHolderID="pageContent" runat="server">
	<h2>Preferences</h2>
	<div class="preferences">
	    <div>
	        <label for="Edit_ddlPublished">Always create new items as Published</label>
    		<asp:DropDownList id="ddlPublished" runat="server" AutoPostBack="false" CssClass="number">
			    <asp:ListItem Value="true">Yes</asp:ListItem>
			    <asp:ListItem Value="false">No</asp:ListItem>
		    </asp:DropDownList> 
		    
	    </div>
	    <div>
	        <label for="Edit_ddlExpandAdvanced">Always expand advanced options</label>
	        <asp:DropDownList id="ddlExpandAdvanced" runat="server" AutoPostBack="false" CssClass="number">
			    <asp:ListItem Value="true">Yes</asp:ListItem>
			    <asp:ListItem Value="false">No</asp:ListItem>
		    </asp:DropDownList> 
	    </div>
        <div>
            <label for="Edit_ddlUsePlainHtmlEditor">Use plain HTML editor</label>
            <asp:DropDownList id="ddlUsePlainHtmlEditor" runat="server" AutoPostBack="false" CssClass="number">
			    <asp:ListItem Value="true">Yes</asp:ListItem>
			    <asp:ListItem Value="false">No</asp:ListItem>            
            </asp:DropDownList>
        </div>
	    <div class="button-div">
		    <asp:Button id="lkbUpdate" runat="server" Text="Save" CssClass="buttonSubmit" onclick="lkbUpdate_Click" />
	    </div>
	</div>
</asp:Content>
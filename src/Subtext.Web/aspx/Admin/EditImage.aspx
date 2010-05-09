<%@ Page Language="C#" Title="Subtext Admin - Edit Image" Codebehind="EditImage.aspx.cs" Inherits="Subtext.Web.Admin.Pages.EditImage" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/aspx/Admin/UserControls/CategoryLinkList.ascx" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
    <h2>Images</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="ImageCollection" />
</asp:Content>

<asp:Content ID="imageContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" />

	<h2 id="headerLiteral" runat="server">Edit Image</h2>
	<fieldset>
	    <legend>Update Image Details</legend>
	    <asp:Label runat="server" AssociatedControlID="txbTitle">Title</asp:Label>
	    <asp:TextBox id="txbTitle" runat="server" Text='<%# Image.Title %>' columns="255" width="70%" />
	    <label>Dimensions</label>
	    <span><%# Image.Width.ToString() %>W x <%# Image.Height.ToString() %>H</span>
	    <asp:Label runat="server" AssociatedControlID="ddlGalleries">Gallery</asp:Label>
	    <asp:DropDownList id="ddlGalleries" runat="server" />
	    <div class="button-div">
	        <asp:CheckBox id="ckbPublished" runat="server" Text="Visible" />
        </div>
	    <asp:HyperLink id="lnkThumbnail" runat="server" Visible="false" CssClass="Thumbnail" />
   	    
		<div class="button-div clear">
		    <div>
                <label class="inline">Original Image:</label> <%# Image.OriginalFile %>
		        <label class="inline">Thumbnail Image:</label> <%# Image.ThumbNailFile %> 
		        <label class="inline">Resized Image:</label> <%# Image.ResizedFile %>
            </div>

		    <asp:Button id="lkbUpdateImage" runat="server" CssClass="buttonSubmit" Text="Update" onclick="lkbUpdateImage_Click" />
	    </div>
	</fieldset>
	<fieldset>
	    <legend>Replace File</legend>
	    <asp:Label runat="server" AssociatedControlID="ImageFile">Local File Location</asp:Label>
	    <input id="ImageFile" class="FileUpload" type="file" size="82" runat="server" name="ImageFile" />
	    <div class="button-div">
		    <asp:Button id="lbkAddImage" runat="server" CssClass="buttonSubmit" Text="Replace" onclick="lbkReplaceImage_Click" />
	    </div>		
	</fieldset>
</asp:Content>

<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Image" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="EditImage.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditImage" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/Admin/UserControls/CategoryLinkList.ascx" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
    <h2>Images</h2>
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="ImageCollection" />
</asp:Content>

<asp:Content ID="imageContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" />

	<st:AdvancedPanel id="ImageDetails" runat="server" Collapsible="false" HeaderText="Image Details" HeaderCssClass="CollapsibleHeader" BodyCssClass="Edit" DisplayHeader="True">
		
		<ASP:HyperLink id="lnkThumbnail" runat="server" Visible="false" CssClass="Thumbnail"></ASP:HyperLink>
		<p class="ThumbnailTitle">
			<%# Image.Title %>
			<br />
			<span><%# Image.Width.ToString() %>W x <%# Image.Height.ToString() %>H</span><br />
			<span><a href='<%# GetImageGalleryUrl() %>'><%# _galleryTitle %></a></span><br />
		</p>
		<br class="clear" />
		<label>Title</label>
		<ASP:TextBox id="txbTitle" runat="server" Text='<%# Image.Title %>' columns="255" width="98%" />
		<label>Gallery</label>
		<ASP:DropDownList id="ddlGalleries" runat="server" />
		<p style="margin-top: 8px;"><label>Visible</label><ASP:checkbox id="ckbPublished" runat="server" /></p>
		<div style="margin-top: 8px">
			<asp:linkbutton id="lkbUpdateImage" runat="server" CssClass="Button" Text="Update" onclick="lkbUpdateImage_Click" />
			<br />&nbsp; 
		</div>			
		<p class="InlineSubtitle">Replace File</p>
		<label>Local File Location</label>
		<input id="ImageFile" class="FileUpload" type="file" size="82" runat="server" name="ImageFile" />
		<br class="clear" />		
		<div style="margin-top: 8px">
			<asp:Button id="lbkAddImage" runat="server" CssClass="buttonSubmit" Text="Replace" onclick="lbkReplaceImage_Click" />
			<br />&nbsp; 
		</div>		
	</st:AdvancedPanel>	
	
	<st:AdvancedPanel id="Advanced" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" LinkText="[toggle]" Collapsible="True" HeaderText="Advanced Options" BodyCssClass="Edit" Collapsed="true" HeaderTextCssClass="CollapsibleTitle">	
		<label>Original Image</label>
		<%# Image.OriginalFile %>
		<label>Thumbnail Image</label>
		<%# Image.ThumbNailFile %>
		<label>Resized Image</label>
		<%# Image.ResizedFile %>
	</st:AdvancedPanel>
</asp:Content>

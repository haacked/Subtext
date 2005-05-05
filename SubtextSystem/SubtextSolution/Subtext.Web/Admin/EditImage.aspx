<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web.Admin" %>
<%@ Page language="c#" Codebehind="EditImage.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.EditImage" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Galleries" CategoryType="ImageCollection">

	<ANW:MessagePanel id="Messages" runat="server" />

	<ANW:AdvancedPanel id="ImageDetails" runat="server" Collapsible="false" HeaderText="Image Details" HeaderCssClass="CollapsibleHeader" BodyCssClass="Edit" DisplayHeader="True">
		
		<ASP:HyperLink id="lnkThumbnail" runat="server" Visible="false" CssClass="Thumbnail"></ASP:HyperLink>
		<p class="ThumbnailTitle">
			<%# Image.Title %>
			<br>
			<span><%# Image.Width.ToString() %>W x <%# Image.Height.ToString() %>H</span><br>
			<span><a href='<%# GetImageGalleryUrl() %>'><%# _galleryTitle %></a></span><br>
		</p>
		<br class="Clear">
		<label class="Block">Title</label>
		<ASP:TextBox id="txbTitle" runat="server" Text='<%# Image.Title %>' columns="255" width="98%" />
		<label class="Block">Gallery</label>
		<ASP:DropDownList id="ddlGalleries" runat="server" />
		<p style="margin-top: 8px;"><label>Visible</label><ASP:checkbox id="ckbPublished" runat="server" /></p>
		<div style="margin-top: 8px">
			<asp:linkbutton id="lkbUpdateImage" runat="server" CssClass="Button" Text="Update"/>
			<br>&nbsp; 
		</div>			
		<p class="InlineSubtitle">Replace File</p>
		<label class="Block">Local File Location</label>
		<input id="ImageFile" class="FileUpload" type="file" size="82" runat="server" name="ImageFile">
		<br class="Clear">		
		<div style="margin-top: 8px">
			<asp:linkbutton id="lbkAddImage" runat="server" CssClass="Button" Text="Replace"/>
			<br>&nbsp; 
		</div>		
	</ANW:AdvancedPanel>	
	
	<ANW:AdvancedPanel id="Advanced" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" LinkText="[toggle]" Collapsible="True" HeaderText="Advanced Options" BodyCssClass="Edit" Collapsed="true" HeaderTextCssClass="CollapsibleTitle">	
		<label class="Block">Original Image</label>
		<%# Image.OriginalFile %>
		<label class="Block">Thumbnail Image</label>
		<%# Image.ThumbNailFile %>
		<label class="Block">Resized Image</label>
		<%# Image.ResizedFile %>
	</ANW:AdvancedPanel>
</ANW:Page>

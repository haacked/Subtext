<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentImages" %>
<div id="aggrecentimages">
<h2>Latest Images</h2>
<asp:repeater id="recentImagesRepeater" runat="server">
    <ItemTemplate>
        <div>
            <asp:HyperLink runat="server" rel="lightbox" 
                ImageUrl='<%# ImageUrl(Container.DataItem) %>' 
                ToolTip='<%# H(GetImage(Container.DataItem).Title) %>' 
                NavigateUrl='<%# GalleryImageUrl(Container.DataItem) %>' />
            From 
            <asp:HyperLink runat="server" 
                Text='<%# H(GetImage(Container.DataItem).CategoryTitle) %>' 
                NavigateUrl='<%# GalleryUrl(Container.DataItem) %>' 
                ToolTip='<%# H(GetImage(Container.DataItem).Title) %>'  />
        </div>
    </ItemTemplate>
</asp:repeater>
</div>

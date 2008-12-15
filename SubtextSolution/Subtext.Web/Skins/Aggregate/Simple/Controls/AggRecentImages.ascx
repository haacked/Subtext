<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentImages" %>
<div id="aggrecentimages">
<h2>Latest Images</h2>
<asp:repeater id="RecentImages" runat="server">
	<ItemTemplate>
		<div>
            <asp:HyperLink ID="HyperLink2" runat="server" rel="lightbox" 
                ImageUrl='<%# GetImageUrl(Eval("CategoryID").ToString(), Eval("Blog.Host").ToString(), Eval("Blog.Subfolder").ToString(), Eval("FileName").ToString()) %>' 
                ToolTip='<%# Eval("Title") %>' 
                NavigateUrl='<%# GetImageLink(Eval("CategoryID").ToString(), Eval("Blog.Host").ToString(), Eval("Blog.Subfolder").ToString(), Eval("FileName").ToString()) %>' />
	From <asp:HyperLink ID="HyperLink1" runat="server" 
	        Text='<%# Eval("CategoryTitle")%>' 
	        NavigateUrl='<%# GetAlbumUrl(Eval("CategoryID").ToString(), Eval("Blog.Host").ToString(), Eval("Blog.Subfolder").ToString(), Eval("FileName").ToString()) %>' 
	        ToolTip='<%# Eval("Title") %>' />
		</div>
	</ItemTemplate>
</asp:repeater>
</div>

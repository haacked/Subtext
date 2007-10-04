<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentImages" %>
<div id="aggrecentimages">
<h2>Latest Images</h2>
<asp:repeater id="RecentImages" runat="server">
	<ItemTemplate>
		<div>
            <asp:HyperLink ID="HyperLink2" runat="server" rel="lightbox" ImageUrl='<%# GetImageUrl(DataBinder.Eval(Container.DataItem,"CategoryID").ToString(), DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString(), DataBinder.Eval(Container.DataItem,"ImageFile").ToString()) %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"ImageTitle") %>' NavigateUrl='<%# GetImageLink(DataBinder.Eval(Container.DataItem,"CategoryID").ToString(), DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString(), DataBinder.Eval(Container.DataItem,"ImageFile").ToString()) %>' />
	From <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryTitle")%>' NavigateUrl='<%# GetAlbumUrl(DataBinder.Eval(Container.DataItem,"CategoryID").ToString(), DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString(), DataBinder.Eval(Container.DataItem,"ImageFile").ToString()) %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"ImageTitle") %>' />
		</div>
	</ItemTemplate>
</asp:repeater>
</div>

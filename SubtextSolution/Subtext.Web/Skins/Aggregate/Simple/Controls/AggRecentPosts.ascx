<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentPosts" %>
<div id="aggrecentposts">
<h2>Latest Posts</h2>
<asp:repeater id="RecentPosts" runat="server">
	<ItemTemplate>
		<div class="post">
			<h3>
				<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetEntryUrl(Eval("Blog.Host").ToString(), Eval("Blog.Subfolder").ToString(), Eval("EntryName").ToString(), (DateTime)Eval("DateCreated")) %>' Text = '<%# Eval("Title") %>' ID="Hyperlink2"/></h3>
			<asp:Literal runat = "server" Text = '<%# Eval("Description") %>' ID="Label4"/>
			<p class="postfoot">
				posted @
				<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(Eval("DateCreated").ToString())).ToShortDateString() + " " + (DateTime.Parse(Eval("DateCreated").ToString())).ToShortTimeString() %>' ID="Label5" />
				by
				<asp:HyperLink Runat = "server" CssClass = "clsSubtext" NavigateUrl = '<%# GetFullUrl(Eval("Blog.Host").ToString(), Eval("Blog.Subfolder").ToString())  %>' Text = '<%# Eval("Author") %>' ID="Hyperlink3"/>
			</p>
		</div>
	</ItemTemplate>
</asp:repeater>
</div>

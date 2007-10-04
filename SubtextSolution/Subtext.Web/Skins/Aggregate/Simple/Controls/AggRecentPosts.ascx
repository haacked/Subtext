<%@ Control Language="C#" AutoEventWireup="true" Inherits="Subtext.Web.UI.Controls.AggRecentPosts" %>
<div id="aggrecentposts">
<h2>Latest Posts</h2>
<asp:repeater id="RecentPosts" runat="server">
	<ItemTemplate>
		<div class="post">
			<h3>
				<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetEntryUrl(DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString(), DataBinder.Eval(Container.DataItem,"EntryName").ToString(), (DateTime)DataBinder.Eval(Container.DataItem,"DateAdded")) %>' Text = '<%# DataBinder.Eval(Container.DataItem,"Title") %>' ID="Hyperlink2"/></h3>
			<asp:Literal runat = "server" Text = '<%# DataBinder.Eval(Container.DataItem,"Description") %>' ID="Label4"/>
			<p class="postfoot">
				posted @
				<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(DataBinder.Eval(Container.DataItem,"DateAdded").ToString())).ToShortDateString() + " " + (DateTime.Parse(DataBinder.Eval(Container.DataItem,"DateAdded").ToString())).ToShortTimeString() %>' ID="Label5" />
				by
				<asp:HyperLink Runat = "server" CssClass = "clsSubtext" NavigateUrl = '<%# GetFullUrl(DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString())  %>' Text = '<%# DataBinder.Eval(Container.DataItem,"Author") %>' ID="Hyperlink3"/>
			</p>
		</div>
	</ItemTemplate>
</asp:repeater>
</div>

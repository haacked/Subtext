<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.RecentComments" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div class="recentcomments">
	<div class="title">Recent Comments</div>
	<asp:Repeater ID="feedList" Runat="server" OnItemCreated="EntryCreated">
		   <HeaderTemplate>
		   </HeaderTemplate>
		   <ItemTemplate>
			   <div class="itemcomments">
					 <asp:HyperLink Runat="server" ID="Link" CssClass="commentlink"/>
					 &nbsp;<asp:Literal Runat="server" ID="Author" /><br />
				</div>
		   </ItemTemplate>
		   <FooterTemplate>
		   </FooterTemplate>
	</asp:Repeater>
</div>
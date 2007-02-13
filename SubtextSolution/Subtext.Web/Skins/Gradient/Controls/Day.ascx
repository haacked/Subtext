<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Import Namespace = "Subtext.Framework" %>
		<div class="dayPosts">
			<asp:HyperLink Runat="server" title="Day Archive" BorderWidth="0" ID="ImageLink" Visible="false"><asp:Literal ID="DateTitle" Runat="server" /></asp:HyperLink>

			<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
				<ItemTemplate>
					<div class="blogpost">
						<h2 class="postTitle"><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /><br /><asp:Label ID="postDate" CssClass="postTitleDate" runat="server" Format="MMM dd" /> </h2>
						<asp:Literal  runat="server" ID="PostText" />
						<p class="postfooter">
							<asp:Label id="permalink" runat="server" Format="MMM dd, yyyy" /> | <asp:Label id="commentCount" runat="server" />
						</p>
					</div> <!-- end #blogpost -->
				</ItemTemplate>
			</asp:Repeater>
		</div>
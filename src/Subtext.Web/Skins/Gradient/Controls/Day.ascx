<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>

		<div class="dayPosts">
			<asp:HyperLink Runat="server" title="Day Archive" BorderWidth="0" ID="ImageLink" Visible="false"><asp:Literal ID="DateTitle" Runat="server" /></asp:HyperLink>

			<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
				<ItemTemplate>
					<div class="blogpost">
						<h2 class="postTitle"><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" /> <asp:Label ID="postDate" CssClass="postTitleDate" runat="server" Format="MMM dd" /> </h2>
						<div class="post-content">
							<asp:Literal  runat="server" ID="PostText" />
							<p class="postfooter">
								<asp:Label id="permalink" runat="server" Format="MMM dd, yyyy" /> | <asp:Label id="commentCount" runat="server" />
							</p>
						</div>
					</div> <!-- end #blogpost -->
				</ItemTemplate>
			</asp:Repeater>
		</div>
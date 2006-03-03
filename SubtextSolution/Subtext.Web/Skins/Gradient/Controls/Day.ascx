<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Import Namespace = "Subtext.Framework" %>
		<div class="dayPosts">
			<h1 class="daylink"><asp:HyperLink Runat="server" title="Day Archive" BorderWidth="0" ID="ImageLink" ><asp:Literal ID = "DateTitle" Runat = "server" /></asp:HyperLink></h1>

			<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
				<ItemTemplate>
					<div class="blogpost">
						<h2 class="postTitle"><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h2>
						<asp:Literal  runat="server" ID="PostText" />
						<div class="postfooter">
							<ul class="postInfo">
								<li class="permalink"><asp:Label id="date" runat="server" Format="MMM dd, yyyy" /></li>
								<li class="commentCount"><asp:Label id="commentCount" runat="server" /></li>
							</ul>
						</div>
					</div> <!-- end #blogpost -->
				</ItemTemplate>
			</asp:Repeater>
		</div>
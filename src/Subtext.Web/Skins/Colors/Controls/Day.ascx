<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>

<asp:HyperLink Runat="server" Title="Day link" ImageUrl="~/images/link.gif" height="15" Width="12" BorderWidth="0" ID="ImageLink" Visible="false" />
<asp:Literal ID="DateTitle" Runat="server" Visible="false" />		  


<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
	<ItemTemplate>
		<div class="dropshadow">
			<div class="contentbox">
				<h2><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink  CssClass="posttitle" Runat="server" ID="TitleUrl" /></h2>
				<div class="content">
					<asp:Literal  runat="server" ID="PostText" />
					<div class="postinfo">			
						<asp:Literal ID="PostDesc" Runat = "server" />
					</div>
				</div>
			</div>
		</div>
	</ItemTemplate>
</asp:Repeater>
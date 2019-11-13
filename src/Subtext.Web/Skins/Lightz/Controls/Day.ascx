<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>

<h1 class="block_title">
	<asp:HyperLink Runat="server" Title = "Day link" ImageUrl="~/images/link.gif" height="15" Width="12" BorderWidth="0" ID="ImageLink" />
	<asp:Literal ID = "DateTitle" Runat = "server" />		  
</h1>

<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
	<HeaderTemplate>
		<div class = "block">
	</HeaderTemplate>
	<ItemTemplate>
			<div class="post">
				<div class="posttitle">
					<asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" />
				</div>
				<div class="postcontent">
					<asp:Literal  runat="server" ID="PostText" />
				</div>
				<div class="itemdesc"><asp:Literal ID = "PostDesc" Runat = "server" />
				</div>
			</div>
	</ItemTemplate>
	<SeparatorTemplate>
		<div class="seperator">&nbsp;</div>
	</SeparatorTemplate>
	<FooterTemplate>
			<div class="block_footer">&nbsp;</div>
		</div>
	</FooterTemplate>
</asp:Repeater>
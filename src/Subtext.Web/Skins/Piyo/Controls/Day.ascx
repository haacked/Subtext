<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>



<div class="entryDate">
<h3><asp:Literal ID = "DateTitle" Runat = "server" /> <asp:HyperLink Runat="server" height="15" Width="12" BorderWidth="0" ID="ImageLink">#</asp:HyperLink></h3>
</div>

<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
	<ItemTemplate>
		<div class="entry">

			<h4><asp:HyperLink Runat="server" ID="editLink" />  <asp:HyperLink Runat="server" ID="editInWlwLink" />  <asp:HyperLink  Runat="server" ID="TitleUrl" /></h4>
			
			<div class="post">
			<asp:Literal  runat="server" ID="PostText" />
			</div>
			
			<div class="info">
				<asp:Literal id="PostDesc"  runat="server" />
			</div>
		</div>
		<div class="clear"></div>
	</ItemTemplate>
</asp:Repeater>
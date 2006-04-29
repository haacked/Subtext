<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Import Namespace = "Subtext.Framework" %>


<div class="day">
	<div class="date">
		<asp:hyperlink runat="server" title="Day Archive" borderwidth="0" id="ImageLink" ><asp:literal id = "DateTitle" runat = "server" /></asp:hyperlink>
	</div>
	
	<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
		<ItemTemplate>
			<div class="post">
				<div class="title">
					<asp:hyperlink runat="server" id="TitleUrl" />
				</div>
				<div class="body">
					<asp:literal  runat="server" id="PostText" />
				</div>
				<div class="info">
					<asp:literal id = "PostDesc" runat = "server" />
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>

</div>
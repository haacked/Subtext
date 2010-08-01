<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>

	<dl class="Day">
		<dt>
			<asp:hyperlink runat="server" title="Day Archive" borderwidth="0" id="ImageLink" ><asp:literal id = "DateTitle" runat = "server" /></asp:hyperlink>
		</dt>
		<dd>
	<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
		<ItemTemplate>
			<dl class="Post">
				<dt>
					<asp:HyperLink  Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:hyperlink runat="server" id="TitleUrl" />
				</dt>
				<dd class="Text">
					<asp:literal  runat="server" id="PostText" />
				</dd>
				<dd class="Info">
					<asp:literal id = "PostDesc" runat = "server" />
				</dd>
			</dl>
		</ItemTemplate>
	</asp:Repeater>
		</dd>
	</dl>

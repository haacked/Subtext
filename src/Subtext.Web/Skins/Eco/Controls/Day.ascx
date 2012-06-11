<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Register TagPrefix="uc1" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>
<%@ Import Namespace = "Subtext.Framework" %>
	<div class="day">
		<div class="date">
			<asp:hyperlink runat="server" title="Day Archive" borderwidth="0" id="ImageLink" ><asp:literal id = "DateTitle" runat = "server" /></asp:hyperlink>
		</div>
	
	<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
		<ItemTemplate>
			<div class="post">
				<div class="title">
					<asp:HyperLink  Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:hyperlink runat="server" id="TitleUrl" />
				</div>
				<div class="body">
					<asp:literal  runat="server" id="PostText" />
				</div>
				<div class="info">
					<asp:literal id = "PostDesc" runat = "server" /> | <uc1:PostCategoryList id="Categories" runat="server"></uc1:PostCategoryList>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>

</div>
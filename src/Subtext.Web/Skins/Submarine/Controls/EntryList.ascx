<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>
<div class="posts">
	<div class="title">
		<asp:literal id="EntryCollectionTitle" runat="server" />
	</div>
	<div class="Description">
		<asp:literal id="EntryCollectionDescription" runat="server" />
	</div>
	<asp:repeater runat="server" id="Entries" onitemcreated="PostCreated">
		<itemtemplate>
			<div class="post">
				<div class="title">
					<asp:HyperLink  Runat="server" ID="editLink" /><asp:hyperlink runat="server" id="TitleUrl" />
				</div>
				<div class="body">
					<asp:literal runat="server" id="PostText" />
				</div>
				<div class="info">
					<asp:literal id="PostDesc" runat="server" />&nbsp;&nbsp;<img src="../images/feedback.gif" runat="server" align="middle">
				</div>
			</div>
		</itemtemplate>
	</asp:repeater>
	<div class="more">
		<asp:hyperlink runat="server" id="EntryCollectionReadMoreLink" />
	</div>
</div>

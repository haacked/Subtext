<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div class="posts">
	<div class="title">
		<asp:literal id="EntryCollectionTitle" runat="server" />
		</dt>
		<div class="Description">
			<asp:literal id="EntryCollectionDescription" runat="server" />
		</div>
		<asp:repeater runat="Server" runat="server" id="Entries" onitemcreated="PostCreated">
			<itemtemplate>
				<div class="post">
					<div class="title">
						<asp:hyperlink runat="server" id="TitleUrl" />
					</div>
					<div class="body">
						<asp:literal runat="server" id="PostText" />
					</div>
					<div class="info">
						<asp:literal id="PostDesc" runat="server" />
					</div>
				</div>
			</itemtemplate>
		</asp:repeater>
		<div class="more">
			<asp:hyperlink runat="server" id="EntryCollectionReadMoreLink" />
		</div>
	</div>

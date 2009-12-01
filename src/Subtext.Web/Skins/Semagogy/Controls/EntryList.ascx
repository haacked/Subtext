<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>

	<dl class="Posts">
		<dt>
			<asp:literal id="EntryCollectionTitle" runat="server" />
		</dt>
		<dd class="Description">
			<asp:literal id="EntryCollectionDescription" runat="server" />
		</dd>
		<dd>
		<asp:repeater runat="Server" runat="server" id="Entries" onitemcreated="PostCreated">
			<itemtemplate>
			<dl class="Post">
				<dt>
					<asp:HyperLink  Runat="server" ID="editLink" /><asp:hyperlink runat="server" id="TitleUrl" />
				</dt>
				<dd class="Text">
					<asp:literal  runat="server" id="PostText" />
				</dd>
				<dd class="Info">
					<asp:literal id = "PostDesc" runat = "server" />
				</dd>
			</dl>
			</itemtemplate>
		</asp:repeater>
		</dd>
		<dd class="More">
			<asp:hyperlink runat="server" id="EntryCollectionReadMoreLink" />
		</dd>
	</dl>		
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Register TagPrefix="st" TagName="ShareThisPost" Src="ShareThisPost.ascx" %>
<%@ Import Namespace = "Subtext.Framework" %>
	<div class="collectiontitle">
		<asp:literal id="EntryCollectionTitle" runat="server" /> <br />
		<asp:literal id="EntryCollectionDescription" runat="server" />
		<br />
	</div>
	<asp:Repeater runat="Server" ID="Entries" onitemcreated="PostCreated">
		<itemtemplate>
			<div class="post">
				<div class="title">
					<asp:HyperLink  Runat="server" ID="editLink" />&nbsp;
					<asp:hyperlink runat="server" id="TitleUrl" />
				</div>
				<div class="body">
					<asp:literal runat="server" id="PostText" />
				</div>
				<div class="info">
					<asp:Label id="commentCount" runat="server" CssClass="commentcount"></asp:Label><font color="#808080"><b></b></font> <asp:Label id="permalink" runat="server" CssClass="permalink"></asp:Label><font color="#808080"><b>&nbsp;| </b></font> <st:ShareThisPost id="shareOptions" runat="server"></st:ShareThisPost>
				</div>
			</div>
		</itemtemplate>
	</asp:repeater>
	<div class="more">
		<asp:hyperlink runat="server" id="EntryCollectionReadMoreLink" />
	</div>

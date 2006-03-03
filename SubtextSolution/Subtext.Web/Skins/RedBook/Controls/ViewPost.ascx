<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<div class="journal_eintrag">
	<h2><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink  CssClass="singleposttitle" Runat="server" ID="TitleUrl" /></h2>
	<asp:Literal id="Body"  runat="server" />

		<p class="postfooter">
			posted @ <asp:Literal id="PostDescription"  runat="server" />
		</p>
		</div>
	<asp:Literal ID = "TrackBack" Runat="server" />
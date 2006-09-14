<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="previousNext" Src="PreviousNext.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RelatedLinks" Src="RelatedLinks.ascx" %>
<%@ Register TagPrefix="st" TagName="ShareThisPost" Src="ShareThisPost.ascx" %>

<uc1:previousNext id="previousNext1" runat="server" />
<div class="post">
	<div class="title">
		<asp:HyperLink  Runat="server" ID="editLink" ImageUrl="~/skins/curvy/Images/report_edit.gif" CssClass="editlink" />&nbsp;<asp:hyperlink runat="server" id="TitleUrl" />
	</div>
	<div class="body">
		<asp:literal id="Body" runat="server" />
	</div>
	<div class="info">
		<asp:Literal id="PostDescription"  runat="server" /><font color="#808080"><b>&nbsp;| </b></font> <st:ShareThisPost id="shareOptions" runat="server"></st:ShareThisPost>
	</div>
	<uc1:RelatedLinks id="RelatedLinks1" runat="server" />
	<div class="trackback">
		<asp:literal id="PingBack" runat="server" />
		<asp:literal id="TrackBack" runat="server" />
	</div>
</div>

<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Register TagPrefix="st" TagName="ShareThisPost" Src="ShareThisPost.ascx" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div class="day">
	<div class="date">
		<asp:hyperlink runat="server" title="Day Archive" borderwidth="0" id="ImageLink" ><asp:literal id = "DateTitle" runat = "server" /></asp:hyperlink>
	</div>
	<asp:Repeater runat="Server" ID="DayList" OnItemCreated="PostCreated">
		<ItemTemplate>
			<div class="post">
				<div class="title">
					<asp:HyperLink  Runat="server" ID="editLink" CssClass="editlink" />&nbsp;<asp:hyperlink runat="server" id="TitleUrl" />
				</div>
				<div class="body">
					<asp:literal runat="server" id="PostText" />
				</div>
				<div class="info">
						<asp:Label id="commentCount" runat="server" CssClass="commentcount"></asp:Label><font color="#808080"><b></b></font>&nbsp;<asp:Label id="permalink" runat="server" CssClass="permalink"></asp:Label><font color="#808080"><b>&nbsp;| </b></font><st:ShareThisPost id="shareOptions" runat="server"></st:ShareThisPost>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
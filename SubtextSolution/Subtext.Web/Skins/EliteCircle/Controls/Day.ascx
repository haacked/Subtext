<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Register TagPrefix="st" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div class="day">
	<div class="date">
		<asp:hyperlink runat="server" title="Day Archive" borderwidth="0" id="ImageLink" ><asp:literal id = "DateTitle" runat = "server" /></asp:hyperlink>
	</div>
	
	<asp:Repeater runat="Server" ID="DayList" OnItemCreated="PostCreated">
		<ItemTemplate>
		
			<h1><asp:HyperLink  Runat="server" ID="editLink" /><asp:hyperlink runat="server" id="TitleUrl" /></h1>

			<asp:literal id="PostText" runat="server" />
			<p class="post-footer align-right">					
				<asp:Label id="commentCount" runat="server" />
				<a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
				<asp:Label id="postDate" runat="server" Format="HH:mm tt - MMM dd, yyyy" CssClass="date" />
			</p>
		</ItemTemplate>
	</asp:Repeater>

</div>
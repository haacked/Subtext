<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Import Namespace = "Subtext.Framework" %>
<h1 class = "datetitle">
	<asp:HyperLink Runat="server" Title = "Day link" ImageUrl="~/images/link.gif" height="15" Width="12" BorderWidth="0" ID="ImageLink" />

	<asp:Literal ID = "DateTitle" Runat = "server" />		  
</h1>

<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">
	<HeaderTemplate>
		<div class = "postlist">
	</HeaderTemplate>
	<ItemTemplate>
			<div class="posttitle">
				<asp:HyperLink  Runat="server" ID="editLink" /><asp:HyperLink  CssClass="posttitle" Runat="server" ID="TitleUrl" />
			</div>
			
			<asp:Literal  runat="server" ID="PostText" />
			
			<div class="itemdesc">			
				<asp:Literal ID = "PostDesc" Runat = "server" />
			</div>
	</ItemTemplate>
	<FooterTemplate>
		</div>
	</FooterTemplate>
</asp:Repeater>
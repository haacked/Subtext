<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Import Namespace = "Subtext.Framework" %>
<p class="date">
	<span>		  
	<asp:HyperLink Runat="server" Title = "Day Archive" BorderWidth="0" ID="ImageLink" ><asp:Literal ID = "DateTitle" Runat = "server" /></asp:HyperLink>
	</span>
</p>

<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">

	<ItemTemplate>
		<div class="post">
			<h2><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h2>
			<asp:Literal  runat="server" ID="PostText" />
			
			<p class="postfoot">		
			<asp:Literal ID = "PostDesc" Runat = "server" />
			</p>
		</div>
	</ItemTemplate>
</asp:Repeater>

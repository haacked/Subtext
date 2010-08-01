<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>
<%@ Register TagPrefix="uc1" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>

<p class="date">
	<span>		  
	<asp:HyperLink Runat="server" Title="Day Archive" BorderWidth="0" ID="ImageLink" ><asp:Literal ID = "DateTitle" Runat = "server" /></asp:HyperLink>
	</span>
</p>

<asp:Repeater runat="Server" Runat="server" ID="DayList" OnItemCreated="PostCreated">

	<ItemTemplate>
		<div class="post">
			<h2><asp:HyperLink Runat="server" ID="editLink" />  <asp:HyperLink Runat="server" ID="editInWlwLink" />  <asp:HyperLink Runat="server" ID="TitleUrl" /></h2>
			<asp:Literal  runat="server" ID="PostText" />
			
			<p class="postfoot">		
			<asp:Literal ID = "PostDesc" Runat = "server" /> | <uc1:PostCategoryList id="Categories" runat="server"></uc1:PostCategoryList>
			</p>
		</div>
	</ItemTemplate>
</asp:Repeater>

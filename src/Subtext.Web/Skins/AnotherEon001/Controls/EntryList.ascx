<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.EntryList" %>
<%@ Register TagPrefix="uc1" TagName="PostCategoryList" Src="PostCategoryList.ascx" %>

<h2><asp:Literal ID = "EntryCollectionTitle" Runat = "server" /></h2>
<asp:Literal ID = "EntryCollectionDescription" Runat = "server" />
<asp:Repeater runat="Server" Runat="server" ID="Entries" OnItemCreated="PostCreated">
	<ItemTemplate>
			<div class="post">
				<h5><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h5>
				
				<asp:Literal  runat="server" ID="PostText" />
				
				<p class="postfoot">		
					<asp:Literal ID = "PostDesc" Runat = "server" /> | <uc1:PostCategoryList id="Categories" runat="server"></uc1:PostCategoryList>
				</p>
			</div>
	</ItemTemplate>
</asp:Repeater>

<p>
<asp:HyperLink Runat="server" ID="EntryCollectionReadMoreLink" />
</p>

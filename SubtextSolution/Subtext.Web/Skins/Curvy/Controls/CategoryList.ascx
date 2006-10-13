<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.CategoryList" %>
<%@ Import Namespace = "Subtext.Framework" %>

<asp:Repeater ID="CatList" Runat="server" OnItemCreated="CategoryCreated">
	<ItemTemplate>
		<div class="title"><asp:literal runat="server" id="Title" />
		<a id="titleContent" 
			onclick="ToggleVisible('links',
									'LinkImage',
									'~/admin/resources/toggle_gray_up.gif',
									'/admin/resources/toggle_gray_down.gif'); return false;"
		href="#">
			
		<img id="LinkImage" 
			src="~/admin/resources/toggle_gray_down.gif" 
			style="border-width:0px;" />
		
		</div>
		<div class="links">
			<asp:repeater id="LinkList" runat="server" onitemcreated="LinkCreated">
				<headertemplate>
					<ul>
				</headertemplate>
				<itemtemplate>
					<li>
						<asp:HyperLink Runat = "server" ID = "RssLink" Text = "(rss)" Visible = "False"/>&nbsp;<asp:hyperlink runat="server" id="Link" />
					</li>
				</itemtemplate>
				<footertemplate>
					</ul>
				</footertemplate>
			</asp:repeater>
		</div>
	</ItemTemplate>
</asp:Repeater>
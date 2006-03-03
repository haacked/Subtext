<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.RecentComments" %>
<%@ Import Namespace = "Subtext.Framework" %>
<div id="recentComments">
<h3>Recent Comments</h3>
<asp:Repeater ID="feedList" Runat="server" OnItemCreated="EntryCreated">
       <HeaderTemplate>
              <ul>
       </HeaderTemplate>
       <ItemTemplate>
              <li><asp:HyperLink Runat="server" ID="Link" /><br /><asp:Literal Runat = "server" ID = "Author" /></li>
       </ItemTemplate>
       <FooterTemplate>
              </ul>
       </FooterTemplate>
</asp:Repeater>
</div>
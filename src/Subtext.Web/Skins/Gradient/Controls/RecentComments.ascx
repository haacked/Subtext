<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.RecentComments" %>
<%@ Import Namespace = "Subtext.Framework" %>
<asp:Repeater ID="feedList" Runat="server" OnItemCreated="EntryCreated">
       <HeaderTemplate>
              <ul>
       </HeaderTemplate>
       <ItemTemplate>
              <li><asp:HyperLink Runat="server" ID="Link" /></li>
       </ItemTemplate>
       <FooterTemplate>
              </ul>
       </FooterTemplate>
</asp:Repeater>
<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.RecentComments" %>

<h1>Recent Comments</h1>

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
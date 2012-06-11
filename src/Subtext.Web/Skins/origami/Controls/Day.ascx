<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Day" %>

<asp:HyperLink Runat="server" height="15" Width="12" BorderWidth="0" ID="ImageLink" Visible="false"></asp:HyperLink><asp:Literal ID = "DateTitle" Runat = "server" Visible="false" />
<asp:Repeater ID="DayList" runat="server" OnItemCreated="PostCreated">
    <ItemTemplate>
        <div class="post normal-post"> <asp:HyperLink ID="editLink" runat="server" Visible="false" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> 
            <h1 class="post-title"><asp:HyperLink  Runat="server" ID="TitleUrl" /></h1>
            <div class="post-content"><br/><asp:Literal  runat="server" ID="PostText" /></div>
            <p class="meta">author:&nbsp;<asp:Label ID="author" runat="server" />&nbsp;|&nbsp;<asp:Literal ID="PostDesc" runat="server" /><!--<br />Tags: <a href="#" rel="tag">Tag1</a>--></p>
        </div>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <div class="post alternate-post"> <asp:HyperLink ID="editLink" runat="server" Visible="false" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> 
            <h1 class="post-title"><asp:HyperLink  Runat="server" ID="TitleUrl" /></h1>
            <div class="post-content"><br/><asp:Literal  runat="server" ID="PostText" /></div>
            <p class="meta">author:&nbsp;<asp:Label ID="author" runat="server" />&nbsp;|&nbsp;<asp:Literal ID="PostDesc" runat="server" /><!--<br />Tags: <a href="#" rel="tag">Tag1</a>--></p>
        </div>
    </AlternatingItemTemplate>
</asp:Repeater>
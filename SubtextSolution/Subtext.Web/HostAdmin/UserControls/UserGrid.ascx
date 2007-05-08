<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserGrid.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.UserGrid" %>
<asp:GridView ID="usersGrid" runat="server" 
    AllowPaging="True" 
    AutoGenerateColumns="False"
    DataKeyNames="UserName" 
    DataSourceID="userDataSource" 
    AllowSorting="True">
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
    </Columns>
</asp:GridView>

<vt:MembershipUserDataSource ID="userDataSource" runat="server" />
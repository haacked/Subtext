<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersList.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.UsersList" %>

<asp:GridView ID="usersGrid" runat="server" 
    AllowPaging="True" 
    AutoGenerateColumns="False"
    DataKeyNames="UserName" 
    DataSourceID="userDataSource" 
    PageSize="10"
    AllowSorting="True">
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
    </Columns>
</asp:GridView>

<st:MembershipUserDataSource ID="userDataSource" runat="server" />
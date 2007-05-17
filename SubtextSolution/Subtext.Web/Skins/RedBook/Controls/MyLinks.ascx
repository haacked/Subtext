<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.MyLinks" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<ul>
	<li><asp:HyperLink Runat="server" NavigateUrl="~/Default.aspx" Text="Home" ID="HomeLink" /></li>
	<li><asp:HyperLink Runat="server" NavigateUrl="~/Archives.aspx" Text="Archives" ID="Archives" /></li>
	<li><asp:HyperLink AccessKey="9" Runat="server" NavigateUrl="~/Contact.aspx" Text="Contact" ID="ContactLink" />
</li>
    <li>
        <asp:LoginView ID="UserLoginView" runat="server">
            <RoleGroups>
                <asp:RoleGroup Roles="Administrators">
                    <ContentTemplate>
                        <asp:HyperLink Runat="server" Text="Admin" ID="AdminLogin" NavigateUrl="~/Admin/Default.aspx" />
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
            <LoggedInTemplate>
                <asp:LoginStatus ID="LoginStatus1" runat="server" />
            </LoggedInTemplate>
            <AnonymousTemplate>
                <asp:LoginStatus ID="LoginStatus2" runat="server" />
            </AnonymousTemplate>
        </asp:LoginView>
    </li>
</ul>

<!-- Not Visible -->
<asp:HyperLink Runat="server" ID="XMLLink" Visible="False"></asp:HyperLink>
<asp:HyperLink Runat="server" Text="Syndication" ID="Syndication" Visible="False" />

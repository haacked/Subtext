<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Login" %>
<!-- Begin: Login.ascx -->
<h3 class="listtitle">
    Sign In</h3>
<ul class="list">
    Username<br />
    <asp:TextBox CssClass="textbox" ID="tbUserName" runat="server" /><br />
    Password<br />
    <asp:TextBox CssClass="textbox" ID="tbPassword" TextMode="Password" runat="server" /><br />
    <asp:CheckBox ID="RememberMe" Checked="True" runat="server" />
    Remember Me<br />
    <asp:Button ID="btnLogin" CssClass="Button" runat="server" Text="Log In" CausesValidation="False" />
    &nbsp;<asp:Literal ID="Message" runat="server" />
</ul>
<!-- End: Login.ascx -->
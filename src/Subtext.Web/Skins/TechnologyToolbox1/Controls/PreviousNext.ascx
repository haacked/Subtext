<%@ Control Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PreviousNext" %>
<!-- Begin: PreviousNext.ascx -->
<asp:Label ID="LeftPipe" runat="server" class="prevNextSeparator" Visible="false"> | </asp:Label>
<asp:HyperLink ID="MainLink" runat="server" Visible="false">Home</asp:HyperLink>
<asp:Label ID="RightPipe" runat="server" class="prevNextSeparator" Visible="false"> | </asp:Label>
<ul class="sb-tools clearfix">
    <li class="previous-post">
        <asp:HyperLink ID="PrevLink" runat="server" ToolTip="previous post" Format="Previous Entry: {0}"
            rel="prev" /></li>
    <li class="next-post">
        <asp:HyperLink ID="NextLink" runat="server" ToolTip="next post" Format="Next Entry: {0}"
            rel="next" /></li>
</ul>
<!-- End: PreviousNext.ascx -->
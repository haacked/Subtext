<%@ Import Namespace="Subtext.Framework.Components" %>
<%@ Control Language="C#" EnableTheming="false" Inherits="Subtext.Web.UI.Controls.DayCollection" %>
<%@ Register TagPrefix="uc1" TagName="Day" Src="Day.ascx" %>
<!-- Begin: DayCollection.ascx -->
<asp:Repeater ID="DaysList" runat="server">
    <HeaderTemplate>
        <div class="hfeed">
    </HeaderTemplate>
    <ItemTemplate>
        <uc1:Day runat="server" ID="DayItem" CurrentDay='<%# (EntryDay) Container.DataItem %>' />
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>
<!-- End: DayCollection.ascx -->

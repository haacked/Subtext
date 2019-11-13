<%@ Import Namespace = "Subtext.Framework.Components" %>
<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.DayCollection" %>
<%@ Register TagPrefix="uc1" TagName="Day" Src="Day.ascx" %>
<asp:Repeater id="DaysList" runat="server">
	<ItemTemplate>
		<uc1:Day id="DayItem" CurrentDay='<%# Container.DataItem %>' runat="server" />
	</ItemTemplate>
</asp:Repeater>

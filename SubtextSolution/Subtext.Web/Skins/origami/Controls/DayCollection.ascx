<%@ Import Namespace = "Subtext.Framework.Components" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.DayCollection" %>
<%@ Register TagName="Day" TagPrefix="origami" Src="Day.ascx" %>
<asp:Repeater id="DaysList" runat="server">
	<ItemTemplate>
		<origami:Day id="DayItem" CurrentDay = '<%# (EntryDay) Container.DataItem %>' runat="server" />
	</ItemTemplate>
</asp:Repeater>
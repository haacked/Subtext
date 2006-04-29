<%@ Import Namespace = "Subtext.Framework.Components" %>
<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.DayCollection" %>
<%@ Register TagPrefix="uc1" TagName="Day" Src="Day.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Top10Module" Src="Top10Module.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentComments" Src="RecentComments.ascx" %>

<table border="0" width="81%" id="table1" style="border-collapse: collapse" cellspacing="5">
	<tr>
		<td>
			<uc1:Top10Module id="Top10Module1" runat="server"></uc1:Top10Module>
		</td>
		<td valign="top">
			<uc1:RecentComments id="RecentComments1" runat="server"></uc1:RecentComments>
		</td>
	</tr>
</table>
<asp:Repeater id="DaysList" runat="server">
	<ItemTemplate>
		<uc1:Day id="DayItem" CurrentDay='<%# (EntryDay) Container.DataItem %>' runat="server" />
	</ItemTemplate>
</asp:Repeater>

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

<div id="ad" align="center">
	<div id="adtop">
		<br />
	</div>
		<script type="text/javascript"><!--
		google_ad_client = "pub-4807607358197473";
		google_ad_width = 468;
		google_ad_height = 60;
		google_ad_format = "468x60_as";
		google_ad_type = "text_image";
		google_ad_channel ="";
		google_color_border = "FF4500";
		google_color_bg = "FFEBCD";
		google_color_link = "DE7008";
		google_color_url = "E0AD12";
		google_color_text = "8B4513";
		//--></script>
		<script type="text/javascript"
		  src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
</div>
<asp:Repeater id="DaysList" runat="server">
	<ItemTemplate>
		<uc1:Day id="DayItem" CurrentDay='<%# (EntryDay) Container.DataItem %>' runat="server" />
	</ItemTemplate>
</asp:Repeater>

<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RelatedLinks" Src="RelatedLinks.ascx" %>
<uc1:PreviousNext id="PreviousNext1" runat="server" />
<div class="post">
	<div class="title">
		<asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" />
	</div>
	<div class="body">
		<asp:Literal id="Body"  runat="server" />
	</div>
	<div class="info">
		posted @ <asp:Literal id="PostDescription" runat="server" />
	</div>
	<uc1:RelatedLinks id="RelatedLinks1" runat="server" />
	<div align="center" class="ado">
		<div class="adtopo">
			<br />
		</div>
		<script type="text/javascript">
			<!--
			google_ad_client = "pub-4807607358197473";
			google_ad_width = 468;
			google_ad_height = 60;
			google_ad_format = "468x60_as";
			google_ad_type = "text_image";
			google_ad_channel ="";
			google_color_border = "B4D0DC";
			google_color_bg = "ECF8FF";
			google_color_link = "0000CC";
			google_color_url = "008000";
			google_color_text = "6F6F6F";
			//-->
		</script>
		<script type="text/javascript"
			src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
	</div>
</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />

	
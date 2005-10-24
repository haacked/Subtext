<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
	<div class="blogpost">
		<h2><asp:HyperLink Runat="server" ID="editLink" /><span class="title"><asp:HyperLink Runat="server" ID="TitleUrl" /></span></h2>
		<asp:Literal id="Body"  runat="server" />
		<p class="postfooter">
			posted on <asp:Literal id="PostDescription"  runat="server" />
		</p>
	</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />
	
	<div style="float:left;">
		<script type="text/javascript"><!--
			google_ad_client = "pub-7694059317326582";
			google_ad_width = 250;
			google_ad_height = 250;
			google_ad_format = "250x250_as";
			google_ad_type = "text_image";
			google_ad_channel ="9445702549";
			google_color_border = "FFFFFF";
			google_color_bg = "FFFFFF";
			google_color_link = "444444";
			google_color_url = "BF5010";
			google_color_text = "444444";
			//--></script>
			<script type="text/javascript"
			src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
	</div>
	<div style="float:right;">
		<script type="text/javascript"><!--
			google_ad_client = "pub-7694059317326582";
			google_ad_width = 250;
			google_ad_height = 250;
			google_ad_format = "250x250_as";
			google_ad_type = "text_image";
			google_ad_channel ="9445702549";
			google_color_border = "FFFFFF";
			google_color_bg = "FFFFFF";
			google_color_link = "444444";
			google_color_url = "BF5010";
			google_color_text = "444444";
			//--></script>
			<script type="text/javascript"
			src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
	</div>
	<div style="clear:both;"></div>
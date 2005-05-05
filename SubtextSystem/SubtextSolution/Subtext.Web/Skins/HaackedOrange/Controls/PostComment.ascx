<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PostComment" %>
<div id="commentform">
<h3>Post Comment</h3>
	<table cellspacing="1" cellpadding="1" border="0">
		<tr>
			<td>Title</td>
			<td>
				<asp:TextBox id="tbTitle" runat="server" size="40" CssClass="Textbox"></asp:TextBox>
			</td>
			<td>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a title"
					ControlToValidate="tbTitle"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Name</td>
			<td>
				<asp:TextBox id="tbName" runat="server" Size="40" Width="300px" CssClass="Textbox"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your name"
					ControlToValidate="tbName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>Url</td>
			<td>
				<asp:TextBox id="tbUrl" runat="server" Size="40" Width="300px" CssClass="Textbox"></asp:TextBox>
			</td>
			<td></td>
		</tr>
		<tr>
			<td colSpan="3">Comment&nbsp;
				<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter a comment"
					ControlToValidate="tbComment"></asp:RequiredFieldValidator><BR>
				<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" 
					TextMode="MultiLine"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<asp:CheckBox id="chkRemember" runat="server" Text="Remember Me?" Checked="True"></asp:CheckBox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button id="btnSubmit" CssClass="Button" runat="server" Text="Submit"></asp:Button>
			</td>
			<td colspan="2">
				<asp:Label id="Message" runat="server" ForeColor="Red"></asp:Label>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<div id="linkadwrap">
					<p class="ad">
						<script type="text/javascript"><!--
							google_ad_client = "pub-7694059317326582";
							google_ad_width = 200;
							google_ad_height = 90;
							google_ad_format = "200x90_0ads_al";
							google_ad_channel ="6041353922";
							google_color_border = "FFFFFF";
							google_color_bg = "FFFFFF";
							google_color_link = "444444";
							google_color_url = "BF5010";
							google_color_text = "444444";

						//--></script>
						<script type="text/javascript"
						src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
						</script>
					</p>
				</div>
			</td>
		</tr>
	</table>
</div>

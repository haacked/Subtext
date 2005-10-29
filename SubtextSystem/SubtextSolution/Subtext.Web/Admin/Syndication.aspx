<%@ Page language="c#" Codebehind="Syndication.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Syndication" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" Categorieslabel="Other Items">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Edit" runat="server" Collapsible="False" HeaderText="Syndication" HeaderCssClass="CollapsibleHeader"
		BodyCssClass="Edit" DisplayHeader="true">
		<p>
			<asp:CheckBox id="chkEnableSyndication" runat="server" Text="Enable Syndication" />
			<SP:HelpToolTip id="HelpToolTip1" runat="server" 
					HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don\'t know what RSS is, please read the following <a href=\'http://slate.msn.com/id/2096660/\'>introduction</a>.">
				<img src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" id="helpImg" RunAt="Server" />
			</SP:HelpToolTip>
		</p>
		<div id="otherSettings">
			<P class="Valuelabel">
				<asp:CheckBox id="chkUseSyndicationCompression" runat="server" Text="Enable Compression"></asp:CheckBox>
				<SP:HelpToolTip id="HelpToolTip2" runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off."><img src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" id="Img1" RunAt="Server" /></SP:HelpToolTip>
			</P>
			<p class="Valuelabel">
				<asp:CheckBox id="chkUseDeltaEncoding" runat="server" Text="Enable Delta Encoding"></asp:CheckBox>
				<SP:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, your feeds will employ RFC3229 Delta Encoding.  This can save on bandwidth usage for aggregators that support this protocol.  It simply sends only the feed items that have changed since the last request."><img src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" id="Img2" RunAt="Server" /></SP:HelpToolTip>
			</p>
			<label for="Edit_txtLicenseUrl" AccessKey="l">
				<u>L</u>icense 
			</label>
			<asp:TextBox id="txtLicenseUrl" runat="server" />
			<SP:HelpToolTip id="HelpToolTip3" runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href=\'http://backend.userland.com/creativeCommonsRssModule\'>spec here</a>.">
					<img src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" id="Img3" RunAt="Server" />
			</SP:HelpToolTip>
		</div>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Save"></asp:Button>
		</div>
	</ANW:AdvancedPanel>
</ANW:Page>

<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Syndication.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Syndication" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" Categorieslabel="Other Items">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Edit" runat="server" DisplayHeader="true" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
		HeaderText="Syndication" Collapsible="False">
		<P>
			<asp:CheckBox id="chkEnableSyndication" runat="server" Text="Enable Syndication"></asp:CheckBox>
			<SP:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don\'t know what RSS is, please read the following <a href=\'http://slate.msn.com/id/2096660/\'>introduction</a>.">
				<IMG id="helpImg" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server">
			</SP:HelpToolTip></P>
		<DIV id="otherSettings">
			<P class="Valuelabel">
				<asp:CheckBox id="chkUseSyndicationCompression" runat="server" Text="Enable Compression"></asp:CheckBox>
				<SP:HelpToolTip id="HelpToolTip2" runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off.">
					<IMG id="Img1" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server"></SP:HelpToolTip>
			</P>
			<P class="Valuelabel">
				<asp:CheckBox id="chkUseDeltaEncoding" runat="server" Text="Enable Delta Encoding"></asp:CheckBox>
				<SP:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, your feeds will employ RFC3229 Delta Encoding.  This can save on bandwidth usage for aggregators that support this protocol.  It simply sends only the feed items that have changed since the last request.">
					<IMG id="Img2" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server"></SP:HelpToolTip>
			</P>
			<LABEL accessKey="l" for="Edit_txtLicenseUrl"><U>L</U>icense </LABEL>
			<asp:TextBox id="txtLicenseUrl" runat="server"></asp:TextBox>
			<SP:HelpToolTip id="HelpToolTip3" runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href=\'http://backend.userland.com/creativeCommonsRssModule\'>spec here</a>.">
				<IMG id="Img3" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server">
			</SP:HelpToolTip></DIV>
		<DIV>
			<asp:Button id="lkbPost" runat="server" Text="Save" CssClass="buttonSubmit"></asp:Button></DIV>
	</ANW:AdvancedPanel>
</ANW:Page>

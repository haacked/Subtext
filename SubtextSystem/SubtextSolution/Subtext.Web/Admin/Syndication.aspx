<%@ Page language="c#" Codebehind="Syndication.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Syndication" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" Categorieslabel="Other Items">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Edit" runat="server" Collapsible="False" HeaderText="Syndication" HeaderCssClass="CollapsibleHeader"
		BodyCssClass="Edit" DisplayHeader="true">
		<P class="ValueLabel"><LABEL for="chkEnableSyndication">
				<SP:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don\'t know what RSS is, please read the following <a href=\'http://slate.msn.com/id/2096660/\'>introduction</a>.">Enable 
Syndication</SP:HelpToolTip>
			</LABEL>
			<asp:CheckBox id="chkEnableSyndication" runat="server"></asp:CheckBox></P>
		<DIV id="otherSettings">
			<P class="ValueLabel"><LABEL for="chkUseSyndicationCompression">
					<SP:HelpToolTip id="HelpToolTip2" runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off.">Enable 
Compression</SP:HelpToolTip>
				</LABEL>
				<asp:CheckBox id="chkUseSyndicationCompression" runat="server"></asp:CheckBox></P>
			<LABEL class="Block">
				<SP:HelpToolTip id="HelpToolTip3" runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href=\'http://backend.userland.com/creativeCommonsRssModule\'>spec here</a>.">License 
Url</SP:HelpToolTip>
			</LABEL>
			<asp:TextBox id="txtLicenseUrl" runat="server" width="400"></asp:TextBox></DIV>
		<DIV style="MARGIN-TOP: 8px">
			<asp:linkbutton id="lkbPost" runat="server" CssClass="Button" Text="Save"></asp:linkbutton><BR>
			&nbsp;
		</DIV>
	</ANW:AdvancedPanel>
</ANW:Page>

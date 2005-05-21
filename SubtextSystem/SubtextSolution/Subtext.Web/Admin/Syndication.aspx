<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Syndication.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Syndication" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" Categorieslabel="Other Items">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Edit" runat="server" DisplayHeader="true" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
		HeaderText="Syndication" Collapsible="False">
		<p class="ValueLabel">
			<label for="chkEnableSyndication">
				<SP:HelpToolTip runat="server" HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don\'t know what RSS is, please read the following <a href=\'http://slate.msn.com/id/2096660/\'>introduction</a>.">Enable Syndication</SP:HelpToolTip>
			</label>
			<asp:CheckBox id="chkEnableSyndication" runat="server"></asp:CheckBox>
		</p>
		<div id="otherSettings">
			<p class="ValueLabel"><label for="chkUseSyndicationCompression">
				<SP:HelpToolTip runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off.">Enable Compression</SP:HelpToolTip>
				</label>
				<asp:CheckBox id="chkUseSyndicationCompression" runat="server"></asp:CheckBox>
			</p>
			
			<label class="Block"><SP:HelpToolTip runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href=\'http://backend.userland.com/creativeCommonsRssModule\'>spec here</a>.">License Url</SP:HelpToolTip></label>
			<asp:TextBox id="txtLicenseUrl" runat="server" width="400"></asp:TextBox>
		</div>
		
		<div style="MARGIN-TOP: 8px">
			<asp:linkbutton id="lkbPost" runat="server" Text="Save" CssClass="Button"></asp:linkbutton><br />
			&nbsp;
		</div>
		
	</ANW:AdvancedPanel>
</ANW:Page>

<%@ Page language="c#" Title="Subtext Admin - Syndication" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Syndication.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Syndication" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="syndicationContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<st:AdvancedPanel id="Edit" runat="server" DisplayHeader="true" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
		HeaderText="Syndication" Collapsible="False">
		<p>
			<asp:CheckBox id="chkEnableSyndication" runat="server" Text="Enable Syndication"></asp:CheckBox>
			<st:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don\'t know what RSS is, please read the following <a href=\'http://slate.msn.com/id/2096660/\'>introduction</a>.">
				<img id="helpImg" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server" />
			</st:HelpToolTip>
		</p>
		<div id="otherSettings">
			<p class="Valuelabel">
				<asp:CheckBox id="chkUseSyndicationCompression" runat="server" Text="Enable Compression"></asp:CheckBox>
				<st:HelpToolTip id="HelpToolTip2" runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off.">
					<img id="Img1" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server" /></st:HelpToolTip>
			</p>
			<p class="Valuelabel">
				<asp:CheckBox id="chkUseDeltaEncoding" runat="server" Text="Enable Delta Encoding"></asp:CheckBox>
				<st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, your feeds will employ RFC3229 Delta Encoding.  This can save on bandwidth usage for aggregators that support this protocol.  It simply sends only the feed items that have changed since the last request.">
					<img id="Img2" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server" /></st:HelpToolTip>
			</p>
			<label accessKey="l" for="Edit_txtLicenseUrl"><u>L</u>icense </label>
			<asp:TextBox id="txtLicenseUrl" runat="server"></asp:TextBox>
			<st:HelpToolTip id="HelpToolTip3" runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href=\'http://backend.userland.com/creativeCommonsRssModule\'>spec here</a>.">
				<img id="Img3" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" RunAt="Server" />
			</st:HelpToolTip>
		</div>
		<div>
			<asp:Button id="lkbPost" runat="server" Text="Save" CssClass="buttonSubmit" onclick="lkbPost_Click"></asp:Button>
		</div>
	</st:AdvancedPanel>
</asp:Content>
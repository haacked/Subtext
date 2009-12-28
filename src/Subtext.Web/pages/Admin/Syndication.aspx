<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Syndication" Codebehind="Syndication.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Syndication" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="syndicationContent" ContentPlaceHolderID="pageContent" runat="server">
    <h2>Syndication</h2>
	<st:MessagePanel id="Messages" runat="server" />
	<fieldset class="edit-form">
	    <legend>Feed Settings</legend>
	    <span class="checkbox">
		    <asp:CheckBox id="chkEnableSyndication" runat="server" Text="Enable Syndication" />
		    <st:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, This will turn on or off your RSS (or ATOM) feed.  If you don't know what RSS is, please read the following <a href='http://slate.msn.com/id/2096660/'>introduction</a>.">
			    <img id="helpImg" src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information"/>
		    </st:HelpToolTip>
		</span>
		<span class="checkbox">
		    <asp:CheckBox id="chkUseSyndicationCompression" runat="server" Text="Enable Compression" />
		    <st:HelpToolTip id="HelpToolTip2" runat="server" HelpText="If checked, your feeds will be compressed for clients that indicate they are able to receive compressed feeds.  This could reduce your bandwidth usage. If you encounter problems with your feed, try turning this off.">
			    <img id="Img1" src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information"/></st:HelpToolTip>
		</span>
		<span class="checkbox">
		    <asp:CheckBox id="chkUseDeltaEncoding" runat="server" Text="Enable Delta Encoding" />
		    <st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, your feeds will employ RFC3229 Delta Encoding.  This can save on bandwidth usage for aggregators that support this protocol.  It simply sends only the feed items that have changed since the last request.">
			    <img id="Img2" src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information"/></st:HelpToolTip>
        </span>
        		
		<label accessKey="f" for="Edit_txtFeedBurnerName"><u>F</u>eedBurner Name or URL<st:HelpToolTip id="hlpFeedburner" runat="server" 
			HelpText="<p>Entering your FeedBurner feed name will redirect your main feed to use <a href='http://feedburner.com/' title='feedburner'>Feedburner</a>.  The URL of your feed will become: <strong>http://feeds.feedburner.com/<i>FEED-BURNER-NAME</i></strong>.</p><p>If you are using the MyBrand service, then enter <strong>your full feedburner URL</strong>.</p>">
			<img id="Img4" src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information"/>
		</st:HelpToolTip></label>
		<asp:TextBox id="txtFeedBurnerName" runat="server" />
		
	
		<label accessKey="l" for="Edit_txtLicenseUrl"><u>L</u>icense<st:HelpToolTip id="HelpToolTip3" runat="server" HelpText="If specifed, indicates that your RSS feed is available under a license using the creativeCommons:license element. This can be used to display any license. For more information, read the <a href='http://backend.userland.com/creativeCommonsRssModule'>spec here</a>.">
			<img id="Img3" src="~/images/icons/ms_information_small.gif" runat="Server" alt="Information" />
		</st:HelpToolTip></label>
		<asp:TextBox id="txtLicenseUrl" runat="server" />
		
	</fieldset>

	<div>
		<asp:Button id="lkbPost" runat="server" Text="Save" CssClass="buttonSubmit" onclick="lkbPost_Click" />
	</div>
</asp:Content>
<%@ Page language="c#" Title="Subtext Admin - Comment Options" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Comments.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Comments" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    Actions
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
    <st:AdvancedPanel id="Edit" runat="server" DisplayHeader="true" BodyCssClass="Edit" HeaderCssClass="CollapsibleHeader"
	    HeaderText="Comments and Trackbacks" Collapsible="False">
	    <p class="Valuelabel"><label for="chkEnableComments">
			    <st:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, enables comments.">Enable Comments</st:HelpToolTip>
		    </label>
		    <asp:CheckBox id="chkEnableComments" runat="server"></asp:CheckBox>
	    </p>
	    <p class="Valuelabel"><label for="chkEnableCommentModeration">
			    <st:HelpToolTip id="helpCommentModeration" runat="server" HelpText="If checked, enables comment moderation.  Does nothing if comments are not enabled.">Enable Comment Moderation</st:HelpToolTip>
		    </label>
		    <asp:CheckBox id="chkEnableCommentModeration" runat="server"></asp:CheckBox>
	    </p>
	    <p class="Valuelabel"><label for="chkCoCommentEnabled">
			    <st:HelpToolTip id="Helptooltip6" runat="server" HelpText="If checked, enables CoComment support.">Enable CoComment Support.</st:HelpToolTip>
		    </label>
		    <asp:CheckBox id="chkCoCommentEnabled" runat="server"></asp:CheckBox>
	    </p>
	    <p class="Valuelabel"><label for="chkEnableTrackbacks">
			    <st:HelpToolTip id="Helptooltip5" runat="server" HelpText="If checked, enables trackbacks and pingbacks.">Enable TrackBacks</st:HelpToolTip>
		    </label>
		    <asp:CheckBox id="chkEnableTrackbacks" runat="server"></asp:CheckBox>
	    </p>
	    <div id="otherSettings">
		    <p class="Valuelabel"><label for="txtCommentDelayIntervalMinutes">
				    <st:HelpToolTip id="HelpToolTip2" runat="server" HelpText="Enter the number of minutes the delay between comments originating from the same source should be.  This helps prevent spam bombing attacks via automated scripts.">Comment Delay In Minutes</st:HelpToolTip>
			    </label>
			    <asp:TextBox id="txtCommentDelayIntervalMinutes" runat="server" Columns="2"></asp:TextBox></p>
		    <label class="Block">
			    <st:HelpToolTip id="Helptooltip3" runat="server" HelpText="If Comments are enabled, this setting allows you to specify whether comments will be disallowed on a post after a certain number of days.  For example, you may wish to have comments close on an item after 30 days.">Number 
			    of Days To Wait Before Comments Are Closed </st:HelpToolTip>(leave blank if 
			    comments never close) 
		    </label>
		    <asp:TextBox id="txtDaysTillCommentsClosed" runat="server" Columns="2"></asp:TextBox></div>
		    <p class="Valuelabel">
			    <label for="chkAllowDuplicates">
			    <st:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, duplicate comments are allowed.  If unchecked, duplicate comments are not allowed.  Not checking this can help prevent some comment spam, but at the cost that short “me too” style comments may be blocked.">Allow Duplicate Comments </st:HelpToolTip>
		    </label>
		    <asp:CheckBox id="chkAllowDuplicates" runat="server"></asp:CheckBox>
		    <p class="Valuelabel"><label class="txtNumberOfRecentComments">
				    <st:HelpToolTip id="Helptooltip7" runat="server" HelpText="This sets how many recent comments are displayed in the sidebar. This is an integer from 1-99.">Number of Recent Comments to Display </st:HelpToolTip>
			    </label>
			    <asp:TextBox id="txtNumberOfRecentComments" runat="server" Columns="2"></asp:TextBox>
		    </p>
		    <p class="Valuelabel"><LABEL class="txtRecentCommentsLength">
                <st:HelpToolTip id="Helptooltip8" runat="server" HelpText="This controls how many characters of recent comments are displayed in the sidebar. This is an integer from 1-99.">Length of Recent Comments to Display (Number of characters)</st:HelpToolTip></LABEL>
			    <asp:TextBox id="txtRecentCommentsLength" runat="server" Columns="2"></asp:TextBox></p>
			</p>
	    <div style="MARGIN-TOP: 8px">
		    <asp:Button id="lkbPost" runat="server" Text="Save" CssClass="buttonSubmit"></asp:Button>&nbsp;
	    </div>
    </st:AdvancedPanel>
</asp:Content>
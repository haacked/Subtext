<%@ Page language="c#" Codebehind="Comments.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Comments" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options" Categorieslabel="Other Items">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Edit" runat="server" Collapsible="False" HeaderText="Comments and Trackbacks"
		HeaderCssClass="CollapsibleHeader" BodyCssClass="Edit" DisplayHeader="true">
		<P class="Valuelabel"><label for="chkEnableComments">
				<SP:HelpToolTip id="HelpToolTip1" runat="server" HelpText="If checked, enables comments.">
					Enable Comments 
				</SP:HelpToolTip>
			</label>
			<asp:CheckBox id="chkEnableComments" runat="server"></asp:CheckBox></P>
		<P class="Valuelabel"><label for="chkEnableTrackbacks">
				<SP:HelpToolTip id="Helptooltip5" runat="server" HelpText="If checked, enables trackbacks and pingbacks.">Enable TrackBacks 
</SP:HelpToolTip>
			</label>
			<asp:CheckBox id="chkEnableTrackbacks" runat="server"></asp:CheckBox></P>
		<div id="otherSettings">
			<P class="Valuelabel"><label for="txtCommentDelayIntervalMinutes">
					<SP:HelpToolTip id="HelpToolTip2" runat="server" HelpText="Enter the number of minutes the delay between comments originating from the same source should be.  This helps prevent spam bombing attacks via automated scripts.">
						Comment Delay In Minutes 
					</SP:HelpToolTip>
				</label>
				<asp:TextBox id="txtCommentDelayIntervalMinutes" runat="server" Columns="2"></asp:TextBox></P>
			<label class="Block">
				<SP:HelpToolTip id="Helptooltip3" runat="server" HelpText="If Comments are enabled, this setting allows you to specify whether comments will be disallowed on a post after a certain number of days.  For example, you may wish to have comments close on an item after 30 days.">Number 
				of Days To Wait Before Comments Are Closed </SP:HelpToolTip>(leave blank if 
				comments never close) </label>
			<asp:TextBox id="txtDaysTillCommentsClosed" runat="server" Columns="2"></asp:TextBox></div>
		<P class="Valuelabel"><label for="chkAllowDuplicates">
				<SP:HelpToolTip id="Helptooltip4" runat="server" HelpText="If checked, duplicate comments are allowed.  If unchecked, duplicate comments are not allowed.  Not checking this can help prevent some comment spam, but at the cost that short “me too” style comments may be blocked.">Allow 
Duplicate Comments </SP:HelpToolTip>
			</label>
			<asp:CheckBox id="chkAllowDuplicates" runat="server"></asp:CheckBox></P>
		<div style="MARGIN-TOP: 8px">
			<asp:linkbutton id="lkbPost" runat="server" CssClass="Button" Text="Save"></asp:linkbutton><br />
			&nbsp;
		</div>
	</ANW:AdvancedPanel>
</ANW:Page>

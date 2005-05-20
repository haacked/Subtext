<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:Repeater ID="blogsList" Runat="server">
	<HeaderTemplate>
		<table class="Listing" cellSpacing="0" cellPadding="0" border="0">
			<tr>
				<th>Title</th>
				<th width="50">Subt</th>
				<th width="75">Web Views</th>
				<th width="75">Agg Views</th>
				<th width="50">Referrals</th>
				<th width="50">&nbsp;</th>
				<th width="50">&nbsp;</th>
			</tr>
	</HeaderTemplate>
</asp:Repeater>
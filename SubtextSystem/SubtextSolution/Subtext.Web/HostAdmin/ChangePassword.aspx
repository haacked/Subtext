<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ChangePassword.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ChangePassword" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Change Host Admin Password</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Change Host Admin Password</MP:ContentRegion>
	<p>Use this page to change the HostAdmin password..</p>
	
	<div class="form">
		<label for="txtCurrentPassword">Current Password</label>
		<asp:TextBox id="txtCurrentPassword" runat="server" />
	</div>

</MP:MasterPage>

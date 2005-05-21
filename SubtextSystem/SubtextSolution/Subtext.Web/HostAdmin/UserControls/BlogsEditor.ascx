<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<ANW:MessagePanel id="messagePanel" runat="server">
</ANW:MessagePanel>
<ANW:AdvancedPanel id="pnlResults" runat="server">
	<asp:CheckBox id="chkShowInactive" Runat="server" Text="Show Inactive Blogs" AutoPostBack="True"></asp:CheckBox>
	<asp:Repeater id="rprBlogsList" Runat="server" OnItemCommand="rprBlogsList_ItemCommand">
		<HeaderTemplate>
			<table class="Listing" cellSpacing="0" cellPadding="4" border="0">
				<tr>
					<th>
						Title</th>
					<th>
						Host</th>
					<th>
						Application</th>
					<th>
						Active
					</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Application") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlogID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="Linkbutton1" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlogID") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="Alt">
				<td>
					<%# DataBinder.Eval(Container.DataItem, "BlogID") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Application") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEditAlt" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlogID") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlogID") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<P id="lblNoMessages" runat="server" visible="false">No entries found.</P>
	<ANW:Pager id="resultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>"
		LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="Default.aspx?pg={0}"
		CssClass="Pager"></ANW:Pager>
</ANW:AdvancedPanel>
<ANW:AdvancedPanel id="pnlEdit" runat="server">
	<script type="text/javascript">
		// These are variables used to hold the various help messages.

		var hostDomainHelp = '<p><strong>Host Domain</strong> is the domain name for this blog. '
			+ 'If you never plan on setting up another blog on this server, then you do not have ' 
			+ 'to worry about this setting.  However, if you decide to add another blog at a later '
			+ 'time, it&#8217;s important to update this setting for your initial blog.</p>'
			+ '<p>For example, if you are hosting this blog at http://www.example.com/, the Host Domain '
			+ 'would be &#8220;www.example.com&#8221;.</p><p>If you are trying to set this up on your ' 
			+ 'own machine for testing purposes (i.e. it&#8217;s not publicly viewable, you might try ' 
			+ '&#8220;localhost&#8221; for the host domain.</p>' 
			+ '<p><strong>Important:</strong>If you are setting up multiple blogs on the same server, '
			+ 'multiple blogs can have the same host domain if they don&#8217;t also have the same ' 
			+ 'Application.  However, two blogs with different host domains can have the same Application.'
			+ '</p>';
			
		var applicationHelp = '<p>'
			+ 'Leave the application blank unless you are hosting multiple blogs on the '
			+ 'same server.</p>'
			+ '<p>The Application is a &#8220;subdirectory&#8221; that will correspond '
			+ 'to this blog.'
			+ '</p>';
		
	</script>
	
	<table border="0">
		<tr>
			<td><label for="lblTitle">Title:</label></td>
			<td>
				<asp:Label id="lblTitle" Runat="server"></asp:Label></td>
		</tr>
		<tr>
			<td><label for="txtHost"><a class="helpLink" href="?" onclick="showHelpTip(event, hostDomainHelp); return false;">Host Domain:</a></label></td>
			<td>
				<asp:TextBox id="txtHost" Runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator id="vldHostRequired" Runat="server" ControlToValidate="txtHost" Display="Dynamic"
					ErrorMessage="Please do not leave the Host Domain blank.">*</asp:RequiredFieldValidator></td>
		</tr>
		<tr>
			<td><label for="txtApplication"><a class="helpLink" href="?" onclick="showHelpTip(event, hostDomainHelp); return false;">Application:</a></label></td>
			<td>
				<asp:TextBox id="txtApplication" Runat="server"></asp:TextBox>
				<asp:RequiredFieldValidator id="vldApplicationRequired" Runat="server" ControlToValidate="txtApplication" Display="Dynamic"
					ErrorMessage="Please do not leave the Application blank.">*</asp:RequiredFieldValidator></td>
		</tr>
		<tr>
			<td><label for="txtUsername">User Name:</label></td>
			<td>
				<asp:TextBox id="txtUsername" Runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr id="passwordRow" runat="server">
			<td><label for="txtPassword">Password:</label></td>
			<td>
				<asp:TextBox id="txtPassword" Runat="server"></asp:TextBox></td>
		</tr>
		<tr id="passwordRowConfirm" runat="server">
			<td><label for="txtPasswordConfirm">Confirm Password:</label></td>
			<td>
				<asp:TextBox id="txtPasswordConfirm" Runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td colSpan="2">
				<asp:Button id="btnCancel" Runat="server" Text="Cancel" CssClass="button"></asp:Button>
				<asp:Button id="btnSave" Runat="server" Text="Save" CssClass="button"></asp:Button></td>
		</tr>
	</table>
</ANW:AdvancedPanel>

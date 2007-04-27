<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<st:MessagePanel id="messagePanel" runat="server"></st:MessagePanel>
<st:AdvancedPanel id="pnlResults" runat="server">

<table class="log">
	<tr>
		<td colspan="6">
			<asp:CheckBox id="chkShowInactive" AutoPostBack="True" Text="Show Inactive Blogs" Runat="server" oncheckedchanged="chkShowInactive_CheckedChanged"></asp:CheckBox>
			<asp:Button ID="addNewBlogButton" runat="server" OnClick="OnCreateNewBlogClick" Text="Create New Blog" />
		</td>
	</tr>

	<st:RepeaterWithEmptyDataTemplate id="rprBlogsList" Runat="server" OnItemCommand="rprBlogsList_ItemCommand">
		<HeaderTemplate>
			<tr class="header">
				<th>Title</th>
				<th>Host</th>
				<th>Subfolder</th>
				<th>Active</th>
				<th colspan="2">Action</th>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<a href="<%# DataBinder.Eval(Container.DataItem, "RootUrl") %>Default.aspx"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Subfolder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alt">
				<td>
					<a href="<%# DataBinder.Eval(Container.DataItem, "RootUrl") %>Default.aspx"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Subfolder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEditAlt" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<EmptyDataTemplate>
			<tr><td colspan="6" align="center">No entries found</td></tr>
		</EmptyDataTemplate>
		<FooterTemplate>
			
		</FooterTemplate>
	</st:RepeaterWithEmptyDataTemplate>
</table>

	<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="Default.aspx?pg={0}" 
			CssClass="Pager" />
</st:AdvancedPanel>

<st:AdvancedPanel id="pnlEdit" runat="server">
<div id="new-blog">
	<st:HelpToolTip id="blogEditorHelp" runat="server">
		<img id="Img3" src="~/images/ms_help.gif" align="right" alt="help" runat="server" />
	</st:HelpToolTip>
	<script type="text/javascript">
			function replace( str, from, to ) 
			{
				var idx = str.indexOf( from );

				while ( idx > -1 ) 
				{
					str = str.replace( from, to );
					idx = str.indexOf( from );
				}

				return str;
			}
			
			var badCharacters = '{}[]/\\ @!#$%;^&*()?+|"=\'<>;,';
			
			function removeInvalidCharacters(str)
			{
				var count = badCharacters.length;
				for(var i = 0; i < count; i++)
				{
					var badChar = badCharacters.substring(i, i + 1);
					str = replace(str, badChar, '');
				}
				return str;
			}
							
			function onPreviewChanged(txtHostId, txtApplicationId, virtualDirectoryId, isBlur)
			{
				var host = document.getElementById(txtHostId);
				var application = document.getElementById(txtApplicationId);
				var urlPreview = document.getElementById('urlPreview');
				var virtdirField = document.getElementById(virtualDirectoryId);
				
				var hostText = 'not specified';
				if(host && host.value != '')
				{
					hostText = removeInvalidCharacters(host.value);
					host.value = hostText;
					host.style.color = '#990000';
				}
				
				var appText = '';
				if(application && application.value != '')
				{
					appText = removeInvalidCharacters(application.value);
					appText = replace(appText, ':', '');
					if(appText.substring(0, 1) == '.')
						appText = appText.substring(1, appText.length);
						
					if(isBlur && appText.substring(appText.length - 1, appText.length) == '.')
					{
						appText = appText.substring(0, appText.length - 1);
					}
					
					application.value = appText;
					application.style.color = '#000099';
					appText = '<span style="color:#000099;">' + appText + '</span>';
					appText += '/';
				}
				
				var virtdirText = '';
				if(virtdirField && virtdirField.value != '')
				{
					virtdirText = '/' + virtdirField.value;
				}
			
				var preview = 'http://<span style="color:#990000">' + hostText + '</span>' + virtdirText + '/' + appText + 'default.aspx';
				
				urlPreview.innerHTML = preview;
			}
	</script>
	<st:HelpToolTip id="Helptooltip1" runat="server" HelpText="Based on what you’ve entered below, this shows what the url to this blog will look like.">
		Url Preview</st:HelpToolTip>:
	<div class="MessagePanel" id="urlPreview">http://
	</div>
	
	<div class="form">
		<div>
			<label for="lblTitle">Title:</label>
			<asp:TextBox id="txtTitle" Runat="server" MaxLength="100" />
		</div>
				
		<div>
			<label for="txtHost">
 				Host Domain <st:HelpToolTip id="hostDomainHelpTip" runat="server" ImageUrl="~/images/icons/help-small.png" />:
 			</label>
			<asp:TextBox id="txtHost" Runat="server" MaxLength="100"></asp:TextBox><input id="virtualDirectory" type="hidden" runat="server" />
		</div>
		<div>
			<label for="txtApplication">
				Subfolder 
				<st:HelpToolTip id="applicationHelpTip" runat="server" ImageUrl="~/images/icons/help-small.png" />:
			</label>
			<asp:TextBox id="txtApplication" Runat="server" MaxLength="50" />
		</div>
		<fieldset>
			<legend>Blog Owner</legend>
			<div>
				<label for="txtUsername">
					User Name:<st:HelpToolTip id="helpUsername" runat="server" HelpText="This will be the user who is the administrator of this blog." ImageUrl="~/images/icons/help-small.png" />
				</label>
				<asp:TextBox id="txtUsername" Runat="server" MaxLength="50" />
			</div>
			<div>
				<label for="emailTextBox">
					Email
				</label>
				<asp:TextBox id="emailTextBox" Runat="server" MaxLength="50" />
			</div>
			<div>
				<label for="txtPassword">
					Password: <st:HelpToolTip id="helpPassword" runat="server" HelpText="When editing an existing blog, you can leave this blank if you do not wish to change the password." ImageUrl="~/images/icons/help-small.png" />
				</label>
				<asp:TextBox id="txtPassword" Runat="server" MaxLength="50" TextMode="Password" />
			</div>
			<div>
				<label for="txtPasswordConfirm">Confirm Password:</label>
				<asp:TextBox id="txtPasswordConfirm" Runat="server" MaxLength="50" TextMode="Password" />
			</div>
			<div class="button-row">
				<asp:Button id="btnSave" Text="Save" Runat="server" CssClass="button" onclick="btnSave_Click" />&nbsp;&nbsp;
				<asp:Button id="btnCancel" Text="Cancel" Runat="server" CssClass="button" onclick="btnCancel_Click" />
			</div>
		</fieldset>
	</div>
</div>
</st:AdvancedPanel>
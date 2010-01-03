<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<st:MessagePanel id="messagePanel" runat="server"></st:MessagePanel>
<st:AdvancedPanel id="pnlResults" runat="server">
	<asp:CheckBox id="chkShowInactive" AutoPostBack="True" Text="Show Inactive Blogs" Runat="server" oncheckedchanged="chkShowInactive_CheckedChanged"></asp:CheckBox>
	<asp:Repeater id="rprBlogsList" Runat="server" OnItemCommand="OnBlogItemCommand">
		<HeaderTemplate>
			<table class="listing" cellspacing="0" cellpadding="4" border="0">
				<tr>
					<th>Title</th>
					<th>Host</th>
					<th>Subfolder</th>
					<th>Group</th>
					<th>Active</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<a href="<%# GetBlogUrl(Container.DataItem) %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
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
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "BlogGroupTitle") %>
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
					<a href="<%# GetBlogUrl(Container.DataItem) %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
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
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "BlogGroupTitle") %>
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
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<p id="lblNoMessages" runat="server" visible="false">No entries found.</p>
	<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="Default.aspx?pg={0}" 
			CssClass="Pager" />
</st:AdvancedPanel>
<st:AdvancedPanel id="pnlEdit" runat="server">
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
	<strong>
		<st:HelpToolTip id="Helptooltip1" runat="server" HelpText="Based on what you’ve entered below, this shows what the url to this blog will look like.">
			Url Preview</st:HelpToolTip>:
	</strong>
	<div class="MessagePanel" id="urlPreview">http://
	</div>
	<table border="0">
		<tr valign="top">
			<td><label for="lblTitle">Title:</label></td>
			<td>
				<asp:TextBox id="txtTitle" Runat="server" MaxLength="100"></asp:TextBox>
			</td>
			<td valign="top"><label>Blog Aliases </label></td>

		</tr>
		<tr valign="top">
			<td>
				<label for="txtHost">
 			    <st:HelpToolTip id="hostDomainHelpTip" runat="server"><strong>Host Domain</strong></st:HelpToolTip>:</label>
			</td>
			<td>
				<asp:TextBox id="txtHost" Runat="server" MaxLength="100"></asp:TextBox><input id="virtualDirectory" type="hidden" runat="server" />
			</td>	
			<td id="tdAliasHost" runat="server" visible="false">
				<asp:HiddenField ID="hdnAliasId" runat="server" />
				<asp:TextBox id="txtAliasHost" Runat="server" MaxLength="100"></asp:TextBox>
			</td>			
			<td rowspan="7" runat="Server" id="tdAliasList">
				<asp:Repeater runat="server" ID="blogAliasListRepeater" OnItemCommand="OnItemCommand">
					<HeaderTemplate>
						<table class="listing" cellspacing="2" cellpadding="0" border="0">
							<tr></tr>
							<tr>
								<th width="10px">&nbsp;</th>
								<th>
									Host</th>
								<th>
									Subfolder</th>
								<th>
									Active</th>
								<th>
									</th>
							</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td><asp:ImageButton ID="editAliasButton" runat="server" ImageUrl="~/Images/icons/edit.gif" CommandName="EditAlias" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' /> </td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "Host") %>
							</td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "Subfolder") %>
							</td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
							</td>
							<td><asp:LinkButton ID="deleteAliasButton" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="DeleteAlias" Text="Delete" /></td>
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alt">
							<td><asp:ImageButton ID="editAliasButton" runat="server" ImageUrl="~/Images/icons/edit.gif" CommandName="EditAlias" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' /> </td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "Host") %>
							</td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "Subfolder")%>
							</td>
							<td>
								<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
							</td>
							<td><asp:LinkButton ID="deleteAliasButton" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="DeleteAlias" Text="Delete" /></td>
						</tr>						
					</AlternatingItemTemplate>
					<FooterTemplate>
							<tr>
								<td colspan="5">
								<asp:LinkButton ID="addAliasButton" CssClass="button" Text="Add Alias" runat="server" CommandName="Add" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlogId") %>' OnClick="OnAddAliasOnClick"></asp:LinkButton>
								</td>
							</tr>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</td>
		</tr>
		<tr valign="top">
			<td>
				<st:HelpToolTip id="applicationHelpTip" runat="server">
					<strong>Subfolder</strong></st:HelpToolTip>:
				</td>
			<td>
				<asp:TextBox id="txtApplication" Runat="server" MaxLength="50"></asp:TextBox>
			</td>
			<td id="tdAliasApplication" runat="server" visible="false">
				<asp:TextBox id="txtAliasApplication" Runat="server" MaxLength="50"></asp:TextBox>
			</td>
		</tr>
		<tr valign="top">
			<td>
			    <label for="ddlGroups">Blog Group:</label>
			</td>
			<td>
				<asp:DropDownList id="ddlGroups" Runat="server" DataTextField="Title" DataValueField="Id"></asp:DropDownList></td>
			<td runat="server" id="tbAliasActive" visible="false">
				<asp:CheckBox ID="cbAliasActive" runat="server" />
			</td>
		</tr>
		<tr valign="top">
			<td>
			    <label for="txtUsername">
				    <st:HelpToolTip id="helpUsername" runat="server" 
				        HelpText="This will be the user who is the administrator of this blog.">
				    User Name:</st:HelpToolTip>
			    </label>
			</td>
			<td>
				<asp:TextBox id="txtUsername" Runat="server" MaxLength="50" />
			</td>
			<td>
				<asp:Button ID="btnAliasCancel" Text="Cancel" runat="server" CssClass="button" OnClick="btnAliasCancel_Click" Visible="False" />
				<asp:Button ID="btnAliasSave" runat="server" CssClass="button" OnClick="btnAliasSave_Click" Text="Save" Visible="False" />
			</td>
		</tr>
		<tr id="passwordRow" runat="server" valign="top">
			<td><label for="txtPassword">
					<st:HelpToolTip id="helpPassword" runat="server" HelpText="When editing an existing blog, you can leave this blank if you do not wish to change the password.">Password:</st:HelpToolTip></label>
			</td>
			<td>
				<asp:TextBox id="txtPassword" Runat="server" MaxLength="50" TextMode="Password" />
			</td>
			<td></td>
		</tr>
		<tr id="passwordRowConfirm" runat="server" valign="top">
			<td><label for="txtPasswordConfirm">Confirm Password:</label></td>
			<td>
				<asp:TextBox id="txtPasswordConfirm" Runat="server" MaxLength="50" TextMode="Password" />
			</td>
		</tr>
		<tr valign="top">
			<td colspan="2">
				<asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="button" OnClick="btnCancel_Click">
				</asp:Button>
				<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="button" OnClick="btnSave_Click">
				</asp:Button>
			</td>
		</tr>
	</table>
</st:AdvancedPanel>

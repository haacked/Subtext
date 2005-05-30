<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<ANW:MessagePanel id="messagePanel" runat="server"></ANW:MessagePanel>
<ANW:AdvancedPanel id="pnlResults" runat="server">
	<asp:CheckBox id="chkShowInactive" AutoPostBack="True" Text="Show Inactive Blogs" Runat="server"></asp:CheckBox>
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
					<a href="<%# DataBinder.Eval(Container.DataItem, "BlogRootUrl") %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
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
					<a href="<%# DataBinder.Eval(Container.DataItem, "BlogRootUrl") %>"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
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
	<ANW:Pager id="resultsPager" runat="server" CssClass="Pager" UrlFormat="Default.aspx?pg={0}"
		LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>"
		UseSpacer="False"></ANW:Pager>
</ANW:AdvancedPanel>
<ANW:AdvancedPanel id="pnlEdit" runat="server">
	<SP:HelpToolTip id="blogEditorHelp" runat="server">
		<IMG id="Img3" src="~/images/ms_help.gif" align="right" runat="server"></SP:HelpToolTip>
	<SCRIPT type="text/javascript">
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
				var virtDirField = document.getElementById(virtualDirectoryId);
				
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
				
				var virtDirText = '';
				if(virtDirField && virtDirField.value != '')
				{
					virtDirText = '/' + virtDirField.value;
				}
			
				var preview = 'http://<span style="color:#990000">' + hostText + '</span>' + virtDirText + '/' + appText + 'default.aspx';
				
				urlPreview.innerHTML = preview;
			}
	</SCRIPT>
	<STRONG>
		<SP:HelpToolTip id="Helptooltip1" runat="server" HelpText="Based on what you’ve entered below, this shows what the url to this blog will look like. <em>(Requires Javascript to be enabled)</em>">Url 
Preview</SP:HelpToolTip>:</STRONG>
	<DIV class="MessagePanel" id="urlPreview">http://
	</DIV>
	<TABLE border="0">
		<TR valign="top">
			<TD><LABEL for="lblTitle">Title:</LABEL></TD>
			<TD>
				<asp:label id="lblTitle" Runat="server"></asp:label>
				<asp:TextBox id="txtTitle" Runat="server" MaxLength="100"></asp:TextBox>
			</TD>
		</TR>
		<TR valign="top">
			<TD><LABEL for="txtHost">
					<SP:HelpToolTip id="hostDomainHelpTip" runat="server">
						<STRONG>Host Domain</STRONG></SP:HelpToolTip>:</LABEL>
			</TD>
			<TD>
				<asp:TextBox id="txtHost" Runat="server" MaxLength="100"></asp:TextBox><INPUT id="virtualDirectory" type="hidden" runat="server">
			</TD>
		</TR>
		<TR valign="top">
			<TD>
				<SP:HelpToolTip id="applicationHelpTip" runat="server">
					<STRONG>Application</STRONG></SP:HelpToolTip>:
				</TD>
			<TD>
				<asp:TextBox id="txtApplication" Runat="server" MaxLength="50"></asp:TextBox>
			</TD>
		</TR>
		<TR valign="top">
			<TD><LABEL for="txtUsername">
				<SP:HelpToolTip id="helpUsername" runat="server" HelpText="This will be the user who is the administrator of this blog.">User 
				Name:</SP:HelpToolTip></LABEL>
			</TD>
			<TD>
				<asp:TextBox id="txtUsername" Runat="server" MaxLength="50"></asp:TextBox></TD>
		</TR>
		<TR id="passwordRow" runat="server" valign="top">
			<TD><LABEL for="txtPassword">
					<SP:HelpToolTip id="helpPassword" runat="server" HelpText="When editing an existing blog, you can leave this blank if you do not wish to change the password.">Password:</SP:HelpToolTip></LABEL>
			</TD>
			<TD>
				<asp:TextBox id="txtPassword" Runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
			</TD>
		</TR>
		<TR id="passwordRowConfirm" runat="server" valign="top">
			<TD><LABEL for="txtPasswordConfirm">Confirm Password:</LABEL></TD>
			<TD>
				<asp:TextBox id="txtPasswordConfirm" Runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
			</TD>
		</TR>
		<TR valign="top">
			<TD colSpan="2">
				<asp:Button id="btnCancel" Text="Cancel" Runat="server" CssClass="button"></asp:Button>
				<asp:Button id="btnSave" Text="Save" Runat="server" CssClass="button"></asp:Button>
			</TD>
		</TR>
	</TABLE>
</ANW:AdvancedPanel>

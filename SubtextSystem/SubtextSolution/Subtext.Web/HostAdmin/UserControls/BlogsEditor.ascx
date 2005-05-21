<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
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
	<ANW:Pager id="resultsPager" runat="server" CssClass="Pager" UrlFormat="Default.aspx?pg={0}"
		LinkFormatActive='<a href="{0}" class="Current">{1}</a>' PrefixText="<div>Goto page</div>"
		UseSpacer="False"></ANW:Pager>
</ANW:AdvancedPanel>
<ANW:AdvancedPanel id="pnlEdit" runat="server">
	<SP:HelpToolTip id="helpBlogEditor" runat="server">
		<IMG id="Img3" src="~/images/ms_help.gif" align="right" runat="server"></SP:HelpToolTip>
	<TABLE border="0">
		<TR>
			<TD colSpan="2">
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
					
					var badCharacters = '/\\ @!#$%;^&*()?+|"=\'<>;';
					
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
									
					function onPreviewChanged(txtHostId, txtApplicationId, virtualDirectoryId)
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
					
						var preview = 'http://<span style="color:#990000">' + hostText + '</span>' + virtDirText + '/' + appText;
						
						urlPreview.innerHTML = preview;
					}
				</script>
				<STRONG>
					<SP:HelpToolTip id="Helptooltip1" runat="server" HelpText="Based on what you’ve entered below, this shows what the url to this blog will look like. <em>(Requires Javascript to be enabled)</em>">Url Preview</SP:HelpToolTip>:</STRONG>
				<DIV class="MessagePanel" id="urlPreview">http://
				</DIV>
			</TD>
		</TR>
		<TR>
			<TD><LABEL for="lblTitle">Title:</LABEL></TD>
			<TD>
				<asp:label id="lblTitle" Runat="server"></asp:label></TD>
		</TR>
		<TR>
			<TD><LABEL for="txtHost">
					<SP:HelpToolTip id="hostDomainHelpTip" runat="server">
						<STRONG>Host Domain</STRONG></SP:HelpToolTip>:</LABEL></TD>
			<TD>
				<asp:TextBox id="txtHost" Runat="server"></asp:TextBox>
				<input type="hidden" id="virtualDirectory" runat="server" />
			</TD>
		</TR>
		<TR>
			<TD>
				<SP:HelpToolTip id="applicationHelpTip" runat="server">
					<STRONG>Application</STRONG></SP:HelpToolTip>:</TD>
			<TD>
				<asp:TextBox id="txtApplication" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD><LABEL for="txtUsername">User Name:</LABEL></TD>
			<TD>
				<asp:TextBox id="txtUsername" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR id="passwordRow" runat="server">
			<TD><LABEL for="txtPassword">Password:</LABEL></TD>
			<TD>
				<asp:TextBox id="txtPassword" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR id="passwordRowConfirm" runat="server">
			<TD><LABEL for="txtPasswordConfirm">Confirm Password:</LABEL></TD>
			<TD>
				<asp:TextBox id="txtPasswordConfirm" Runat="server"></asp:TextBox></TD>
		</TR>
		<TR>
			<TD colSpan="2">
				<asp:Button id="btnCancel" Text="Cancel" Runat="server" CssClass="button"></asp:Button>
				<asp:Button id="btnSave" Text="Save" Runat="server" CssClass="button"></asp:Button></TD>
		</TR>
	</TABLE>
</ANW:AdvancedPanel>

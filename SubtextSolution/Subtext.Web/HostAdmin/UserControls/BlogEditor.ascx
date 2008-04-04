<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogEditor" %>
<%@ Register TagPrefix="st" TagName="UserChooser" Src="~/HostAdmin/UserControls/UserChooser.ascx" %>
<div id="new-blog">
	<st:MessagePanel id="messagePanel" runat="server" />
	
	<asp:ValidationSummary ID="validationMessages" CssClass="MessagePanel ErrorPanel" runat="server" />
		
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
	
	<st:HelpToolTip id="urlPreviewHelp" runat="server" HelpText="Based on what you’ve entered below, this shows what the url to this blog will look like.">
		Url Preview</st:HelpToolTip>:
	
	<div class="MessagePanel" id="urlPreview">http://</div>
	
	<div class="form">
		<fieldset id="blog-info">
			<legend>Blog Info</legend>
			<div>
				<asp:Label AssociatedControlId="titleTextBox" runat="server" ID="titleLabel">Title:</asp:Label>
				<asp:TextBox id="titleTextBox" Runat="server" MaxLength="100" Text="<%# Blog.Title %>" />
				<asp:RequiredFieldValidator ID="titleRequired" runat="server" 
					ControlToValidate="titleTextBox" 
					ErrorMessage="Everyone will know you by your blog&#8217;s title. Please enter one." 
					Display="None" />
			</div>
			
			<div>
				<asp:Label AssociatedControlId="hostTextBox" runat="server" ID="hostLabel">
 					Host Domain <st:HelpToolTip id="hostDomainHelpTip" runat="server" ImageUrl="~/images/icons/help-small.png" />:
 				</asp:Label>
				<asp:TextBox id="hostTextBox" Runat="server" MaxLength="100" Text="<%# Blog.Host %>"  /><input id="virtualDirectory" type="hidden" runat="server" />
				<asp:RequiredFieldValidator ID="hostRequiredValidator" runat="server" 
					ControlToValidate="hostTextBox" 
					ErrorMessage="Specify a host name." 
					Display="None" />
			</div>
			
			<div>
				<asp:Label AssociatedControlId="subfolderTextBox" runat="server" ID="subfolderLabel">
					Subfolder 
					<st:HelpToolTip id="subfolderHelpTip" runat="server" ImageUrl="~/images/icons/help-small.png" />:
				</asp:Label>
				<asp:TextBox id="subfolderTextBox" Runat="server" MaxLength="50" Text="<%# Blog.Subfolder %>"  />
			</div>
		</fieldset>
		<fieldset>
			<legend>Blog Owner</legend>
			<p class="field-instructions">
				While every blog may contain multiple admins and authors, 
				there must always be one owner for each blog. This person 
				is the one who receives emails from the contact page of the blog.
			</p>
			<st:UserChooser runat="server" 
				UserName="<%# CurrentUserName %>"
				id="blogOwnerChooser"  />
		</fieldset>
			
		<div class="button-row">
			<asp:Button id="btnSave" Text="Save" Runat="server" CssClass="button" onclick="OnSaveClick" />&nbsp;&nbsp;
			<asp:Button id="btnCancel" Text="Cancel" Runat="server" CssClass="button" onclick="OnCancelClick" CausesValidation="false" />
		</div>
		
	</div>
</div>
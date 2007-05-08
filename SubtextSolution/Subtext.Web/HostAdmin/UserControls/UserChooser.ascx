<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserChooser.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.UserChooser" %>
<div id="user-chooser">
	<div class="highlight">
		<asp:UpdatePanel ID="currentOwnerUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
			<ContentTemplate>
				<label class="inline">Current Owner:</label> <asp:Literal ID="usernameLiteral" runat="server" Text="<%# UserName %>" />
				<span id="changeowner" class="clickable action" onclick="onchangeownerclick();">[change]</span>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div style="display: none;" id="ownereditor">
		<div>
			<asp:UpdatePanel ID="changeOwnerPanel" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<h3>Select Existing User</h3>
					<st:UserGridView runat="server" id="usersGrid" 
						AutoGenerateSelectButton="true"
						EmptyDataText="No Results"
						PageSize="2" 
						CssClass="log highlightTable users"
						OnSelectedIndexChanged="OnUserSelected"
						DataSourceId="userDataSource"
					>
						<Columns>
							<asp:BoundField DataField="ProviderUserKey" HeaderText="ID" ReadOnly="true" Visible="false" />
							<asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="true" />
							<asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" NullDisplayText="n/a" />
						</Columns>
					</st:UserGridView>
					
					<st:MembershipUserDataSource ID="userDataSource" runat="server" />
				
					<asp:PlaceHolder ID="createUserPlaceholder" runat="server" Visible="false">
						<fieldset id="create-user">
							<legend>Create New User</legend>
							
							<div>
								<asp:Label AssociatedControlId="usernameTextBox" runat="server" ID="usernameLabel">
									User Name:<st:HelpToolTip id="helpUsername" runat="server" HelpText="This will be the user who is the administrator of this blog." ImageUrl="~/images/icons/help-small.png" />
								</asp:Label>
								<asp:TextBox id="usernameTextBox" Runat="server" MaxLength="50" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
									ControlToValidate="usernameTextBox" 
									ErrorMessage="Specify a username for the blog owner." 
									Display="None" />
							</div>
							
							<div>
								<asp:Label AssociatedControlId="emailTextBox" runat="server" ID="emailLabel">
									Email
								</asp:Label>
								<asp:TextBox id="emailTextBox" Runat="server" MaxLength="50" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
									ControlToValidate="emailTextBox" 
									ErrorMessage="Specify an email address for the blog owner." 
									Display="None" />
							</div>
							
							<div>
								<asp:Label AssociatedControlId="passwordTextBox" runat="server" ID="passwordLabel">
									Password: <st:HelpToolTip id="helpPassword" runat="server" HelpText="When editing an existing blog, you can leave this blank if you do not wish to change the password." ImageUrl="~/images/icons/help-small.png" />
								</asp:Label>
								<asp:TextBox id="passwordTextBox" Runat="server" MaxLength="50" TextMode="Password" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
									ControlToValidate="passwordTextBox" 
									ErrorMessage="Enter a password for the blog owner." 
									Display="None" />
							</div>
							
							<div>
								<asp:Label AssociatedControlId="passwordConfirmTextBox" runat="server" ID="confirmLabel">Confirm Password:</asp:Label>
								<asp:TextBox id="passwordConfirmTextBox" Runat="server" MaxLength="50" TextMode="Password" />
								<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
									ControlToValidate="passwordConfirmTextBox" 
									ErrorMessage="Please confirm the password." 
									Display="None" />
								<asp:CompareValidator ID="passwordCompareValidator" runat="server"
									ControlToValidate="passwordConfirmTextBox"
									ControlToCompare="passwordTextBox"
									Type="String"
									ErrorMessage="The passwords do not match."
									Display="None" />
							</div>
						</fieldset>
					</asp:PlaceHolder>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</div>
</div>
<script type="text/javascript">
	function onchangeownerclick()
	{
		if($('changeowner').innerHTML == '[change]')
		{
			$('changeowner').innerHTML = '[cancel]';
			Effect.SlideDown('ownereditor', {duration:0.5});
		}
		else
		{
			$('changeowner').innerHTML = '[change]';
			Effect.SlideUp('ownereditor', {duration:0.5});
		}
	}
	
	Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(pageLoadingHandler);
	
	function pageLoadingHandler(sender, e)
	{
		if(sender._panelsToRefreshIDs[0].indexOf('currentOwnerUpdatePanel') >= 0)
			onchangeownerclick();
	}
</script>


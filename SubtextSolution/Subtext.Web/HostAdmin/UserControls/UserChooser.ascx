<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserChooser.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.UserChooser" %>
<%@ Register TagPrefix="st" TagName="CreateUserControl" Src="CreateUserControl.ascx" %>
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
					<asp:PlaceHolder ID="selectUserPlaceholder" runat="server">
						<h3>Select Existing User</h3>
						<asp:LinkButton ID="createNewUserButton" runat="server" Text="[new user]" CssClass="clickable action" OnClick="ShowCreateUser" />
						<st:UserGridView runat="server" id="usersGrid" 
							AutoGenerateSelectButton="true"
							EmptyDataText="No Results"
							PageSize="10" 
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
					</asp:PlaceHolder>				
					<asp:PlaceHolder ID="createUserPlaceholder" runat="server" Visible="false">
						<st:CreateUserControl id="createUserControl" runat="server" />
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


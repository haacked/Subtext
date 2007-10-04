<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Welcome" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Default" %>
<%@ Import namespace="Subtext.Framework.Configuration"%>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">	
	<asp:Wizard ID="installationWizard" runat="server" 
		NavigationButtonStyle-CssClass="button" 
		SkipLinkText=""
		StartNextButtonText="Next &gt;&gt;"
		StepPreviousButtonText="&lt;&lt; Previous"
		FinishPreviousButtonText="&lt;&lt; Previous"
		OnNextButtonClick="OnNextButtonClick"
		OnFinishButtonClick="OnFinishButtonClick"
	>
	<SideBarStyle CssClass="hidden" />
	
	<wizardsteps>
		<asp:WizardStep ID="WizardStep1" title="Installation Type" runat="server" StepType="Start">
			<fieldset>
				<legend>Choose an Installation Type</legend>
				<p>
					Are you planning to host a single blog or multiple blogs?
					If you are unsure, choose &#8220;Multiple Blogs&#8221;
				</p>
				<asp:RadioButton ID="chkSingle" runat="server" Text="Single Blog" CssClass="majorchoice radio" GroupName="InstallationType" Checked="true" />
				<asp:RadioButton ID="chkMultiple" runat="server" Text="Multiple Blogs" CssClass="majorchoice radio" GroupName="InstallationType" />
				<p class="footnote">* Please note that the current version of Subtext only supports 
					Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
					for other databases and file system storage.
				</p>
			</fieldset>
		</asp:WizardStep>
		
		<asp:TemplatedWizardStep ID="WizardStep2" title="Host Administration" runat="server" StepType="Step">
			<ContentTemplate>
				<fieldset>
					<legend>Host Administration Account</legend>
					<p>
						Please enter information for the default Host Administrator.
					</p>
					<p>
						A Host Administrator is a user with the ability to maintain a 
						Subtext <em>installation</em>. This user is not an administrator 
						of any specific blog, but can add, edit, and delete blogs and 
						users.
					</p>
					<p>
						The first Host Administrator you create here will be flagged 
						as the <em>owner</em> of the installation and cannot be deleted. 
					</p>
					<div>
						<asp:ValidationSummary ID="vldHostAdminSummary" runat="server" ValidationGroup="HostAdministration" HeaderText="Please correct the following issues" />
						
						<label for="txtHostAdminUserName">UserName</label> 
						<asp:TextBox ID="txtHostAdminUserName" runat="server" CssClass="textbox" ValidationGroup="HostAdministration" />
						<asp:RequiredFieldValidator ID="vldHostUsernameRequired" runat="server" ControlToValidate="txtHostAdminUserName" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
						
						<label for="txtHostAdminEmail">Email</label> 
						<asp:TextBox ID="txtHostAdminEmail" runat="server" CssClass="textbox" />
						<asp:RequiredFieldValidator ID="vldHostEmailRequired" runat="server" ControlToValidate="txtHostAdminEmail" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
						<asp:RegularExpressionValidator ID="vldHostEmail" runat="server" ControlToValidate="txtHostAdminEmail" ValidationGroup="HostAdministration" Text="*" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Email address is not valid" Display="Dynamic" />
						
						<label for="txtHostAdminPassword">Password</label> 
						<asp:TextBox ID="txtHostAdminPassword" runat="server" CssClass="textbox" TextMode="Password" />
						<asp:RequiredFieldValidator ID="vldHostAdminPassword" runat="server" ControlToValidate="txtHostAdminPassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
						
						<label for="txtHostAdminComparePassword">Confirm Password</label> 
						<asp:TextBox ID="txtHostAdminComparePassword" runat="server" CssClass="textbox" TextMode="Password" />
						<asp:RequiredFieldValidator ID="vldHostComparePassword" runat="server" ControlToValidate="txtHostAdminComparePassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
						<asp:CompareValidator ID="vldHostPasswordsMatch" runat="server" ControlToValidate="txtHostAdminComparePassword" ControlToCompare="txtHostAdminPassword" ValidationGroup="HostAdministration" Text="*" ErrorMessage="The passwords do not match." Display="Dynamic" />
					</div>
				</fieldset>
			</ContentTemplate>
			
			<CustomNavigationTemplate>
				<asp:Button ID="btnPrevious" runat="server" CssClass="button" Text="&lt;&lt; Previous" CommandName="MovePrevious"  />
				<asp:Button ID="btnNext" runat="server" CssClass="button" CommandName="MoveNext" Text="Next &gt;&gt;" CausesValidation="true" ValidationGroup="HostAdministration" UseSubmitBehavior="true" />
			</CustomNavigationTemplate>
		</asp:TemplatedWizardStep>
		
		<asp:TemplatedWizardStep ID="WizardStep3" title="Blog Adminstrator" runat="server" StepType="Step">
			<ContentTemplate>
				<fieldset>
					<legend>Blog Administration Account</legend>
					<p>
						Please enter information for the blog administrator.
					</p>
					<p>
						You&#8217;ll be able to create other users (including other 
						administrators) for your blog later. This user will be the 
						<em>owner</em> of the blog.
					</p>
					<div>
						<label for="txtAdminUserName">UserName</label> 
						<asp:TextBox ID="txtAdminUserName" runat="server" CssClass="textbox" ValidationGroup="BlogAdministration" />
						<asp:RequiredFieldValidator ID="vldHostUsernameRequired" runat="server" ControlToValidate="txtAdminUserName" ValidationGroup="BlogAdministration" Text="*" Display="Dynamic" />
												
						<label for="txtAdminEmail">Email</label> 
						<asp:TextBox ID="txtAdminEmail" runat="server" CssClass="textbox" />
						<asp:RequiredFieldValidator ID="vldHostEmailRequired" runat="server" ControlToValidate="txtAdminEmail" ValidationGroup="BlogAdministration" Text="*" Display="Dynamic" />
						<asp:RegularExpressionValidator ID="vldHostEmail" runat="server" ControlToValidate="txtAdminEmail" ValidationGroup="BlogAdministration" Text="*" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Email address is not valid" Display="Dynamic" />
												
						<label for="txtAdminPassword">Password</label> 
						<asp:TextBox ID="txtAdminPassword" runat="server" CssClass="textbox" TextMode="Password" />
						<asp:RequiredFieldValidator ID="vldAdminPassword" runat="server" ControlToValidate="txtAdminPassword" ValidationGroup="BlogAdministration" Text="*" Display="Dynamic" />
												
						<label for="txtAdminComparePassword">Confirm Password</label> 
						<asp:TextBox ID="txtAdminComparePassword" runat="server" CssClass="textbox" TextMode="Password" />
						<asp:RequiredFieldValidator ID="vldHostComparePassword" runat="server" ControlToValidate="txtAdminComparePassword" ValidationGroup="BlogAdministration" Text="*" Display="Dynamic" />
						<asp:CompareValidator ID="vldHostPasswordsMatch" runat="server" ControlToValidate="txtAdminComparePassword" ControlToCompare="txtAdminPassword" ValidationGroup="BlogAdministration" Text="*" ErrorMessage="The passwords do not match." Display="Dynamic" />
					</div>
				</fieldset>
			</ContentTemplate>
			
			<CustomNavigationTemplate>
				<asp:Button ID="btnPrevious" runat="server" CssClass="button" Text="&lt;&lt; Previous" CommandName="MovePrevious"  />
				<asp:Button ID="btnNext" runat="server" CssClass="button" CommandName="MoveNext" Text="Next &gt;&gt;" CausesValidation="true" ValidationGroup="HostAdministration" UseSubmitBehavior="true" />
			</CustomNavigationTemplate>

		</asp:TemplatedWizardStep>
		
		<asp:WizardStep ID="WizardStep4" title="Install!" runat="server" StepType="Finish">
			<fieldset>
				<legend>Ready to Install!</legend>
				<p>
					Please review the following information and click <em>Finish</em> 
					to begin the installation process.
				</p>
				
				<asp:Label ID="installationStateMessage" runat="server" CssClass="error" Visible="false" />
				
				<div class="summary">
					<dl>
						<dt><label>UserName:</label></dt><dd><span><asp:Literal ID="ltlUsername" runat="server" Text="<%# AdminUserName %>" /></span></dd>
						<dt><label>Email:</label></dt><dd><span><asp:Literal ID="ltlEmail" runat="server" Text="<%# AdminEmail %>" /></span></dd>
						<dt><label>Database:</label></dt><dd><asp:Literal id="ltlDbName" Runat="server" Text="<%# Config.ConnectionString.Database %>" /> <strong>*</strong></dd>
					</dl>
				</div>
				<p class="clear">
					<strong>Before you click <em>Finish</em>, please make sure the database 
					user has <em>database owner</em> (<acronym title="database owner">dbo</acronym>) privileges</strong> temporarily 
					<st:HelpToolTip id="HelpToolTip1" runat="server" 
						HelpText="If you are using trusted connections, then the user is either <strong><em>MachineName/Network Services</em></strong> for Windows 2003 or <strong><em>MachineName/ASPNET</em></strong> for Windows 2000 or XP. Otherwise the user is specified in the <code>ConnectionStrings</code> section of <code>Web.config</config>. The Connection String Name should be &#8220;subtextData&#8221;">
						<img id="Img2" src="~/Admin/Resources/Scripts/Images/ms_information_small.gif" runat="Server" />
					</st:HelpToolTip>. 
				</p>
				<p class="footnote">*If the database is not correct, update your web.config file and refresh this page.</p>
			</fieldset>
		</asp:WizardStep>
		
		<asp:WizardStep ID="WizardStep5" Title="Installation Complete!" runat="server" StepType="complete">
			<fieldset>
				<legend>Installation Complete!</legend>
				<p>
					<strong>Congratulations!</strong> Your Subtext installation is complete.  
					Consider <a href="http://subtextproject.com/Home/Docs/Configuration/ConfiguringACustom404Page/tabid/121/Default.aspx" title="Configuring a Custom 404 Page">configuring a custom 404 (File Not Found) page</a> in order to support 
					friendly URLs.
				</p>
				<p>
					To learn more about Subtext, visit the <a href="http://subtextproject.com/" title="Subtext Info">Subtext Project Website</a>.
				</p>
							
				<asp:Panel ID="pnlSingleBlog" runat="server" Visible="<%# SingleBlogSetup %>">
					<p>
						The <strong>NEXT STEP</strong> is to login and 
						<strong><asp:HyperLink ID="navBlogAdmin" runat="server" NavigateUrl="~/Admin/Options.aspx" Text="customize your blog" /></strong>.
					</p>
				</asp:Panel>
				
				<asp:Panel ID="pnlMultiBlog" runat="server" Visible="<%# !SingleBlogSetup %>">
					<p>
						The <strong>NEXT STEP</strong> is to login to the 
						<strong><asp:HyperLink ID="lnkHostAdmin" runat="server" NavigateUrl="~/HostAdmin/Default.aspx" Text="Host Administration Site" /></strong> 
						and start creating blogs.
					</p>
				</asp:Panel>
				
			</fieldset>
		</asp:WizardStep>
	</wizardsteps>
	</asp:Wizard>
</asp:Content>

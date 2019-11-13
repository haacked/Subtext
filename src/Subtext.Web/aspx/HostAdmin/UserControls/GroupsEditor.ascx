<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="GroupsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.GroupsEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<st:MessagePanel id="messagePanel" runat="server"></st:MessagePanel>
<st:AdvancedPanel id="pnlResults" runat="server">
	<asp:CheckBox id="chkShowInactive" AutoPostBack="True" Text="Show Inactive Blogs Groups" Runat="server" oncheckedchanged="chkShowInactive_CheckedChanged"></asp:CheckBox>
	<asp:Repeater id="rprGroupsList" Runat="server" OnItemCommand="rprGroupsList_ItemCommand">
		<HeaderTemplate>
			<table class="listing" cellspacing="0" cellpadding="4" border="0">
				<tr>
					<th>Title</th>
					<th>Display Order</th>
					<th>Active</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<strong>
						<%# NullValue.IsNull(Int32.Parse(DataBinder.Eval(Container.DataItem, "DisplayOrder").ToString())) ? string.Empty : DataBinder.Eval(Container.DataItem, "DisplayOrder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkActivate" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text='Delete' runat="server" OnClientClick="return confirm('Are you sure you want to delete this group?');" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<strong>
						<%# NullValue.IsNull(Int32.Parse(DataBinder.Eval(Container.DataItem, "DisplayOrder").ToString())) ? string.Empty : DataBinder.Eval(Container.DataItem, "DisplayOrder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkActivate" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "|" + DataBinder.Eval(Container.DataItem, "IsActive").ToString()  %>' Text='Delete' runat="server" OnClientClick="return confirm('Are you sure you want to delete this group?');" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<p id="lblNoMessages" runat="server" visible="false">No entries found.</p>	
</st:AdvancedPanel>
<st:AdvancedPanel id="pnlEdit" runat="server">
	<st:HelpToolTip id="blogEditorHelp" runat="server">
		<img id="Img3" src="~/images/ms_help.gif" align="right" alt="help" runat="server" />
	</st:HelpToolTip>
    <table border="0">
		<tr valign="top">
			<td><label for="lblTitle">Title:</label></td>
			<td>
                <asp:HiddenField ID="hfActive" runat="server" />
				<asp:TextBox id="txtTitle" Runat="server" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                    Display="Dynamic" ErrorMessage="Title is required" ValidationGroup="Group"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr valign="top">
			<td>
				<label for="txtDisplayOrder">
 			    <strong>DisplayOrder</strong>:</label>
			</td>		
			<td>
				<asp:TextBox id="txtDisplayOrder" Runat="server" MaxLength="10"></asp:TextBox><asp:RangeValidator
                    ID="RangeValidator1" errormessage="Display order must be be a number between 0 and 1 000 000" runat="server" ControlToValidate="txtDisplayOrder" MaximumValue="1000000" MinimumValue="0" Display="Dynamic" Type="Integer" ValidationGroup="Group"></asp:RangeValidator>
			</td>	
			</tr>
		<tr valign="top">
			<td>
				<label for="txtDescription">
 			    <strong>Description</strong>:</label>
			</td>		
			<td>
				<asp:TextBox id="txtDescription" Runat="server" MaxLength="1000" Columns="50" Rows="10" TextMode="MultiLine"></asp:TextBox>
			</td>
		</tr>
		<tr valign="top">
			<td colspan="2">
				<asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="button" OnClick="btnCancel_Click">
				</asp:Button>
				<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="button" OnClick="btnSave_Click" ValidationGroup="Group">
				</asp:Button>
			</td>
		</tr>
    </table>
</st:AdvancedPanel>

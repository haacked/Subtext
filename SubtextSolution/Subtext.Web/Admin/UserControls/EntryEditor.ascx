<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="EntryEditor.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.EntryEditor"%>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Import Namespace = "Subtext.Web.Admin" %>



<ANW:MessagePanel id="Messages" runat="server" />

<ANW:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True">
	<asp:Repeater id="rprSelectionList" runat="server">
		<HeaderTemplate>
			<table id="Listing" class="listing highlightTable" cellspacing="0" cellpadding="0" border="0" style="<%= CheckHiddenStyle() %>">
				<tr>
					<th>Description</th>
					<th width="50">Active</th>
					<th width="75">Web Views</th>
					<th width="75">Agg Views</th>
					<th width="50">Referrals</th>
					<th width="50">&nbsp;</th>
					<th width="50">&nbsp;</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
				    <asp:HyperLink runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "FullyQualifiedUrl") %>' ToolTip="View Entry" >
				        <%# DataBinder.Eval(Container.DataItem, "Title") %></asp:HyperLink>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>												
				<td>
					<%# DataBinder.Eval(Container.DataItem, "WebCount") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "AggCount") %>
				</td>				
				<td>
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>" title="View Referrals">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEdit" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alt">
				<td>
					<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "FullyQualifiedUrl") %>' ToolTip="View Entry" >
				        <%# DataBinder.Eval(Container.DataItem, "Title") %></asp:HyperLink>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "WebCount") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "AggCount") %>
				</td>					
				<td>
					<a href="Referrers.aspx?EntryID=<%# DataBinder.Eval(Container.DataItem, "Id") %>" title="View Referrals">View</a>
				</td>				
				<td>
					<asp:LinkButton id="lnkEditAlt" CausesValidation="False" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CausesValidation="False" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	
	<p id="NoMessagesLabel" runat="server" visible="false">No entries found.</p>
		
	<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="EditPosts.aspx?pg={0}" 
			CssClass="Pager" />
	<br class="clear" />
</ANW:AdvancedPanel>

<ANW:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle" Collapsible="False" HeaderText="Edit Post">
	<div id="entry-editor" class="Edit">
		<!-- DEBUG -->
		<p class="Label"><asp:HyperLink id="hlEntryLink" Runat="server" /></p>
		
		<label for="Editor_Edit_txbTitle" accesskey="t">Post <u>T</u>itle&nbsp;<asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" ForeColor="#990066" ErrorMessage="Your post must have a title" /></label>
		<asp:TextBox id="txbTitle" runat="server" MaxLength="250" />
		<label for="Editor_Edit_richTextEditor" accesskey="b">Post <u>B</u>ody&nbsp;<asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" ForeColor="#990066" ErrorMessage="Your post must have a body" /></label>
		<st:RichTextEditor id="richTextEditor" runat="server" onerror="richTextEditor_Error"></st:RichTextEditor>
		<label>Categories</label>
		<p><asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"></asp:CheckBoxList></p>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post"  />
			<asp:Button id="lkUpdateCategories" runat="server" CssClass="buttonSubmit" CausesValidation="false" Text="Categories" />
			<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" CausesValidation="false" Text="Cancel" />
			&nbsp;
		</div>
	</div>

	<ANW:AdvancedPanel id="Enclosure" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True" Collapsed="False" HeaderText="Enclosure" BodyCssClass="Edit">
	<asp:CheckBox id="chkEnableEnclosure" runat="server" Text="Enable Enclosure" />
	<div id="enclosure-editor" class="Edit">
		<fieldset title="Enclosure">
			<legend>Basic Enclosure</legend>
		    <label for="Editor_Enclosure_txbEnclosureTitle" accesskey="e"><u>E</u>nclosure Title</label>
	        <asp:TextBox id="txbEnclosureTitle" runat="server" MaxLength="250" />
	        <label for="Editor_Enclosure_txbEnclosureUrl" accesskey="u">Enclosure <u>U</u>rl&nbsp;<asp:RequiredFieldValidator id="valEncUrlRequired" runat="server" ControlToValidate="txbEnclosureUrl" ForeColor="#990066" ErrorMessage="Url is required" /></label>
	        <asp:TextBox id="txbEnclosureUrl" runat="server" MaxLength="250" />
	        <label for="Editor_Enclosure_txbEnclosureSize" accesskey="s">Enclosure <u>S</u>ize (in bytes) &nbsp;<asp:RequiredFieldValidator id="valEncSizeRequired" runat="server" ControlToValidate="txbEnclosureSize" ForeColor="#990066" ErrorMessage="Size is required" Display="Dynamic" /><asp:RangeValidator ControlToValidate="txbEnclosureSize" MinimumValue="0" MaximumValue="999999999" id="valEncSizeFormat" runat="server" ForeColor="#990066"  Type="Integer" ErrorMessage="Not a valid size." ></asp:RangeValidator></label>
	        <asp:TextBox id="txbEnclosureSize" runat="server" MaxLength="250" />
	        <label for="Editor_Enclosure_ddlMimeType" accesskey="m">Enclosure <u>M</u>imetype&nbsp;<asp:CompareValidator Operator="NotEqual" ValueToCompare="none" id="valEncMimeTypeRequired" runat="server" ControlToValidate="ddlMimeType" ForeColor="#990066" ErrorMessage="MimeType is required." /></label>
	        <asp:DropDownList ID="ddlMimeType" runat="server">
	            <asp:ListItem Value="none">Choose...</asp:ListItem>
	            <asp:ListItem Value="application/pdf">application/pdf</asp:ListItem>
	            <asp:ListItem Value="application/octet-stream">application/octet-stream</asp:ListItem>
	            <asp:ListItem Value="audio/mpeg">audio/mpeg</asp:ListItem>
	            <asp:ListItem Value="video/mp4">video/mp4</asp:ListItem>
	            <asp:ListItem Value="other">Other</asp:ListItem>
	        </asp:DropDownList> <asp:TextBox id="txbEnclosureOtherMimetype" CssClass="othertype" runat="server" MaxLength="25" />&nbsp;<asp:RequiredFieldValidator id="valEncOtherMimetypeRequired" runat="server" ControlToValidate="txbEnclosureOtherMimetype" ForeColor="#990066" ErrorMessage="You have to specify a custom mimetype." />
	        <label for="Editor_Enclosure_ddlDisplayOnPost" accesskey="p">Display with <u>P</u>ost on website</label>
	        <asp:DropDownList ID="ddlDisplayOnPost" runat="server">
	            <asp:ListItem Value="true">Yes</asp:ListItem>
	            <asp:ListItem Value="false">No</asp:ListItem>
	        </asp:DropDownList>
	        <label for="Editor_Enclosure_ddlAddToFeed" accesskey="f">Add to <u>F</u>eed</label>
	        <asp:DropDownList ID="ddlAddToFeed" runat="server">
	            <asp:ListItem Value="true">Yes</asp:ListItem>
	            <asp:ListItem Value="false">No</asp:ListItem>
	        </asp:DropDownList>
	    </fieldset>
	</div>
	</ANW:AdvancedPanel>

	<ANW:AdvancedPanel id="Advanced" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="True" Collapsed="False" HeaderText="Advanced Options" BodyCssClass="Edit">
		<!-- todo, make this more css based than table driven -->
		<table cellpadding="4">
			<tr>
				<td width="200"><asp:CheckBox id="ckbPublished" runat="server" Text="Published" textalign="Right" />&nbsp;</td>
				<td width="200"><asp:CheckBox id="chkComments" runat="server" Text="Show Comments" textalign="Right" />&nbsp;</td>	
				<td width="200"><asp:CheckBox id="chkCommentsClosed" runat="server" Text="Comments Closed" textalign="Right" />&nbsp;</td>
				<td width="200"><asp:CheckBox id="chkDisplayHomePage" runat="server" Text="Display on HomePage" textalign="Right" />&nbsp;</td>
			</tr>
			<tr>
				<td><asp:CheckBox id="chkMainSyndication" runat="server" Text="Syndicate on Main Feed" textalign="Right" />&nbsp;</td>
				<td><asp:CheckBox id="chkSyndicateDescriptionOnly" runat="server" Text="Syndicate Description Only" textalign="Right" />&nbsp;</td>
				<td><asp:CheckBox id="chkIsAggregated" runat="server" Text="Include in Aggregated Site" textalign="Right" />&nbsp;</td>
			</tr>
		</table>
		<div id="advanced-options">
		    <label for="Editor_Edit_txtPostDate" accesskey="d">Post <u>D</u>ate <asp:CustomValidator ID="vCustomPostDate" runat="server" Text="Invalid PostDate format. Must be a valid date/time expression" ControlToValidate="txtPostDate" /></label> 
		    <asp:TextBox ID="txtPostDate" runat="server" CssClass="date" MaxLength="25" />
		    <label for="Editor_Edit_txbEntryName" accesskey="n">Entry <u>N</u>ame (page name) <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate="txbEntryName" ValidationExpression="^([a-zA-Z0-9-\.]*([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9-_]+)$" Text = "Invalid EntryName Format. Must only contain characters allowable in an URL." runat="server"/></label>
		    <asp:TextBox id="txbEntryName" runat="server" MaxLength="150" />
		    <label for="Editor_Edit_txbExcerpt" accesskey="e"><u>E</u>xcerpt</label>
		    <asp:TextBox id="txbExcerpt" runat="server" rows="5" textmode="MultiLine" MaxLength="500" />
		</div>
	</ANW:AdvancedPanel>
	

	
</ANW:AdvancedPanel>

<script type="text/javascript">

        function ValidatorEnclusureEnable(enabled)
        {
            ValidatorEnable($("#<%= valEncUrlRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncSizeRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncMimeTypeRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncOtherMimetypeRequired.ClientID %>")[0], enabled);
        }

        function toggleOtherMimeType(elem)
        {
            if(elem!=undefined) 
            {
                if(elem.value=="other")
                {
                    $("#<%= txbEnclosureOtherMimetype.ClientID %>").show();
                    ValidatorEnable($("#<%= valEncOtherMimetypeRequired.ClientID %>")[0], true);
                }
                else
                {
                    $("#<%= txbEnclosureOtherMimetype.ClientID %>").hide();
                    ValidatorEnable($("#<%= valEncOtherMimetypeRequired.ClientID %>")[0], false);
                }
            }
        }
        
        function toggleEnclosureBox(elem)
        {
            if(elem!=undefined) 
            {
                if(elem.checked==true)
                {
                    $("#enclosure-editor").show();
                    ValidatorEnclusureEnable(true);
                }
                else
                {
                    $("#enclosure-editor").hide();
                    ValidatorEnclusureEnable(false);
                }
            }
        }

        // first let's hook up some events
        $(document).ready(function()
        {
            toggleEnclosureBox($("#<%= chkEnableEnclosure.ClientID %>")[0]);
            toggleOtherMimeType($("#<%= ddlMimeType.ClientID %>")[0]);

            $("#<%= ddlMimeType.ClientID %>").change(function() 
            {
                toggleOtherMimeType(this);
            });
            
            $("#<%= chkEnableEnclosure.ClientID %>").change(function() 
            {
                toggleEnclosureBox(this);
                toggleOtherMimeType(this);
            });
        });
       

</script>
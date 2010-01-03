<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="EntryEditor.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.EntryEditor"%>

<st:MessagePanel id="Messages" runat="server" />
    
	<div id="entry-editor" class="Edit">
	    <h2><% if (PostId == null) {%>New<% } else { %>Edit<%} %> Post</h2>
	    
	    <% if (PostId != null) { %>
		    <p class="Label"><asp:HyperLink id="hlEntryLink" Runat="server" /></p>
		<% } %>
		
		<asp:Label AssociatedControlID="txbTitle" AccessKey="t" runat="server">
		    Post <u>T</u>itle&nbsp;
		    <asp:RequiredFieldValidator id="valTitleRequired" runat="server" ControlToValidate="txbTitle" ForeColor="#990066" ErrorMessage="Your post must have a title" />
		</asp:Label>
		<asp:TextBox id="txbTitle" runat="server" MaxLength="250" />
		
		<asp:Label AssociatedControlID="richTextEditor" AccessKey="b" runat="server">
		    Post <u>B</u>ody&nbsp;
		    <asp:RequiredFieldValidator id="valtbBodyRequired" runat="server" ControlToValidate="richTextEditor" ForeColor="#990066" ErrorMessage="Your post must have a body" />
		</asp:Label>
		<st:RichTextEditor id="richTextEditor" runat="server" onerror="richTextEditor_Error" />
		
		<label>Categories</label>
		<p><asp:CheckBoxList id="cklCategories" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" CssClass="checkbox" /></p>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post"  />
			<asp:Button id="lkUpdateCategories" runat="server" CssClass="buttonSubmit" CausesValidation="false" Text="Categories" />
			<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" CausesValidation="false" Text="Cancel" />
			&nbsp;
		</div>
	</div>

	<st:AdvancedPanel id="Advanced" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="False" Collapsed="False" HeaderText="Advanced Options" BodyCssClass="Edit">
		<!-- todo, make this more css based than table driven -->
		<table cellpadding="4">
			<tr>
				<td width="200"><asp:CheckBox id="ckbPublished" runat="server" Text="Published" textalign="Right" CssClass="checkbox" /></td>
				<td width="200"><asp:CheckBox id="chkComments" runat="server" Text="Show Comments" textalign="Right" CssClass="checkbox" /></td>	
				<td width="200"><asp:CheckBox id="chkCommentsClosed" runat="server" Text="Comments Closed" textalign="Right" CssClass="checkbox" /></td>
				<td width="200"><asp:CheckBox id="chkDisplayHomePage" runat="server" Text="Display on HomePage" textalign="Right" CssClass="checkbox" /></td>
			</tr>
			<tr>
				<td><asp:CheckBox id="chkMainSyndication" runat="server" Text="Syndicate on Main Feed" textalign="Right" CssClass="checkbox" /></td>
				<td><asp:CheckBox id="chkSyndicateDescriptionOnly" runat="server" Text="Syndicate Description Only" textalign="Right" CssClass="checkbox" /></td>
				<td><asp:CheckBox id="chkIsAggregated" runat="server" Text="Include in Aggregated Site" textalign="Right" CssClass="checkbox" /></td>
			</tr>
		</table>
		<div id="advanced-options">
		    <asp:Label AssociatedControlID="txtPostDate" AccessKey="d" runat="server">
		        Post <u>D</u>ate 
		        <asp:CustomValidator ID="vCustomPostDate" runat="server" Text="Invalid PostDate format. Must be a valid date/time expression" ControlToValidate="txtPostDate" />
		    </asp:Label>
		    <asp:TextBox ID="txtPostDate" runat="server" CssClass="date" MaxLength="25" />
		    
		    <asp:Label AssociatedControlID="txbEntryName" AccessKey="n" runat="server">
		        Entry <u>N</u>ame (page name) 
		        <asp:RegularExpressionValidator ID="vRegexEntryName" ControlToValidate="txbEntryName" ValidationExpression="^([a-zA-Z0-9-\.]*([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9-_]+)$" Text = "Invalid EntryName Format. Must only contain characters allowable in an URL." runat="server"/>
		    </asp:Label>
		    <asp:TextBox id="txbEntryName" runat="server" MaxLength="150" />
		    
		    <asp:Label AssociatedControlID="txbExcerpt" AccessKey="e" runat="server">
		        <u>E</u>xcerpt
		    </asp:Label>
		    <asp:TextBox id="txbExcerpt" runat="server" rows="5" textmode="MultiLine" MaxLength="500" />
		</div>
	</st:AdvancedPanel>
	
	<st:AdvancedPanel id="Enclosure" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" LinkText="[toggle]" Collapsible="False" Collapsed="True" HeaderText="Enclosure" BodyCssClass="Edit">
        <div id="messagePanelContainer" style="display: none;">
            <div id="messagePanelWrapper">
                <div id="messagePanel" style="display: none;">
                </div>
            </div>
        </div>
        <div id="enclosure-editor">
	        <fieldset title="Enclosure">
		        <legend>Basic Enclosure</legend>
	            <asp:Label AssociatedControlID="txbEnclosureTitle" accesskey="e" runat="server">
	                <u>E</u>nclosure Title
	            </asp:Label>
                <asp:TextBox id="txbEnclosureTitle" runat="server" MaxLength="250" />
                
                <asp:Label for="txbEnclosureUrl" AccessKey="u" runat="server">
                    Enclosure <u>U</u>rl 
                    <asp:RequiredFieldValidator id="valEncUrlRequired" runat="server" ControlToValidate="txbEnclosureUrl" ForeColor="#990066" ErrorMessage="Url is required" Display="Dynamic" /> <asp:RegularExpressionValidator ControlToValidate="txbEnclosureUrl" id="valEncUrlFormat" runat="server" ForeColor="#990066" ErrorMessage="Not a valid Url." ValidationExpression="^(https?://)?([\w-]+\.)+[\w-]+([\w-./?%&=:]*)?$" /> 
                </asp:Label>
                <asp:TextBox id="txbEnclosureUrl" runat="server" MaxLength="250" />
                
                <asp:Label AssociatedControlID="txbEnclosureSize" AccessKey="s" runat="server">
                    Enclosure <u>S</u>ize (in bytes) &nbsp;
                    <asp:RequiredFieldValidator id="valEncSizeRequired" runat="server" ControlToValidate="txbEnclosureSize" ForeColor="#990066" ErrorMessage="Size is required" Display="Dynamic" /><asp:RangeValidator ControlToValidate="txbEnclosureSize" MinimumValue="0" MaximumValue="999999999" id="valEncSizeFormat" runat="server" ForeColor="#990066"  Type="Integer" ErrorMessage="Not a valid size."/>
                </asp:Label>
                <asp:TextBox id="txbEnclosureSize" runat="server" MaxLength="250" />
                
                <asp:Label AssociatedControlID="ddlMimeType" AccessKey="m" runat="server">
                    Enclosure <u>M</u>imetype&nbsp;
                    <asp:CompareValidator Operator="NotEqual" ValueToCompare="none" id="valEncMimeTypeRequired" runat="server" ControlToValidate="ddlMimeType" ForeColor="#990066" ErrorMessage="MimeType is required." />
                </asp:Label>
                <asp:DropDownList ID="ddlMimeType" runat="server" />
                
                <asp:TextBox id="txbEnclosureOtherMimetype" CssClass="othertype" runat="server" MaxLength="25" />&nbsp;<asp:RequiredFieldValidator id="valEncOtherMimetypeRequired" runat="server" ControlToValidate="txbEnclosureOtherMimetype" ForeColor="#990066" ErrorMessage="You have to specify a custom mimetype." />
                
                <asp:Label AssociatedControlID="ddlDisplayOnPost" AccessKey="p" runat="server">
                    Display with <u>P</u>ost on website
                </asp:Label>
                
                <asp:DropDownList ID="ddlDisplayOnPost" runat="server">
                    <asp:ListItem Value="true">Yes</asp:ListItem>
                    <asp:ListItem Value="false">No</asp:ListItem>
                </asp:DropDownList>
                
                <asp:Label AssociatedControlID="ddlAddToFeed" AccessKey="f" runat="server">
                    Add to <u>F</u>eed
                </asp:Label>
                <asp:DropDownList ID="ddlAddToFeed" runat="server">
                    <asp:ListItem Value="true">Yes</asp:ListItem>
                    <asp:ListItem Value="false">No</asp:ListItem>
                </asp:DropDownList>
            </fieldset>
        </div>
	</st:AdvancedPanel>

<script type="text/javascript">

        /* ---- { a few global variables } ---- */
        var msgPanel = $('#messagePanel');
        var msgPanelWrap = msgPanel.parent();

        function ValidatorEnclosureEnable(enabled)
        {
            ValidatorEnable($("#<%= valEncUrlRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncSizeRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncMimeTypeRequired.ClientID %>")[0], enabled);
            ValidatorEnable($("#<%= valEncOtherMimetypeRequired.ClientID %>")[0], enabled);
        }

        function toggleOtherMimeType(elem)
        {
            if(elem != undefined) 
            {
                if(elem.value == "other")
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
        
        function enclosureEnabled()
        {

            if( $("#<%= txbEnclosureTitle.ClientID %>").val() != "")
            {
                return true;
            }
            if( $("#<%= txbEnclosureUrl.ClientID %>").val() != "")
            {
                return true;
            }
            if( $("#<%= txbEnclosureSize.ClientID %>").val() != "")
            {
                return true;
            }
            if( $("#<%= ddlMimeType.ClientID %>").val() != "none")
            {
                return true;
            }
            return false;
        }
        
        function toggleEnclosureBox()
        {
            if($("#<%= txbEnclosureTitle.ClientID %>").length==1)
            {
                if(enclosureEnabled())
                {
                    ValidatorEnclosureEnable(true);
                }
                else
                {
                    ValidatorEnclosureEnable(false);
                }
                toggleOtherMimeType($("#<%= ddlMimeType.ClientID %>")[0]);
            }
        }
        
        function setupChangeHandlers()
        {
            $("#<%= txbEnclosureTitle.ClientID %>").change(toggleEnclosureBox);
            $("#<%= txbEnclosureUrl.ClientID %>").change(toggleEnclosureBox);
            $("#<%= txbEnclosureSize.ClientID %>").change(toggleEnclosureBox);
            $("#<%= ddlMimeType.ClientID %>").change(toggleEnclosureBox);
        }

        // first let's hook up some events
        $(document).ready(function()
        {
            toggleEnclosureBox();

            $("#<%= ddlMimeType.ClientID %>").change(function() 
            {
                toggleOtherMimeType(this);
            });
            
            setupChangeHandlers();
            
            $("#<%= txbEnclosureUrl.ClientID %>").change(function()
            {
                var url = $("#<%= txbEnclosureUrl.ClientID %>").val();
                var requiredValidationPassed = $("#<%= valEncUrlRequired.ClientID %>")[0].style.display=="none";
                var formatValidationPassed = $("#<%= valEncUrlFormat.ClientID %>")[0].style.visibility=="hidden";

                if(url=="")
                {
                    $("#<%= txbEnclosureOtherMimetype.ClientID %>").val("");
                    $("#<%= ddlMimeType.ClientID %>").val("none");
                    toggleEnclosureBox();
                }

                if(requiredValidationPassed && formatValidationPassed)
                    {
                        hideMessagePanel();
                        ajaxServices.detectMimeType(url, function(response)
                        {
                            if(response.error)
                            {
                                handleError(response.error);
                            }
                            else
                            {
                                var mimetype = response.result;
                                if($("#<%= ddlMimeType.ClientID %> option").contents().is(":contains('"+mimetype+"')"))
                                {
                                    $("#<%= ddlMimeType.ClientID %>").val(mimetype);
                                    $("#<%= txbEnclosureOtherMimetype.ClientID %>").val("");
                                }
                                else
                                {
                                    $("#<%= ddlMimeType.ClientID %>").val("other");
                                    $("#<%= txbEnclosureOtherMimetype.ClientID %>").val(mimetype);
                                }
                                ValidatorValidate($("#<%= valEncMimeTypeRequired.ClientID %>")[0]);
                                toggleOtherMimeType($("#<%= ddlMimeType.ClientID %>")[0]);
                            }
                        });
                    }
            });
            
        });
       

        /* ---- { error handling methods } ---- */

        function handleError(error)
        {
            hideMessagePanel();
            msgPanelWrap.addClass("error");
            showMessagePanel(error.message);
        
            // available properties on error
            //error.errors -> [{error}, ...]
            //error.name
            //error.message
            //error.stackTrace
        }

        function showMessagePanel(message)
        {
            msgPanel.empty().append("<p>" + message + "</p>").fadeIn("slow");
        }
        
        function hideMessagePanel()
        {
            msgPanel.fadeOut();
            msgPanelWrap.removeClass("error").removeClass("warn").removeClass("info").removeClass("success");
        }
        

</script>
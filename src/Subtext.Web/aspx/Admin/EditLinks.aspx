<%@ Page Language="C#" Title="Subtext Admin - Edit Links" CodeBehind="EditLinks.aspx.cs" Inherits="Subtext.Web.Admin.Pages.EditLinks" %>

<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/aspx/Admin/UserControls/CategoryLinkList.ascx" %>
<asp:content contentplaceholderid="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:content>
<asp:content contentplaceholderid="categoryListHeading" runat="server">
    <h2>Categories</h2>
</asp:content>
<asp:content contentplaceholderid="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="LinkCollection" />
</asp:content>
<asp:content id="linkContent" contentplaceholderid="pageContent" runat="server">

<script language="javascript">

    /* ---- { Look at the xfnclient textbox, if empty do nothing, if not empty create array, iterate through array 
    and select/check appropriate input elements.} ---- */
    function checkElementsBasedOnLinkText() {
        var itemArray = new Array();
        var title = $('input.title').val();
        var relationships = $('input.relationship').val();
        
        if (relationships != null && title != null && title.length > 0) {
            if (relationships != 'me') {
                itemArray = $.map([relationships], function(value) { return value.split(' '); });
                resetCheckedState();
                jQuery.each(itemArray, function() {
                    var element = $('#' + this);
                    if (element.length > 0) {
                        CheckSelect(element);
                    }
                });
            }
            else {
                var eMe = $('#meRel');
                eMe.attr('checked', true);
                setRelationship();
            }
        }

        if (title != null && title.length == 0) {
            $('input.relationship').val('');
        }
    }

    function resetCheckedState() {
        $("#xfnRelations input[id!='meRel']").attr('checked', false);
        $("#xfnRelations input.none").attr('checked', true);
    }
    
    /* ---- { Select or check an element } ---- */
    function CheckSelect(element) {
        element.attr('checked', true);
        element.attr('select', true);
    }

    /* ---- { returns boolean if "me" is checked } ---- */
    function meChecked() {
        var undefined;
        var eMe = document.getElementById('meRel');
        if (eMe == undefined) return false;
        else return eMe.checked;
    }

    function setRelationshipTextInputValue() {
        var inputs = '';
        $("#xfnRelations input:checked[id!='meRel'][value]").each(function() {
            inputs += ' ' + $(this).val();
        });
        $('input.relationship').val(jQuery.trim(inputs));
    }

    function toggleMeChecked(e) {
        var meRel = $('#meRel');
        if (meRel.attr('checked')) {
            $('input.relationship').val(meRel.val());
            resetCheckedState();
        }
        else {
            $('input.relationship').val('');
        }
        $("#xfnRelations input[id!='meRel']").attr('disabled', meRel.attr('checked'));
    }

    $(function() {
        $("#xfnRelations input[id!='meRel']").click(function() {
            setRelationshipTextInputValue();
        });
        $("#xfnRelations input[id='meRel']").click(function(e) {
            toggleMeChecked(e);
        });
        $('input.relationship').change(function() {
            checkElementsBasedOnLinkText();
            setRelationshipTextInputValue();
        });

        checkElementsBasedOnLinkText();
    });
</script>
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<h2 id="headerLiteral" runat="server">Links</h2>
	<asp:Repeater id="rprSelectionList" runat="server">
		<HeaderTemplate>
			<table id="Listing" class="listing highlightTable" cellspacing="0" cellpadding="0" border="0" style="<%= CheckHiddenStyle() %>">
				<tr>
					<th>Link Title</th>
					<th width="50">Url</th>
					<th width="50">&nbsp;</th>
					<th width="50">&nbsp;</th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Url") %>
				</td>
				<td>
					<asp:linkbutton id="lnkEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" /></td>
				<td>
					<asp:linkbutton id="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" /></td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alt">
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Title") %>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "Url") %>
				</td>
				<td>
					<asp:linkbutton id="lnkEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Edit" runat="server" />
				</td>
				<td>
					<asp:linkbutton id="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text="Delete" runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>			
	<st:PagingControl id="resultsPager" runat="server" 
		PrefixText="<div>Goto page</div>" 
		LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
		UrlFormat="EditLinks.aspx?pg={0}" 
		CssClass="Pager" />
	<br class="clear" />

	<st:AdvancedPanel id="ImportExport" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleTitle" HeaderText="Import/Export" Collapsible="True" BodyCssClass="Edit"
		visible="false">
		<div style="HEIGHT: 0px"><!-- IE bug hides label in following div without this -->
			<div>
				<div>
					<p><label>Local File Location (*.opml)</label></p>
					<input class="FileUpload" id="OpmlImportFile" type="file" size="62" name="ImageFile" runat="server" />
					<p>Categories</p>
					<p>
						<asp:DropDownList id="ddlImportExportCategories" runat="server"></ASP:DropDownList></p>
				</div>
				<div class="button-div">
					<asp:Button id="lkbImportOpml" runat="server" CssClass="Button" Text="Import" onclick="lkbImportOpml_Click"></asp:Button><A class="Button" href="Export.aspx?command=opml">Export</A>
					<br class="clear" />
					&nbsp;
				</div>
			</div>
		</div>
	</st:AdvancedPanel>
	<asp:PlaceHolder id="Edit" runat="server">
	    <h2>Edit Link</h2>
		<fieldset>
		    <legend>Edit Link</legend>
			<label>Link ID</label>
			<asp:Label id="lblEntryID" runat="server" />
			
			<asp:Label AssociatedControlID="txbTitle" AccessKey="t" runat="server">Link <u>T</u>itle
			    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbTitle" ForeColor="#990066"
				ErrorMessage="Your link must have a title"></asp:RequiredFieldValidator>
			</asp:Label>
			<asp:TextBox id="txbTitle" runat="server" CssClass="textbox title" />
			<asp:Label AssociatedControlID="txbUrl" AccessKey="w" runat="server"><u>W</u>eb Url
			    <asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbUrl" ForeColor="#990066" ErrorMessage="Your link must have a url" />
			</asp:Label>
			<asp:TextBox id="txbUrl" runat="server" CssClass="textbox" />
		    <asp:Label AssociatedControlID="txbRss" AccessKey="r" runat="server"><u>R</u>ss Url</asp:Label>
			<asp:TextBox id="txbRss" runat="server" CssClass="textbox" />
			<asp:Label AssociatedControlID="txtXfn" AccessKey="l" runat="server"><u>L</u>ink relationship</asp:Label>
			<asp:TextBox ID="txtXfn" runat="server" CssClass="textbox relationship" />
			<div id="xfnRelations">			
			    <div>						
			        <h4>Link relationship helper</h4>
			        <span class="xfnTitleCol">identity</span>
			        <label for="meRel" class="xfnListOption">
			        <input type=checkbox id="meRel" value="me" />another web address of mine</label> 
			    </div>
    			
			    <div>
			        <span class="xfnTitleCol">friendship</span>
			        <label for="contact" class="xfnListOption">
				    <input class="xfnclass" type="radio" name="friendship" value="contact" id="contact"  /> contact</label>
				    <label for="acquaintance"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="friendship" value="acquaintance" id="acquaintance"  />  acquaintance</label>
				    <label for="friend"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="friendship" value="friend" id="friend"  /> friend</label>
				    <label for="friendship"  class="xfnListOption">
				    <input name="friendship" type="radio" class="xfnclass none" value="" id="friendship"  checked="checked" /> none</label>
			    </div>
    			
		        <div>
		            <span class="xfnTitleCol">physical</span>
		            <label for="met" class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="physical" value="met" id="met"  />met</label>
		        </div>
    		    
			    <div>
			        <span class="xfnTitleCol">professional</span>
			        <label for="co-worker"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="professional" value="co-worker" id="co-worker"  />
			        co-worker</label>
			        <label for="colleague"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="professional" value="colleague" id="colleague"  />
				     colleague</label>
			    </div>
    			
			    <div>
			        <span class="xfnTitleCol">geographical</span>
			        <label for="co-resident"  class="xfnListOption">
			        <input class="xfnclass" type="radio" name="geographical" value="co-resident" id="co-resident"  />
			        co-resident</label>
			        <label for="neighbor"  class="xfnListOption">
			        <input class="xfnclass" type="radio" name="geographical" value="neighbor" id="neighbor"  />
			        neighbor</label>
			        <label for="geographical"  class="xfnListOption">
			        <input class="xfnclass none" type="radio" name="geographical" value="" id="Radio3" checked="checked" />none</label>
		        </div>
    			
			    <div>
			        <span class="xfnTitleCol">family</span>
			        <label for="child"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="family" value="child" id="child"/>
				    child</label>
				    <label for="kin"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="family" value="kin" id="kin"  />
				    kin</label>
				    <label for="parent"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="family" value="parent" id="parent"  />
				    parent</label>
				    <label for="sibling"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="family" value="sibling" id="sibling" />
				    sibling</label>
				    <label for="spouse"  class="xfnListOption">
				    <input class="xfnclass" type="radio" name="family" value="spouse" id="spouse" />
				    spouse</label>
				    <label for="family"  class="xfnListOption">
				    <input class="xfnclass none" type="radio" name="family" value="" checked="checked" />none</label>
			    </div>		
			    <div>
			        <span class="xfnTitleCol">romantic</span>
			        <label for="muse"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="romantic" value="muse" id="muse"  />
			        muse</label>
			        <label for="crush"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="romantic" value="crush" id="crush"  />
			        crush</label>
			        <label for="date"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="romantic" value="date" id="date"  />
			        date</label>
			        <label for="romantic"  class="xfnListOption">
			        <input class="xfnclass" type="checkbox" name="romantic" value="sweetheart" id="sweetheart"  />
			        sweetheart</label>
			    </div>			
			</div>				
			
		    <label for="Edit_ddlCategories" AccessKey="c"><u>C</u>ategories</label>
			<asp:DropDownList id="ddlCategories" runat="server" />
			<span class="checkbox">
			    <asp:CheckBox id="ckbIsActive" runat="server" textalign="Left" Text="Visible" />
			</span>
			
			<div>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" onclick="lkbPost_Click" />
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" onclick="lkbCancel_Click" />
				&nbsp;
			</div>
		</fieldset>
	</asp:PlaceHolder>
</asp:content>

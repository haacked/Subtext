<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Links" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="EditLinks.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditLinks" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/Admin/UserControls/CategoryLinkList.ascx" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
    <h2>Categories</h2>
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="LinkCollection" />
</asp:Content>

<asp:Content ID="linkContent" ContentPlaceHolderID="pageContent" runat="server">

<script language="javascript">

/* ---- { Look at the xfnclient textbox, if empty do nothing, if not empty create array, iterate through array 
and select/check appropriate input elements.} ---- */
function SelectElements()
{
var itemArray = new Array();
var initialReturn = document.getElementById('<%= txtXfn.ClientID %>');
var title = document.getElementById('<%= txbTitle.ClientID %>');
if (initialReturn != null && title != null && title.value.length > 0) 
{
            if(initialReturn.value != 'me')
            {            
            itemArray = $.map([initialReturn.value],function(value){return value.split(' ');});
       
                for(i = 0;i<itemArray.length;i++)
                    {CheckSelect(itemArray[i]);}
            }                    
            else{
                    var eMe = document.getElementById('meRel');
                    eMe.checked = true;
                    upit();
                  }        
        
} 

if(title != null && title.value.length == 0)
    {
    initialReturn.value = '';
    }
}
window.onload = function(){
SelectElements();
}
/* ---- { Select or check an element } ---- */
function CheckSelect(object)
{
    var undefined;
    var inputObject = document.getElementById(object);
    if(inputObject != undefined)
        {
            inputObject.select = true;
            inputObject.checked = true;
        }
}

/* ---- { Get elements on page that have a common tag i.e input and class name i.e. xfnclass and populate array. } ---- */
function GetElementsWithClassName(elementName, className) {
	var allElements = document.getElementsByTagName(elementName);
	var elemColl = new Array();
	for (i = 0; i < allElements.length; i++) {
		if (allElements[i].className == className) {
			elemColl[elemColl.length] = allElements[i];
		}
	}
	return elemColl;
}

/* ---- { returns boolean if "me" is checked } ---- */
function meChecked() {
	var undefined;
	var eMe = document.getElementById('meRel');
	if (eMe == undefined) return false;
	else return eMe.checked;
}

/* ---- { On each onclick or keyup iterate through the input controls to see what is and is not checked displaying 
each value in the xfn textbox. } ---- */
function upit() {
	var isMe = meChecked(); 
	var inputColl = GetElementsWithClassName('input', 'xfnclass');
	var results = document.getElementById('<%= txtXfn.ClientID %>');
	var linkText, linkUrl, inputs = '';
	for (i = 0; i < inputColl.length; i++) {
		 inputColl[i].disabled = isMe;		
		 if (!isMe && inputColl[i].checked && inputColl[i].value != '') {
			inputs += inputColl[i].value + ' ';
				}
		 }
	inputs = inputs.substr(0,inputs.length - 1);
	
	if (isMe) inputs='me';
	results.value = inputs;
	}

/* ---- { Add onclick and onkeyup events to the input objects } ---- */
function blurry() {
	if (!document.getElementById) return;

	var aInputs = document.getElementsByTagName('input');

	for (var i = 0; i < aInputs.length; i++) {
		 aInputs[i].onclick = aInputs[i].onkeyup = upit;
	}
}

addLoadEvent(blurry);
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
			
			<label for="Edit_txbTitle" AccessKey="t">Link <u>T</u>itle
			    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txbTitle" ForeColor="#990066"
				ErrorMessage="Your link must have a title"></asp:RequiredFieldValidator>
			</label>
			<asp:TextBox id="txbTitle" runat="server" CssClass="textbox" />
			<label for="Edit_txbUrl" AccessKey="w"><u>W</u>eb Url
			    <asp:RequiredFieldValidator id="Requiredfieldvalidator2" runat="server" ControlToValidate="txbUrl" ForeColor="#990066"
				ErrorMessage="Your link must have a url" />
			</label>
			<asp:TextBox id="txbUrl" runat="server" CssClass="textbox" />
		    <label for="Edit_txbRss" AccessKey="r"><u>R</u>ss Url</label>
			<asp:TextBox id="txbRss" runat="server" CssClass="textbox" />
			<label for="txtXfn" accesskey="L"><u>L</u>ink relationship</label></strong><asp:TextBox ID="txtXfn" runat="server" CssClass="textbox" />
			<div id="xfnRelations">			
			<div>						
			<h4>Link relationship helper</h4>
			<span class="xfnTitleCol">identity</span>
			<label for="meRel" class="xfnListOption">
			<input type=checkbox id="meRel" />another web address of mine</label> 
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
						<input name="friendship" type="radio" class="xfnclass" value="" id="friendship"  checked="checked" /> none</label>
						
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
						<input class="xfnclass" type="radio" name="geographical" value="" id="Radio3"  checked="checked" />
						none</label>
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
						<input class="xfnclass" type="radio" name="family" value="" checked="checked" />
						none</label>
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
			    <asp:CheckBox id="chkNewWindow" runat="server" textalign="Left" Text="New Window" />
			</span>
			
		
			<div>
				<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" onclick="lkbPost_Click" />
				<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" onclick="lkbCancel_Click" />
				&nbsp;
			</div>
		</fieldset>
	</asp:PlaceHolder>
</asp:Content>

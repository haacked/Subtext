<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Customize.aspx.cs" Inherits="Subtext.Web.Admin.Pages.Customize" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
    <h2>Meta Tags</h2>
    <div id="messagePanel" class="notice"></div>
    <div id="dynamic-content">
        <fieldset>
            <legend>Meta Tags</legend>
            <div class="right">
                <button type="button" class="dynamic-add" title="Add a New Meta Tag">Add Meta Tag</button>
            </div>
            <div id="no-items-message" class="clear" <% if(ContainsTags) { %>style="display:none;" <% } %>>
                There are no Meta Tags created for this blog. Add some now!
             </div>
             <div id="items-wrapper" class="clear" <% if(!ContainsTags) { %>style="display:none;" <% } %>>
                <asp:Repeater ID="MetatagRepeater" runat="server">
                    <HeaderTemplate>
                        <table id="dynamic-table" class="listing highlightTable">
                            <tbody>
                                <tr>
                                    <th>Name</th>
                                    <th>Content</th>
                                    <th>Http-Equiv</th>
                                    <th>Action</th>
                                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="dynamic-<%# EvalTag(Container.DataItem).Id %>">
                            <td>
                                <%# EvalName(Container.DataItem) %>
                            </td>
                            <td>
                                <%# EvalContent(Container.DataItem) %>
                            </td>
                            <td>
                                <%# EvalHttpEquiv(Container.DataItem) %>
                            </td>
                            <td>
                                <button type="button" class='dynamic-edit' title='Edit Meta Tag'>Edit</button> 
                                <button type="button" class='dynamic-delete' title='Delete Meta Tag'>Delete</button>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alt" id="dynamic-<%# EvalTag(Container.DataItem).Id %>">
                            <td>
                                <%# EvalName(Container.DataItem) %>
                            </td>
                            <td>
                                <%# EvalContent(Container.DataItem) %>
                            </td>
                            <td>
                                <%# EvalHttpEquiv(Container.DataItem) %>
                            </td>
                            <td>
                                <button type="button" class="dynamic-edit" title="Edit Meta Tag">Edit</button> 
                                <button type="button" class='dynamic-delete' title='Delete Meta Tag'>Delete</button>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tr id="dynamic-add-row" style="display: none;">
                            <td>
                                <input type="text" class="textbox" />
                            </td>
                            <td>
                                <input type="text" class="textbox" />
                            </td>
                            <td>
                                <input type="text" class="textbox" />
                            </td>
                            <td>
                                <button type="button" class="dynamic-save" title="Save the Meta Tag">Save</button> 
                                <button type="button" class="dynamic-cancel" title="Cancel Changes">Cancel</button>
                            </td>
                        </tr>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                
                <div class="Pager">
                    <st:PagingControl id="resultsPager" runat="server" 
		                PrefixText="<div>Goto page</div>" 
		                LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
		                UrlFormat="Customize.aspx?pg={0}" 
		                CssClass="Pager" />
		            </div>
            </div>
        </fieldset>
    </div>

    <script type="text/javascript">
    
        /* ---- { some objects and enums } ---- */
        
        function MetaTag()
        {
            this.id = null;
            this.name = null;
            this.content = null;
            this.httpEquiv = null;
        }
        
        var MetaTagAction = {
            add: 0,
            remove: 1,
            edit: 2,
            undo: 3,
            readonly: 4 };
        
        
        /* ---- { a few global variables } ---- */
        var msgPanel = $('#messagePanel');
        var noItemsMessage = $('#no-items-message');
        var itemsWrapDiv = $('#items-wrapper');
        var DELETE_BUTTONS_TEMPLATE = "<button type='button' class='dynamic-edit' title='Edit Meta Tag'>Edit</button> <button type='button' class='dynamic-delete' title='Delete Meta Tag'>Delete</button>"
        var SAVE_BUTTONS_TEMPLATE = "<button type='button' class='dynamic-save' title='Save the Meta Tag'>Save</button> <button type='button' class='dynamic-cancel' title='Cancel Changes'>Cancel</button>";
        
        // a global variable for un-doing an operation
        var undoOperation = null;
        
        /* ---- { page and event setup } ---- */
        function onAddClick()
        {
            itemsWrapDiv.show();
            noItemsMessage.hide();

            var theRow = $("#dynamic-add-row");
            if (!$.browser.msie) {
                // Workaround a bug in jQuery 1.3.2 with IE8
                theRow.fadeIn("slow", function() { $(":input", theRow)[0].focus(); });
            }
            else {
                theRow.attr('style', 'display:block');
                $(":input", theRow)[0].focus();
            }
        }       
        
        // first let's hook up some events
        $(document).ready(function()
        {
            // wire up the Add Button handler
            $(".dynamic-add").click(function() 
            {
                onAddClick();
            });
            
            // next is the Edit Button handlers
            $("tr[id^='dynamic-'] .dynamic-edit").click(function()
            {
                // grab the table row that holds this meta tag
                var editRow = $(this).parents("tr[id^='dynamic-']");
                setupEditUI(editRow);
            });
            
            // wire up the Delete Button handlers
            $("tr[id^='dynamic-'] .dynamic-delete").click(function()
            {
                var deleteRow = $(this).parents("tr[id^='dynamic-']");
                deleteItem(deleteRow);
            });
            
            // now wire up the save and cancel buttons
            $(".dynamic-save").click(saveMetaTag);
            $(".dynamic-cancel").click(clearAndHideAddMetaTagUI);
            
            // setup some hotkeys
            $.hotkeys.add(
            	"return",
                { target:jQuery("#dynamic-add-row")[0] },
                function(){ $(".dynamic-save").click(); });
        });
        
        
        /* ---- { Meta Tag methods } ---- */
        
        function clearAndHideAddMetaTagUI()
        {
            var theRow = $("#dynamic-add-row");
            
            theRow.fadeOut("fast");
            
            $(":input[type='text']", theRow).each(function() 
                {
                    $(this).val("");
                });
            
            if (getActiveitemRows().length == 0)
            {
                itemsWrapDiv.hide();
                noItemsMessage.fadeIn();
            }
        }
        
        function saveMetaTag()
        {
            hideMessagePanel();
            
            var addRow = $(this).parents("tr[id^='dynamic-']");
            createMetaTag(getMetaTagForAction(MetaTagAction.add, addRow));
        }
        
        function createMetaTag(metaTag)
        {
            // unbind the click event so a user can't click it until the current action is done.
            var addBtn = $(".dynamic-save");
            addBtn.unbind("click").fadeTo("fast", .5);
            
            metaTag = ajaxServices.addMetaTagForBlog(metaTag.content, metaTag.name, metaTag.httpEquiv, function(response)
                {
                    // wire the click events back up.
                    $(".dynamic-save").bind("click", saveMetaTag).fadeTo("normal", 1);
                    
                    if (response.error)
                        handleError(response.error);
                    else
                    {
                        clearAndHideAddMetaTagUI();
                        onItemCreated(response.result);
                    }
                });
        }
        
        function onItemCreated(metaTag)
        {
            hideMessagePanel();
            noItemsMessage.hide();
            itemsWrapDiv.fadeIn("normal");
            
            msgPanel.addClass("success");
            showMessagePanel("Meta Tag successfully added. Tag id = " + metaTag.id + ".");
            
            // add the new metatag to the table
            var tableRow = "<tr class='new' style='display:none'><td>" + checkForNull(metaTag.name) + "</td>";
            tableRow += "<td>" + checkForNull(metaTag.content) + "</td>";
            tableRow += "<td>" + checkForNull(metaTag.httpEquiv) + "</td>";
            tableRow += "<td>" + DELETE_BUTTONS_TEMPLATE + "</td></tr>";
            
            $("#dynamic-add-row").before(tableRow);
            var newRow = $("#dynamic-table tr.new:last");
            
            newRow.attr('id', 'dynamic-' + metaTag.id);
            
            // now wire up the events for the row's buttons
            $('.dynamic-edit', newRow).click(function()
            {
                setupEditUI(newRow);
            });
            $('.dynamic-delete', newRow).click(function()
            {
                deleteItem(newRow);
            });
            
            newRow.show();
            newRow.animate( { backgroundColor: 'transparent' }, 5000);
            onAddClick();
        }
        
        function setupEditUI(itemRow)
        {
            var cells = $("td", itemRow);
            var currTag = getMetaTagForAction(MetaTagAction.readonly, itemRow);
            
            // replace the edit/delete buttons w/ save/cancel buttons
            var cell = $(cells[3]);
            cell.html(SAVE_BUTTONS_TEMPLATE);
            cell.append("<input type='hidden' value='" + JSON.stringify(currTag) + "' /");
            
            // could use the currTag, but wanted to save a little typing so a simple loop works
            for (i = 0; i < 3; i++)
            {
                cell = $(cells[i]);
                var currValue = cell.text().trim();
                cell.html("<input type='text' />");
                $(":input", cell).val(currValue);
            }
            
            $(".dynamic-save", itemRow).click(function()
            {
                var editRow = $(this).parents("tr[id^='dynamic-']");
                var tag = getMetaTagForAction(MetaTagAction.edit, editRow);
                
                var updatedTag = ajaxServices.updateMetaTag(tag, function(response)
                    {
                        if (response.error)
                            handleError(response.error);
                        else
                        {
                            // update the UI to let the user know we're done
                            editRow.attr("style","");
                            setupReadOnlyEditRow(editRow, tag);
                            editRow.addClass("updated");
                            
                            hideMessagePanel();
                            msgPanel.addClass("success");
                            showMessagePanel("Meta Tag successfully udpated.");
                            
                            editRow.animate( { backgroundColor: 'transparent' }, 5000);
                        }
                    });
            });
            
            $(".dynamic-cancel", itemRow).click(function()
            {
                var editRow = $(this).parents("tr[id^='dynamic-']");
                var origTag = JSON.parse($("td input:hidden", itemRow).val());
                
                setupReadOnlyEditRow(editRow, origTag);
            });
        }
        
        function setupReadOnlyEditRow(itemRow, readOnlyMetaTag)
        {
            var cells = $("td", itemRow);
            
            // replace the edit/delete buttons w/ save/cancel buttons
            $(cells[3]).html(DELETE_BUTTONS_TEMPLATE);
            
            var cell = $(cells[0]);
            cell.html(checkForNull(readOnlyMetaTag.name));
            
            cell = $(cells[1]);
            cell.html(checkForNull(readOnlyMetaTag.content));
            
            cell = $(cells[2]);
            cell.html(checkForNull(readOnlyMetaTag.httpEquiv));
            
            $(".dynamic-edit", itemRow).click(function()
            {
                setupEditUI(itemRow);
            });
            
            // wire up the Delete Button handlers
            $(".dynamic-delete", itemRow).click(function()
            {
                deleteItem(itemRow);
            });
        }
        
        function deleteItem(itemRow)
        {
            // partially fade the row and then unbind the click event so the buttons can't be clicked again.
            itemRow.fadeTo("fast", .6);
            $("button", itemRow).unbind("click").disabled=true;
            hideMessagePanel();
            
            undoOperation = getMetaTagForAction(MetaTagAction.remove, itemRow);
            
            ajaxServices.deleteMetaTag(undoOperation.id, function(response)
                {
                    if (response.error)
                        handleError(response.error);
                    else
                        onItemDeleted(response.result, itemRow);
                });
        }
        
        function onItemDeleted(isDeleted, itemRow)
        {
            hideMessagePanel();
            
            if (!isDeleted)
            {
                msgPanel.addClass("warn");
                showMessagePanel("Could not delete the meta tag... perhaps it's already gone!");
                return;
            }
        
            // fade the row all the way out and the remove it's contents from the DOM.
            itemRow.fadeOut(function()
            {
                itemRow.empty();
                
                if (getActiveitemRows().length == 0)
                {
                    itemsWrapDiv.hide();
                    noItemsMessage.fadeIn("normal");
                }
            });
            
            msgPanel.addClass("success");
            showMessagePanel("The meta tag was successfully deleted. <button type='button' title='Bring back your tag!'>Undo</button>");
            
            var undoBtn = msgPanel.find("button");
            undoBtn.click(undoAction);
        }
        
        function undoAction()
        {
            createMetaTag(getMetaTagForAction(MetaTagAction.undo));
        }
        
        /* ---- { helper methods } ---- */
        
        function handleError(error)
        {
            hideMessagePanel();
            msgPanel.addClass("error");
            showMessagePanel(error.message);
        
            // available properties on error
            //error.errors -> [{error}, ...]
            //error.name
            //error.message
            //error.stackTrace
        }
        
        function getMetaTagForAction(actionType, itemRow)
        {
            var tag = new MetaTag();
            // if adding or editing a tag, collect the values from the form
            if (actionType == MetaTagAction.add || actionType == MetaTagAction.edit)
            {
                var inputBoxes = $(":input", itemRow);
                
                tag.name = $(inputBoxes[0]).val().trim();
                tag.content = $(inputBoxes[1]).val().trim();
                tag.httpEquiv = $(inputBoxes[2]).val().trim();
                
                if (actionType == MetaTagAction.edit)
                    tag.id = itemRow.attr('id').split('dynamic-').pop();
                
                return tag;
            }
            else if (actionType == MetaTagAction.remove || actionType == MetaTagAction.readonly)
            {
                var cells = itemRow.children('td');
                
                tag.id = itemRow.attr('id').split('dynamic-').pop();
                tag.name = returnNullForEmpty($(cells[0]).text().trim());
                tag.content = $(cells[1]).text().trim();
                tag.httpEquiv = returnNullForEmpty($(cells[2]).text().trim());
                
                return tag;
            }
            else if (actionType == MetaTagAction.undo)
            {
                return undoOperation;
            }
            
            return null;
        }
        
        function showMessagePanel(message)
        {
            msgPanel.empty().append("<p>" + message + "</p>").fadeIn("slow");
        }
        
        function hideMessagePanel()
        {
            msgPanel.fadeOut();
            msgPanel.removeClass("error").removeClass("warn").removeClass("info").removeClass("success");
        }
        
        function getActiveitemRows()
        {
            return $("tr[id^='dynamic-'][id!='dynamic-add-row']:visible", "#dynamic-table");
        }
        
        function returnNullForEmpty(val)
        {
            if (val == null || val.length > 0)
                return val;
                
            return null;
        }
        
        function checkForNull(val)
        {
            return val == null ? "" : val;
        }
    </script>

</asp:Content>

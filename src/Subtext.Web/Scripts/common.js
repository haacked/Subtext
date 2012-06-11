var Subtext;
if (!Subtext) {
    Subtext = {};
}

jQuery(function() {
    String.prototype.trim = function() {
        return jQuery.trim(this);
    };

    jQuery('a.close').click(function() {
        jQuery(jQuery(this).attr('href')).slideUp();
    });
});


/*-------------------------------------------------*/
/* Disables submit button during update panel post */
/*-------------------------------------------------*/
function initializeRequest(sender, args) {
    //Disable button to prevent double submit
    var button = $get(args._postBackElement.id);
    if (button) 
    {
        button.disabled = true;
        button.oldValue = button.value;
        if (button.oldValue && button.oldValue !== '') {
            button.value = 'Posting...';
        }
        if (button.className == 'buttonSubmit') 
        {
            button.className = 'button-disabled';
        }
    }
}

function endRequest(sender, args) 
{
    //Re-enable button
    var button = $get(sender._postBackSettings.sourceElement.id);
    if (button) 
    {
        button.disabled = false;
        button.value = button.oldValue;
        if (button.className == 'button-disabled') 
        {
            button.className = 'buttonSubmit';
        }
    }
}

function pageLoad(sender, args) {
    var requestManager = Sys.WebForms.PageRequestManager.getInstance();
    requestManager.add_initializeRequest(initializeRequest);
    requestManager.add_endRequest(endRequest);
}
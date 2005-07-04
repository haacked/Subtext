
// Toggles whether every checkbox is checked 
// or not checked based on the state of the 
// specified checkbox.
function ToggleCheckAll(checkbox)
{
	var form = checkbox.form;
	if(checkbox.checked)
	{
		SetAllCheckboxes(form, true);
	}
	else
	{
		SetAllCheckboxes(form, false);
	}
}

// Sets the checked property of all 
// checkboxes to the specified value.
function SetAllCheckboxes(form, checked)
{
	for(var i = 0; i < form.elements.length; i++)
	{
		if(form.elements[i].type == 'checkbox')
		{
			form.elements[i].checked = checked;
		}
	}
}

function toggleHideOnCheckbox()
{
}
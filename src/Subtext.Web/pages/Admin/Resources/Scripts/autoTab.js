/*
Adapted from http://particletree.com/features/10-tips-to-a-better-form/
Item #3

To flag an input as auto-tabbed...
<input type="text" name="areacode" class="autoTab" tabindex="1" maxlength="3"  />

*/
function autoTab(e) 
{
    if(this.value.length == this.getAttribute("maxlength") && 
        e.KeyCode != 8 && e.keyCode != 16 && e.keyCode != 9) 
    {
            new Field.activate(findNextElement(this.getAttribute("tabindex")));
    }
}

function findNextElement(index) 
{
    elements = new Form.getElements('shippingInfo');
    for(i = 0; i < elements.length; i++) 
    {
        element = elements[i];
        if(parseInt(element.getAttribute("tabindex")) == (parseInt(index) + 1)) 
        {
            return element;
        }
    }
    return elements[0];
}
/*
Adapted from http://particletree.com/features/10-tips-to-a-better-form/
Item #8
*/
function initLabels() 
{
    labels = document.getElementsByTagName("label");
    for(i = 0; i < labels.length; i++) {
        addEvent(labels[i], "click", labelFocus);
    }
}

function labelFocus() 
{
    new Field.focus(this.getAttribute('for'));
}

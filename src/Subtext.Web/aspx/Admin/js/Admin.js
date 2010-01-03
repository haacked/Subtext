/*--------------------------------------------------*/
/* Add behaviors to elements based on CSS classes   */
/*--------------------------------------------------*/
$(function() {
    /* Check all checkboxes toggle */
    $('.check-all').click(function() {
        var checked = this.checked;
        $('input[type=checkbox]').each(function() {
            this.checked = checked;
        });
    });

    /* Delete confirmation */
    $(".confirm-delete").click(function(evt) {
        var ans = confirm("Are you sure you want to delete this?");
        if (!ans) {
            evt.preventDefault();
        }
    });
});

$(function() {
    $('table.highlightTable tr').mouseover(function() {
        $(this).addClass('highlight');
    });
    $('table.highlightTable tr').mouseout(function() {
        $(this).removeClass('highlight');
    });
    $('table.highlightTable tr:even').addClass('even');
    $('table.highlightTable tr:odd').addClass('odd');
});

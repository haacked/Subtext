/*
Method for applying a specific target and style to 
external links.  Adapted from 
*/

$(function() {
    $("a[href][rel*='external']").each(function() {
        $(this).addClass('external');
        $(this).attr('title', jQuery.trim($(this).attr('title') + ' (new window)'));
        $(this).attr('target', '_blank');
    });
});
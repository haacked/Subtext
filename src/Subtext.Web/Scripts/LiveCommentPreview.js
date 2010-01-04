/*
* LivePreview jQuery Plugin v1.0
*
* Copyright (c) 2009 Phil Haack, http://haacked.com/
* Licensed under the MIT license.
*/

(function($) {
    $.fn.livePreview = function(options) {
        var opts = $.extend({}, $.fn.livePreview.defaults, options);
        var previewMaxIndex = opts.previewElement.length - 1;

        var allowedTagsRegExp = new RegExp("&lt;(/?(" + opts.allowedTags.join("|") + ")(\\s+.*?)?)&gt;", "g");

        return this.each(function(i) {
            var textarea = $(this);
            var preview = $(opts.previewElement[Math.min(i, previewMaxIndex)]);

            textarea.handleKeyUp = function() {
                textarea.unbind('keyup', textarea.handleKeyUp);
                if (!preview.updatingPreview) {
                    preview.updatingPreview = true;
                    window.setTimeout(function() { textarea.reloadPreview(); }, opts.interval);
                }
                return false;
            };

            textarea.htmlUnencode = function(s) {
                return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            };

            textarea.reloadPreview = function() {
                var previewString = this.val();
                if (previewString.length > 0 && previewString.indexOf('<') > -1) {
                    previewString = this.htmlUnencode(previewString);
                    previewString = previewString.replace(opts.paraRegExp, "<p>$1</p><p>$2</p>");
                    previewString = previewString.replace(opts.lineBreakRegExp, "$1<br />$2");
                    previewString = previewString.replace(allowedTagsRegExp, "<$1>");
                }

                try {
                    // Workaround for a bug in jquery 1.3.2 which is fixed in 1.4
                    preview[0].innerHTML = previewString;
                }
                catch (e) {
                    alert("Sorry, but inserting a block element within is not allowed here.");
                }

                preview.updatingPreview = false;
                this.bind('keyup', this.handleKeyUp);
            };

            textarea.reloadPreview();
        });

    };

    $.fn.livePreview.defaults = {
        paraRegExp: new RegExp("(.*)\n\n([^#*\n\n].*)", "g"),
        lineBreakRegExp: new RegExp("(.*)\n([^#*\n].*)", "g"),
        allowedTags: ['a', 'b', 'strong', 'blockquote', 'p', 'i', 'em', 'u', 'strike', 'super', 'sub', 'code'],
        interval: 80
    };

})(jQuery);

// This extra bit is specific to Subtext. Makes it so it works automatically.
$(function(){
    $('textarea.livepreview').livePreview({
        previewElement: $('div.livepreview'),
        allowedTags: subtextAllowedHtmlTags, 
        interval: 20
    });
});
(function($) {
    $.fn.undoable = function(options) {
        var opts = $.extend({}, $.fn.undoable.defaults, options);

        return this.each(function(i) {
            $this = $(this);

            $this.click(function() {
                var clickSource = $(this);
                clickSource.attr('disabled', 'disabled');
                var target = (opts.getTarget || $.fn.undoable.getTarget).call($.fn.undoable, clickSource);
                var url = opts.url;
                var data = (opts.getPostData || $.fn.undoable.getPostData).call($.fn.undoable, clickSource, target);

                $.fn.undoable.postToServer(url, data, function(response) {
                    var undoData = $.extend(response, data);
                    target.hide();
                    target.inlineStyling = opts.inlineStyling;
                    var undoable = (opts.showUndo || $.fn.undoable.showUndo).call($.fn.undoable, target, undoData, opts);

                    undoable.find('.undo a').click(function() {
                        $(this).attr('disabled', 'disabled');
                        
                        var data = (opts.getUndoPostData || $.fn.undoable.getUndoPostData).call($.fn.undoable, clickSource, target, opts);
                        $.fn.undoable.postToServer(opts.undoUrl || url, data, function() {
                            (opts.hideUndo || $.fn.undoable.hideUndo).call($.fn.undoable, undoable, target, opts);
                            clickSource.removeAttr('disabled');
                        }, clickSource);
                        return false;
                    });
                }, clickSource);

                return false;
            });
        });
    };

    $.fn.undoable.getTarget = function(clickSource) {
        var tr = clickSource.closest('tr');
        if (tr.length === 0) {
            return clickSource.closest('.target');
        }
        return tr;
    };

    $.fn.undoable.getPostData = function(clickSource, target) {
        return { id: clickSource.attr('href').substr(1) };
    };

    $.fn.undoable.getUndoPostData = function(clickSource, target, options) {
        return (options.getPostData || this.getPostData).call(this, clickSource, target, options);
    };

    $.fn.undoable.showUndo = function(target, data, options) {
        var message = (options.formatStatus || this.formatStatus).call(this, data);

        if (target[0].tagName === 'TR') {
            var colSpan = target.children('td').length;
            target.after('<tr class="undoable"><td class="status" colspan="' + (colSpan - 1) + '">' + message + '</td><td class="undo"><a href="#' + data.id + '">undo</a></td></tr>');
        }
        else {
            var tagName = target[0].tagName;
            var classes = target.attr('class');
            target.after('<' + tagName + ' class="undoable ' + classes + '"><p class="status">' + message + '</p><p class="undo"><a href="#' + data.id + '">undo</a></p></' + tagName + '>');
        }

        var undoable = target.next();

        if (options.showingStatus) {
            options.showingStatus(undoable);
        }

        undoable.hide().fadeIn('slow').show();
        if (target.inlineStyling) {
            (options.applyUndoableStyle || $.fn.undoable.applyUndoableStyle).call($.fn.undoable, undoable);
        }
        return undoable;
    };

    $.fn.undoable.hideUndo = function(undoable, target, options) {
        if (options.hidingStatus) {
            options.hidingStatus(undoable, target);
        }
        undoable.remove();
        target.fadeIn('slow').show();
        return target;
    };

    $.fn.undoable.formatStatus = function(data) {
        return '<strong class="subject">' + data.subject + '</strong> <span class="predicate">' + data.predicate + '</span>';
    };

    $.fn.undoable.applyUndoableStyle = function(undoable) {
        undoable.css('backgroundColor', '#e0e0e0');
        undoable.css('color', '#777777');
        var styled = undoable;
        if (undoable[0].tagName === 'TR') {
            styled = undoable.children('td');
            undoable.children('td:first').css('borderLeft', 'solid 2px #bbbbbb');
        }
        else {
            styled.css('borderLeft', 'solid 2px #bbbbbb');
        }
        styled.css('text-align', 'center');
        styled.css('borderTop', 'solid 2px #bbbbbb');
        styled.css('borderBottom', 'solid 1px #cccccc');
    };

    $.fn.undoable.postToServer = function(url, data, success, clickSource) {
        if (url) {
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                data: data,
                success: success
            });
        }
        else {
            // demo mode
            success({ subject: 'The item', predicate: 'has been removed' });
        }
    };

    $.fn.undoable.defaults = {
        /* Applies some default styling if true. If false, all styling is done via CSS class */
        inlineStyling: true
    };

})(jQuery);
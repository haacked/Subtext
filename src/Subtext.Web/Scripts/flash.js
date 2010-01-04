(function($) {
    $.fn.flash = function(options) {
        return this.each(function(i) {
            var bg = $(this).css('background-color');
            $(this).css('opacity', 0)
                .animate({ opacity: 1.0, backgroundColor: options.bg || 'khaki' }, 800)
                .animate({ backgroundColor: bg }, 350, function() {
                    this.style.removeAttribute('filter');
                });
        });
    };
})(jQuery);
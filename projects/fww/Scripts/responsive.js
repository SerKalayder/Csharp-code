$(function() {
    var menu_button = $('#menu-button');
        menu = $('.to-be-expanded nav');
        //menuHeight = menu.height();
 
        $(menu_button).on('click', function (e) {
        e.preventDefault();
        menu.slideToggle();
    });
});

/*
$(window).resize(function () {
    var w = $(window).width();
    if (w > 320 && menu.is(':hidden')) {
        menu.removeAttr('style');
    }
});
*/

$(document).ready(function () {
    //$(".first-li").addClass("openul").children('ul').show();

    $(".main-li > a").on('click', function () {
        var getli = $(this).parent('li');
        if (getli.hasClass('openul'))
        {
            getli.removeClass('openul');
            getli.find('ul').slideUp(200);
        }
        else {
            getli.addClass('openul');
            getli.children('ul').slideDown(200);
        }
    });
})

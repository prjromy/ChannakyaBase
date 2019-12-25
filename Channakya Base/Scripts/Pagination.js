$(document).ready(function () {
    $.fn.pagination(1, 0, 0)

$('.pagination').on('click', '.pagerClass', function () {
    debugger;
    var pagingData = $(this).attr('id');
    $.fn.pagination(pagingData, 0, 0);

});

$('.pagination').on('click', 'li.next , li.previous', function () {
    debugger;
    var cls = $(this).attr('class');
    if ($(this).attr('class') == 'next') {
        var pager = $('ul.pagination.employeepaginatoin').find('.active').attr('id');
        var finalId = parseInt(pager) + 1;
        var hasNext = $('ul.pagination.employeepaginatoin').find('li#' + finalId)
        if ($(hasNext).length > 0) {
            $.fn.pagination(finalId, 0, 0)
        }
    }
    else {
        var pager = $('ul.pagination.employeepaginatoin').find('.active').attr('id');
        var finalId = parseInt(pager) - 1;
        var hasPrev = $('ul.pagination.employeepaginatoin').find('li#' + finalId)
        if ($(hasPrev).length > 0) {
            $.fn.pagination(finalId, 0, 0)
        }
    }

});
});

$.fn.pagination = function (pagerId, next, previous) {
    debugger;
    var checkActive = $('ul.pagination.employeepaginatoin').find('li').hasClass('active');
    if (checkActive == true) {
        $('ul.pagination.employeepaginatoin').find('.active').removeClass('active');
    }
    else {
        var listcount = 0;
        $('.detailsList').each(function (index, item) {
            listcount++;
        });
        var i;
        var mod = listcount / 10;
        for (i = 1; i <= mod + 1; i++) {
            debugger;
            $('ul.pagination.employeepaginatoin').append('<li id=' + i + ' class="pagerClass"><a>' + i + '</a> </li>')
        }
    }
    $('ul.pagination.employeepaginatoin').find('li#' + pagerId).addClass('active');
    var pagerRange = 10;
    var paginatonData = pagerId * pagerRange;
    $('.detailsList').each(function (index, item) {
        debugger;
        if (paginatonData > pagerRange) {
            var checkPagination = paginatonData - pagerRange;
            if (index <= paginatonData && index > checkPagination) {
                var text = $(item).find('.inner').attr('Employee-caption').toUpperCase();
                $(item).css('display', 'table-row');
            }
            else {
                var text = $(item).find('.inner').attr('Employee-caption').toUpperCase();
                $(item).css('display', 'none');
            }
        }
        else {
            if (index <= paginatonData) {
                var text = $(item).find('.inner').attr('Employee-caption').toUpperCase();
                $(item).css('display', 'table-row');
            }
            else {
                var text = $(item).find('.inner').attr('Employee-caption').toUpperCase();
                $(item).css('display', 'none');
            }
        }

    });
};


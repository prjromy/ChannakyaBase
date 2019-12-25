$(document).ready(function () {
    $(document).ready(function () {
        $('.pagination').on('click', 'li.next , li.previous', function () {
            debugger;
            var Search = $('.Search ').val();
            var cls = $(this).attr('class');
            if ($(this).attr('class') == 'next') {
                var pager = $('ul.pagination.employeepaginatoin').find('.active').attr('id');
                var finalId = parseInt(pager) + 1;
                var hasNext = $('ul.pagination.employeepaginatoin').find('li#' + finalId)
                if ($(hasNext).length > 0) {
                    var container = $(this).closest('.modal-body').find('.bodycontent-and-paging');
                    $(container).load("/Voucher1/_AddVoucherDetails", { query: Search, page: finalId });
                    $('#getVoucherDetails').modal('show');

                    var checkActive = $('ul.pagination.employeepaginatoin').find('li').hasClass('active');
                    if (checkActive == true) {
                        $('ul.pagination.employeepaginatoin').find('.active').removeClass('active');
                    }
                    $('ul.pagination.employeepaginatoin').find('li#' + finalId).addClass('active');
                }
            }
            else {
                var pager = $('ul.pagination.employeepaginatoin').find('.active').attr('id');
                var finalId = parseInt(pager) - 1;
                var hasPrev = $('ul.pagination.employeepaginatoin').find('li#' + finalId)

                if ($(hasPrev).length > 0) {
                    //var container = $(this).closest('.Voucher1-explore').find('#getVoucherDetails');
                    var container = $(this).closest('.modal-body').find('.bodycontent-and-paging');
                    $(container).load("/Voucher1/_AddVoucherDetails", { query: Search, page: finalId });
                    $('#getVoucherDetails').modal('show');

                    var checkActive = $('ul.pagination.employeepaginatoin').find('li').hasClass('active');
                    if (checkActive == true) {
                        $('ul.pagination.employeepaginatoin').find('.active').removeClass('active');
                    }
                    $('ul.pagination.employeepaginatoin').find('li#' + finalId).addClass('active');
                }
            }

        });
        $('.search-container').on('blur', '.searchPager', function () {
            debugger;
            var finalId = $(this).val();
            var Search = $('.Search ').val();
            var container = $(this).closest('.modal-body').find('.bodycontent-and-paging');
            // var container = $(this).closest('#getVoucherDetails').find('.modal-body');
            if (isNaN(finalId) == false || finalId != 0) {
                $.ajax({
                    url: "/Voucher1/_AddVoucherDetails",
                    type: "POST",
                    data: { query: Search, page: finalId },
                    success: function (data) {
                        debugger;
                        $(container).html(data);
                        $(container).focus();

                    }

                });
            }
            // $(subsiContainer).load("/VoucherBalance/AddSubsiBalance", { ledgerId: ledgerId, page: finalId });

            var checkActive = $('ul.pagination.employeepaginatoin').find('li').hasClass('active');
            if (checkActive == true) {
                $('ul.pagination.employeepaginatoin').find('.active').removeClass('active');
            }
            $('ul.pagination.employeepaginatoin').find('li#' + finalId).addClass('active');

        });
        $('.pagination').on('click', '.pagerClass', function () {
            debugger;
            var pagingData = $(this).attr('id');
            var Search = $('.Search ').val();
            var container = $(this).closest('.modal-body').find('.bodycontent-and-paging');
            $(container).load("/Voucher1/_AddVoucherDetails", { query: Search, page: pagingData });
            $('#getVoucherDetails').modal('show');
            var checkActive = $('ul.pagination.employeepaginatoin').find('li').hasClass('active');
            if (checkActive == true) {
                $('ul.pagination.employeepaginatoin').find('.active').removeClass('active');
            }
            $('ul.pagination.employeepaginatoin').find('li#' + pagingData).addClass('active');

        });

    });

});
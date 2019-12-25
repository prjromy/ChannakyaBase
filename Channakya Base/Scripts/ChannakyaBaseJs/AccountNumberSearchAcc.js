var _globalObject;
$("button.btn-account-open-search").on('click', function (e) {
    debugger;
    subsiEvent = $(this);
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber").attr("id");
    var filterAccount = $(this).closest("div.account-number-div").find(".account-aumber#" + _globalObject).attr("accountFilter")
    var accountType = $(this).closest("div.account-number-div").find(".account-aumber#" + _globalObject).attr("accountType")
    var accountNumber = $("#" + _globalObject).val();
    var ledgerId = $("input#Fid").val();
    $.ajax({
        type: 'GET',
        url: "/Voucher1/AccountNumberSearch",
        data: {
            accountNumber: accountNumber,
            filterAccount: filterAccount,
            accountType: accountType,
            ledgerId:ledgerId
        },
        success: function (result) {
            $('#account-pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });
        },
        error: function () {

        }
    });
});
$(".account-aumber").on('change', function (e) {
    if ($(this).val() == "") {
        return;
    }
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber").attr("id");

    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $("button#btn-account-view-details-" + _globalObject)
    $.ajax({
        type: 'GET',
        url: "/Voucher1/GetAccountNumber",
        data: {
            accountNumber: accountNumber,

        },
        success: function (result) {
            var count = result.length;
            if (count == 1) {

                $(".account-aumber#" + _globalObject).val(result[0].Accno);
                $(".account-id#" + _globalObject).val(result[0].IAccno);
                if (data != "NoDisplay") {
                    $('#account-details-show-div').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                    $(viewDetailsBtn).attr('Action', 'Show')

                    $(viewDetailsBtn).removeClass('hidden');
                }
            } else {
                $.ajax({
                    type: 'GET',
                    url: "/Voucher1/AccountNumberSearch",
                    data: {
                        accountNumber: accountNumber,
                        filterAccount: filterAccount,
                        accountType: accountType
                    },
                    success: function (result) {
                        $('#account-pop-up-div').html(result).modal({
                            'show': true,
                            'backdrop': 'static'
                        });
                    },
                    error: function () {

                    }
                });
            }
        },
        error: function () {
        }
    });
});
$(".account-aumber").on('keyup', function (e) {

    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber").attr("id");
    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $("button#btn-account-view-details-" + _globalObject)
    if (e.keyCode == 8) {
        $(".account-aumber#" + _globalObject).val("");
        $(".account-id#" + _globalObject).val("");
    }
    if (e.keyCode == 13) {
        $.ajax({
            type: 'GET',
            url: "/Voucher1/GetAccountNumber",
            data: { accountNumber: accountNumber },
            success: function (result) {
                var count = result.length;
                if (count == 1) {
                    $(".account-aumber#" + _globalObject).val(result[0].Accno);
                    $(".account-id#" + _globalObject).val(result[0].IAccno);
                    if (data != "NoDisplay") {
                        $('#account-details-show-div').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                        $(viewDetailsBtn).attr('Action', 'Show')

                        $(viewDetailsBtn).removeClass('hidden');
                    }
                } else {
                    $.ajax({
                        type: 'GET',
                        url: "/Voucher1/AccountNumberSearch",
                        data: {
                            accountNumber: accountNumber,
                            filterAccount: filterAccount,
                            accountType: accountType
                        },
                        success: function (result) {
                            $('#account-pop-up-div').html(result).modal({
                                'show': true,
                                'backdrop': 'static'
                            });
                        },
                        error: function () {

                        }
                    });
                }


            },
            error: function () {

            }
        });
    }
});
$(document).on('click', '.table-click-account-number-search table tr', function (e) {

    debugger;
    e.stopImmediatePropagation();
    debugger;
    var subsiId = $(this).attr('iaccno');
    var subsiText = $(this).attr('accountname');

    var doesSameIdExists = $(subsiEvent).closest('.add-more-subsi-container').find('tr');
    var count = 0;
    $(doesSameIdExists).each(function (index, item) {

        var hassameElement = $(item).find('input.internal-value').val();
        if (subsiId == hassameElement) {
            count++;
        }
    });
    if (count > 0) {
        ErrorAlert("Subsi Already Selected", 5000);

    }
    else {
        $(subsiEvent).closest('.account-number-div').find('input.account-aumber').val(subsiText);
        $(subsiEvent).closest('.account-number-div').find('input.account-id').val(subsiId)
        
        $('div#account-pop-up-div').modal('hide');
    }
});

$('.btn-account-view-details-button').on('click', function () {

    var attrShowHide = $(this).attr('Action');
    if (attrShowHide == 'Show') {
        $('.fa-toggle-off').removeClass('hidden');
        $('.fa-toggle-on').addClass('hidden');
        $(this).attr('Action', 'Hide')
    } else {
        $('.fa-toggle-off').addClass('hidden');
        $('.fa-toggle-on').removeClass('hidden');
        $(this).attr('Action', 'Show')
        $('#account-details-show-div').addClass('hidden');
        return;
    }


    var idAttribute = _globalObject;
    var showType = $(".account-number-div").find(".account-aumber#" + idAttribute).attr('showwith');
    var AccountId = $(".account-number-div").find(".account-id#" + idAttribute).val();
    if (AccountId == 0 || AccountId == "") {
        return;
    }
    $.ajax({
        type: 'GET',
        url: '/Voucher1/_AcountDetailsViewShow',
        data: {
            accountId: AccountId,
            showType: showType
        },
        success: function (accountDetailsShow) {
            $('#account-details-show-div').removeClass('hidden');
            $('#account-details-show-div').html(accountDetailsShow);
        }
    })
});

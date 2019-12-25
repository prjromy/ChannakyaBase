$("#btn-refresh").on("click", function () {

    var productCode = $(this).parents().find(".appount-search-product-code").find("#ProductId").val();
    var branchCode = $(this).parents().find(".appount-search-branch").find("#BranchId option:selected").val();
    var currencyCode = $(this).parents().find(".appount-search-currency").find("#CurrencyId option:selected").val();
    var filterAccount = $(this).parents().find(".appount-search-branch").find("#FilterAction").val();
    var accountType = $(this).parents().find(".appount-search-branch").find("#AccountType").val();
    $.ajax({
        type: 'GET',
        url: "/Voucher1/_AccountNumberSearch",
        data: { branchCode: branchCode, productCode: productCode, currencyCode: currencyCode, filterAccount: filterAccount, accountType: accountType },
        success: function (result) {
            $("#account-number-search").html(result);
        },
        error: function () {

        }
    });

});
$("#btn-account-holder-search").on("click", function () {
    debugger
    var productCode = $(this).parents().find(".appount-search-product-code").find("#ProductId").val();
    var branchCode = $(this).parents().find(".appount-search-branch").find("#BranchId option:selected").val();
    var currencyCode = $(this).parents().find(".appount-search-currency").find("#CurrencyId option:selected").val();
    var filterAccount = $(this).parents().find(".appount-search-branch").find("#FilterAction").val();
    var accountType = $(this).parents().find(".appount-search-branch").find("#AccountType").val();
    var customerName = $("#AccountHolder").val();
    $.ajax({
        type: 'GET',
        url: "/Voucher1/_AccountNumberSearch",
        data: { branchCode: branchCode, productCode: productCode, currencyCode: currencyCode, customerName: customerName, filterAccount: filterAccount, accountType: accountType },
        success: function (result) {
            $("#account-number-search").html(result);
        },
        error: function () {

        }
    });

});
$("#ProductCode").on("keyup", function (e) {
    e.stopImmediatePropagation();
    var productCode = $(this).val();
    if (e.keyCode == 13) {
        GetProductSearch(productCode);
    }
    if (e.keyCode == 8 || $("#ProductCode").val() == "") {
        $("#ProductCode").val("");
        $("#ProductId").val("");
    }
})
$("#ProductCode").on('change', function () {
    var productCode = $(this).val();
    if (productCode != "") {
        GetProductSearch(productCode);
    }

})
$("#btn-product-search").on("click", function (e) {

    e.stopImmediatePropagation();
    var productCode = $("#ProductCode").val();
    $.ajax({
        type: 'GET',
        url: "/Teller/ProductList",
        data: { productCode: productCode },
        success: function (result) {
            $('#common-pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });

        },
        error: function () {

        }
    });

})

function GetProductSearch(productCode) {
    $.ajax({
        url: '/Teller/GetProductByCode',
        data: { productCode: productCode },
        dataType: "json",
        success: function (result) {
            debugger;
            var count = result.length;
            if (count == 0) {
                $.MessageBox({
                    buttonDone: "OK",
                    message: "product not found!!"
                }).done(function () {

                    return false;
                }).fail(function () {
                    //return false;
                })
                return;
            }
            if (count == 1) {
                var currentText = result[0].ProductCode;
                $("#ProductCode").val(currentText);
                $("#ProductId").val(result[0].ProductId)
            }
            else {
                $.ajax({
                    type: 'GET',
                    url: "/Teller/ProductList",
                    data: { productCode: productCode },
                    success: function (result) {
                        $('#common-pop-up-div').html(result).modal({
                            'show': true,
                            'backdrop': 'static'
                        });

                    },
                    error: function () {

                    }
                });
            }

        }
    });
}
$(document).on('click', '.table-click-product table tr', function (e) {
   
    e.stopImmediatePropagation();
    var productId = $(this).closest('tr').attr('id');
    var parent = $(this).parents();

    $.ajax({
        type: 'GET',
        url: '/Teller/GetSelectProduct',
        data: {
            productId: productId
        },
        success: function (result) {

            $("#ProductCode").val(result.ProductCode);
            $("#ProductId").val(result.ProductId)
            var BranchList = '';
            $.each(result.BranchList, function (index, item) {
                BranchList = BranchList + "<option value='" + item.Value + "'>" + item.Text + "</option>"
            })
            $(parent).find(".appount-search-branch").find("#BranchId").html(BranchList);
            $("#common-pop-up-div").modal('hide');
        },
    });
});

$("#CurrencyCode").on("keypress", function (e) {

    e.stopImmediatePropagation();
    var CurrencyCode = $(this).val();
    if (e.keyCode == 13) {
        GetCurrencySearch(CurrencyCode);
    }
})
$("#CurrencyCode").on("change", function (e) {

    e.stopImmediatePropagation();
    var CurrencyCode = $(this).val();
    GetCurrencySearch(CurrencyCode);

})
$(document).on('click', '.table-click-currency table tr', function (e) {

    e.stopImmediatePropagation();
    var currencyId = $(this).closest('tr').attr('id');
    var parent = $(this).parents();

    $.ajax({
        type: 'GET',
        url: '/Teller/GetSelectCurrency',
        data: {
            currencyId: currencyId
        },
        success: function (result) {

            $("#CurrencyCode").val(result.CurrencyCode);
            $("#common-pop-up-div").modal('hide');
        },
    });

});
function GetCurrencySearch(currencyCode) {
    $.ajax({
        url: '/Teller/GetCurrencyByCode',
        data: { currencyCode: currencyCode },
        dataType: "json",
        success: function (result) {

            var count = result.length;
            if (count == 0) {
                $.MessageBox({
                    buttonDone: "OK",
                    message: "Branch not found!!"
                }).done(function () {

                    return false;
                }).fail(function () {
                    //return false;
                })
                return;
            }
            if (count == 1) {
                var currentText = result[0].CurrencyCode;
                $("#CurrencyCode").val(currentText);
            }
            else {
                $.ajax({
                    type: 'GET',
                    url: "/Teller/CurrencyList",
                    data: { currencyCode: currencyCode },
                    success: function (result) {
                        $('#common-pop-up-div').html(result).modal({
                            'show': true,
                            'backdrop': 'static'
                        });

                    },
                    error: function () {

                    }
                });
            }

        }
    });
}
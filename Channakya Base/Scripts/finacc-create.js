

$.GetTypeFinAcc = function (F2Id,Pid) {
debugger;

    $.ajax({
        url: '/FinAcc/GetFinSysType/',
        data: { id: F2Id },
        success: function (finId) {
            var finsys1Id = finId;
            debugger;
            $.LoadLedgerCreate(finsys1Id, Pid)
        }

    });

}




$.LoadLedgerCreate = function (finsys1Id, Pid) {
    debugger;

    $.ajax({
        url: '/FinAcc/IsGroupFinSys1/',
        data: { finsys1Id: finsys1Id },
        dataType: "html",
        success: function (isGroup) {
            debugger;
            //if (isGroup == "false" && finsys1Id != 15 && finsys1Id != 126 && finsys1Id != 2)
            //{
            //    LoadDimension();
            //}
       
        }
    });


    if (finsys1Id == 15) {
        debugger;
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Deposit Product" + "</b>")
        var container = $('.FinAcc-create').find('.deposit-product-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddDepositProduct/',
            data: { schemeId:Pid},
            dataType: "html",
            success: function(data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
            }
        });

    }
    else {
        $('.deposit-product-container').empty();
    }

    if (finsys1Id == 14) {
        debugger;
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Deposit Scheme" + "</b>")
        var container = $('.FinAcc-create').find('.deposit-scheme-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddDepositScheme/',
            dataType: "html",
            success: function(data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
            }
        });

    }
    else {
        $('.deposit-scheme-container').empty();
    }
    if (finsys1Id == 125) {
        debugger;
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Deposit Scheme" + "</b>")
        var container = $('.FinAcc-create').find('.deposit-scheme-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddLoanScheme/',
            dataType: "html",
            success: function(data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
            }
        });

    }
    else {
        $('.deposit-scheme-container').empty();
    }
    if (finsys1Id == 126) {
        debugger;
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Loan Account" + "</b>")
        var container = $('.FinAcc-create').find('.loan-account-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddDepositProduct/',
            data: { schemeId: Pid },
            dataType: "html",
            success: function (data) {
                $(container).empty();
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
            }
        });
    }
    else {
        $('.loan-account-container').empty();
    }


    if (finsys1Id== 5) {
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Bank Name" + "</b>");

        var container = $('.FinAcc-create').find('.bank-info-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddBankInfoDetails/',
            dataType: "html",
            success: function (data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
                //LoadDimension();
            }
        });

    }
    else {
        $(".bank-info-container").empty();

    }
    if (finsys1Id == 7) {
        $('#labelclass').empty();
        $('#labelclass').append("<b>" + " Subsi Name" + "</b>")
        var container = $('.FinAcc-create').find('.subsititle-details-container')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddSubsiDetails/',
            dataType: "html",
            success: function (data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
                //LoadDimension();
            }
        });

    }
    else {
        $('.subsititle-details-container').empty();
    }

    function LoadDimension()
    {
        
        var container = $('.FinAcc-create').find('.dimension-group')
        var form = $(container).closest("form");
        $.ajax({
            url: '/FinAcc/AddDimension/',
            dataType: "html",
            success: function (data) {
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(form));
            }
        });
    }



}
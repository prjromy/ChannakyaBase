function taskVerifierList(eventid, ismultiVerify) {
    debugger;
    $.ajax({
        type: 'GET',
        url: "/TaskVerification/VerifierList",
        data: { eventid: eventid, ismultiVerify: ismultiVerify },
        success: function (result) {
            $('.verfier-list').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });
        },
        error: function () {

        }
    });
}
function IsUserCashLimitExceed(eventid, ismultiVerify) {
    debugger;
    $.ajax({
        type: 'GET',
        url: "/TaskVerification/UserCashLimtVerification",
        data: { eventid: eventid, ismultiVerify: ismultiVerify },
        success: function (result) {
            debugger;
            $('.verfier-list').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });
        },
        error: function () {

        }
    });
}
$(document).on('click', '.table-click-verifier table tr', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var closestTr = $(this).closest('tr');
    var objCheck = $(closestTr).find('.IsSelected');
    var isChecked = $(closestTr).find('.IsSelected').prop("checked");
    var IsMultiVerifier = $(this).closest('.table-click-verifier').find("#IsMultiVerifier").val();
    var me = $(this);
    var count = 0;
    if (isChecked === true) {
        objCheck.prop('checked', false);
        $(closestTr).removeAttr('style');
    } else {


        if (IsMultiVerifier == false) {


            $(".table-click-verifier-list > tbody > tr").each(function (trCounttotal, obj) {

                if ($(obj).find('#IsSelected').prop("checked") == true) {
                    count = count + 1;
                }

            });


            if (count > 0) {
                ErrorAlert("Cannot be selected", 2000);
                document.getElementById('alert-error').scrollIntoView();
            }
            else {
                objCheck.prop('checked', true);
                $(closestTr).css('background-color', '#a2e5b5');
            }


        }

        else if (isChecked == false) {
            objCheck.prop('checked', true);
            $(closestTr).css('background-color', '#a2e5b5');
        }
    }
});
$(document).on('click', '.btn-account-verifier-save', function (e) {
    e.stopImmediatePropagation();
    debugger;

    var count = 0;
    $(".table-click-verifier-list > tbody > tr").each(function (trCounttotal, obj) {

        if ($(obj).find('.IsSelected').prop("checked") == true) {
            count = count + 1;
        }
    });
    if (count == 0) {
        $.MessageBox({
            buttonDone: "OK",
            message: "Please Select The Verifier!!"
        }).done(function () {
            return false;
        }).fail(function () {
            //return false;
        })
        return;

    }
    var eventId = $(this).attr('eventid');

    if (eventId == 1) {
        CreateNewAccount();
    } else if (eventId == 2) {
        depositTransaction();
    }
    else if (eventId == 3 || eventId == 11) {
        WithdrawTransaction();
    }
    else if (eventId == 9) {
        chequeRegistration();
    }
    else if (eventId == 8) {

        ManualChargeCreate();
    } else if (eventId == 13) {
        chequeGoodForPayment();
    } else if (eventId == 14) {
        internalChequeDeposit()
    } else if (eventId == 15) {
        remittanceDepositSave()
    }
    else if (eventId == 16) {
        remittancePaymentSave()
    }
    else if (eventId == 17) {
        LoanRegistrationSave();

    }
    else if (eventId == 18) {

        LoanVerifySave();

    } else if (eventId == 19) {
        LoanAccountOpen();

    } else if (eventId == 22) {

        LoanPaymentTransaction();
    }
    else if (eventId == 21) {
        DisburseLoan()
    }
    else if (eventId == 24) {

        createNewShareAccount()
    }
    else if (eventId == 25) {

        purchaseNewShare();
    }
    else if (eventId == 26) {

        ShareReturn();
    }
    else if (eventId == 27) {
        CreateChechClearence();
    }
    else if (eventId == 29) {
        EditTransaction();
    }
    else if(eventId == 32){
        LoanRebateTransaction();
}



    $(this).parents().find(".verfier-list").modal('toggle');
});

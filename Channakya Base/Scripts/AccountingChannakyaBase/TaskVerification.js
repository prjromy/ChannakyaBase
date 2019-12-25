function taskVerifierList(eventid, ismultiVerify,V1TId,narration) {
    $.ajax({
        type: 'GET',
        url: "/TaskVerificationAcc/VerifierList",
        data: { eventid: eventid, ismultiVerify: ismultiVerify,V1TId:V1TId , narration:narration },
        success: function (result) {
            debugger;
            $('#getVerifiedLists').html(result);
            $('#getVerifiedLists').modal('show');
        },
        error: function () {

        }
    });
}
$(document).on('click', '.table-click-verifier table tr', function (e) {
    e.stopImmediatePropagation();
    var closestTr = $(this).closest('tr');
    var objCheck = $(closestTr).find('.IsSelected');
    var isChecked = $(closestTr).find('.IsSelected').prop("checked");
    var IsMultiVerifier = $(this).closest('.table-click-verifier').find("#IsMultiVerifier").val();
    var me = $(this);
    var count = 0;
    if (isChecked == true) {
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
                $(closestTr).css('background-color', '#c1e9ea');
            }


        }

        else if (isChecked == false) {
            objCheck.prop('checked', true);
            $(closestTr).css('background-color', '#c1e9ea');
        }
    }
})
$(document).on('click', '.btn-account-verifier-save', function (e) {
    e.stopImmediatePropagation();
    var eventId = $(this).attr('eventid');
   
    if (eventId == 1) {
        CreateNewAccount();
    } else if (eventId == 2) {
        depositTransaction();
    }
    else if (eventId == 3) {
        WithdrawTransaction();
    }
    else if (eventId == 9) {
        chequeRegistration();
    }
    else if (eventId == 8) {
        debugger;
        ManualChargeCreate();
    }


    $(this).parents().find(".verfier-list").modal('toggle');
})

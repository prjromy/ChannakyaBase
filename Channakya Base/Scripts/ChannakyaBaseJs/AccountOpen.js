
$('#btn-Nominee').on('click', function () {

    debugger;
    var AccountNominee_NomNam = $("#AccountNominee_NomNam");
    var AccountNominee_NomRel = $("#AccountNominee_NomRel");
    var AccountNominee_ContactNo = $("#AccountNominee_ContactNo");
    var AccountNominee_ContactAddress = $("#AccountNominee_ContactAddress");
    var AccountNominee_CCertID = $("#AccountNominee_CCertID option:selected");
    var AccountNominee_CCertno = $("#AccountNominee_CCertno");
    var AccountNominee_Share = $("#AccountNominee_Share");
    var NomNam = $(AccountNominee_NomNam).val();
    var NomRel = $(AccountNominee_NomRel).val();
    var ContactNo = $(AccountNominee_ContactNo).val();
    var ContactAddress = $(AccountNominee_ContactAddress).val();
    var CCertID = $(AccountNominee_CCertID).val();
    var CCertno = $(AccountNominee_CCertno).val();
    var Share = $(AccountNominee_Share).val();

    if (NomNam == "") {
        AccountNominee_NomNam.focus();
        return;
    } else if (NomRel == "") {
        AccountNominee_NomRel.focus();
        return
    } else if (ContactNo == "") {
        AccountNominee_ContactNo.focus();
        return;
    } else if (CCertID == "0" || CCertID == "") {
        AccountNominee_CCertID.focus();
        return;
    } else if (Share == "" || Share == "0") {
        AccountNominee_Share.focus();
        return;
    }
    var tr = $('table#Nominee-table-div').find("tr").find(".share");
    var total = 0;
    $.each(tr, function (index, item) {
        total += parseFloat($(item).val())
    })
    total = total + parseFloat($("#AccountNominee_Share").val())
    if (total > 100) {
        total = total - parseFloat($("#AccountNominee_Share").val())
        $.MessageBox({
            buttonDone: "OK",
            message: "share not more than 100%!!"
        }).done(function () {
            $("#AccountNominee_Share").val(0);
            $("#AccountNominee_Share").focus();
            return false;
        }).fail(function () {
            //return false;
        });
        return;
    }
    $("#calShare").val(total);
    var Nominee = {
        CCertID: CCertID,
        CCertno: CCertno,
        ContactAddress: ContactAddress,
        ContactNo: ContactNo,
        NomNam: NomNam,
        NomRel: NomRel,
        Share: Share
    };
    $.ajax({
        url: "/Teller/AddNominee",
        data: JSON.stringify({ aNominee: Nominee }),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            if (total == 100) {
                $(".Nominee-table-div-Select").hide();

            }

            $('#Nominee-table-div >tbody').append(result);
            $(AccountNominee_NomNam).val("");
            $(AccountNominee_NomRel).val("");
            $(AccountNominee_ContactNo).val("");
            $(AccountNominee_ContactAddress).val("");
            $("#AccountNominee_CCertID").val("");
            $(AccountNominee_CCertno).val("");
            $(AccountNominee_Share).val("");
        },
        error: function () {
            alert('error in calling ajax !!')
        }
    });

});

function DeleteRow(e) {
    debugger;
    $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        message: "Are you sure?"
    }).done(function () {

        $(e).closest('tr').remove();
        var total = 0;
        var tr = $('table#Nominee-table-div').find("tr").find(".share");
        $.each(tr, function (index, item) {
            total += parseFloat($(item).val())
        })
        $("#calShare").val(total);

        $(".Nominee-table-div-Select").show();
    }).fail(function () {
    })

}
$('#SchemeId').on('change', function () {
    debugger;
    var schemId = $(this).val();


    $('#PID').empty();
    $('#Duration').empty();
    $('#InterestCalRule').empty();
    $('#ContributionAmount').empty();
    $('#Basic').empty();
    $('#InterestCapitalize').empty();

    $('#InterestRate').val("");
    $('#MinimumBalance').val("");
    $('#MovementAcc').empty();

    if (schemId === "") {
        $.ajax({
            type: "GET",
            url: "/Teller/GetTransactionDate",
            //datatype: "Json",
            success: function (result) {
                debugger;

                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").removeAttr('disabled');
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").removeAttr('disabled');
                $("#MaturationDate").val(result.duration.Date);

                $('#DateType').prop('readonly', true);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").val(result.duration.EnglishDate);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").val(result.duration.NepaliDate);


            }
        })

    }
    else {
        $.ajax({
            type: "GET",
            url: "/Teller/GetTransactionDate",
            //datatype: "Json",
            success: function (result) {
                debugger;

                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").removeAttr('disabled');
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").removeAttr('disabled');
                $("#MaturationDate").val(result.duration.Date);

                $('#DateType').prop('readonly', true);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").val(result.duration.EnglishDate);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").val(result.duration.NepaliDate);


            }
        });

        $.ajax({
            
            type: "GET",
            url: "/Teller/GetProductBySchemeId",
            datatype: "Json",
            data: { schemeId: schemId },
            success: function (result) {
                debugger;
                var optionList = '';
                $.each(result.Product, function (index, item) {
                    optionList = optionList + "<option value='" + item.ProductId + "'>" + item.ProductName + "</option>"
                })
                $('#PID').html(optionList);

                $('#PID').trigger('change');

            }
        });

    }
});


$('#AgreementAmount').on('keyup', function () {
    debugger;
    var agreeAmount = $('#AgreementAmount').val();
    var PID = $('#PID').val();
    $.ajax({

        type: "GET",
        url: "/Teller/CheckAgreementAmountWithLAmountProduct",
        datatype: "Json",
        data: { agreeAmount: agreeAmount, PID: PID },
        success: function (result) {
            if (result == true) {
                $('#AgreementAmount').focus();
            }
            else {
                ErrorAlert("Agreement Amount must be greater than Limit amount of product",3000);
            }
        }
    })
  


})
$('#PID').on('change', function () {
    debugger;
    var productId = $("#PID option:selected").val();
    $.ajax({
        type: "GET",
        url: "/Teller/GetAllProductDetailsByProductId",
        datatype: "Json",
        data: { productId: productId },
        success: function (result) {
            debugger;
            //var getDate = $('#MaturationDate').val();
            if (result.ChkProductType.IsDuration) {

                //if (result.ChkProductType.IsFixed) {

                //}
                if (result.ChkProductType.IsRecurring || result.ChkProductType.IsOtherTypeRecurring) {
                    $(".ContributionAmountForRecurring").removeClass('hidden');
                    $(".AggrementForFixed").addClass('hidden');
                    $("#Basic").removeAttr('disabled');
                    $(".Basic").removeClass('hidden');
                    $(".MaturationDate").show();
                    //$(".AggrementForFixed").removeClass('hidden');
                    $(".Duration").removeClass('hidden');
                    $(".MinimumBalance").removeClass('hidden');
                    $(".AggrementForFixed").find('#IsFiexedAggrAmt').val(false);
                } else {
                    $(".ContributionAmountForRecurring").addClass('hidden');
                    $(".AggrementForFixed").removeClass('hidden');
                    $(".Duration").removeClass('hidden');
                    $(".Basic").addClass('hidden');
                    $(".MinimumBalance").addClass('hidden');
                    $(".MaturationDate").show();

                    //$("#Basic").attr('disabled', 'disabled');
                    $(".AggrementForFixed").find('#IsFiexedAggrAmt').val(true);

                }

                $("#Duration").removeAttr('disabled');
                $("#AgreementAmount").removeAttr('disabled');
                $("#ContributionAmount").removeAttr('disabled');

                var durationList = "";

                //durationList = $('#Duration').prepend(newOption);
                //durationList = $('#Duration').append("<option value> Please Select</option>");
                //durationList = $('#Duration').append('select option:contains("-Select-Duration-")').prop('selected', true);


                debugger;
                //var newOption = $("#Duration").append('<option value = "0" selected ="selected">select</option>');
                //result.productDurationList.unshift("-Select-Duration");
                $.each(result.productDurationList, function (index, item) {


                    durationList = durationList + "<option value='" + item.Id + "'>" + item.Duration + "</option>";


                })
                $('#Duration').html(durationList).prepend('<option value = "0" selected ="selected">-Select-Duration-</option>');
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").removeAttr('disabled');;
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").removeAttr('disabled');;
                $("#InterestRate").val("");
                //$("#Duration").trigger("change")
            } else {
                $(".AggrementForFixed").find('#IsFiexedAggrAmt').val(false);
                //$('#Duration').html("");
                //$("#Basic").attr('disabled', 'disabled');
                $(".Basic").addClass('hidden');
                $(".MaturationDate").hide();
                $(".AggrementForFixed").addClass('hidden');
                $(".Duration").addClass('hidden');
                //$("#Duration").attr('disabled', 'disabled');

                $("#InterestRate").val("");
                $("#AgreementAmount").attr('disabled', 'disabled');
                $("#ContributionAmount").attr('disabled', 'disabled');
                var InterestCapitalizeList = '';

                $.each(result.InterestCapital, function (index, item) {
                    InterestCapitalizeList = InterestCapitalizeList + "<option value='" + item.InterestCapitalizeId + "'>" + item.InterestCapitalizeName + "</option>"
                })
                $('#InterestCapitalize').html(InterestCapitalizeList);
                $("#InterestCapitalize").trigger("change");

            }
            $('#DateType').prop('readonly', true);

            var InterestCalList = '';

            $.each(result.Policy, function (index, item) {

                InterestCalList = InterestCalList + "<option value='" + item.PloicyIntCalId + "'>" + item.PolicyIntCalName + "</option>"
            })
            $('#InterestCalRule').html(InterestCalList);

            if (result.ChkProductType.IsMovement) {
                $("#MovementAcc").removeAttr('disabled');
                $(".btn-account-open-search").removeAttr('disabled');
            } else {
                $("#MovementAcc").val("");
                $("#MovementAcc.account-id").val("");
                $("#MovementAcc").attr('disabled', 'disabled');
                $(".btn-account-open-search").attr('disabled', 'disabled');
            }
            var CurrList = '';
            $.each(result.Currency, function (index, item) {
                CurrList = CurrList + "<option value='" + item.CTId + "'>" + item.CurrencyName + "</option>"
            })
            $('#CurrID').html(CurrList);

            //var FloatingInterestList = '';
            //$.each(result.FloatingInterest, function (index, item) {
            //    FloatingInterestList = FloatingInterestList + "<option value='" + item.FloatingId + "'>" + item.FloatingName + "</option>"
            //})
            //$('#FloatingInterest').html(FloatingInterestList);
            if (result.ChkProductType.IsIndiviualInterestRate) {
                $("#InterestRate").removeAttr('readonly');
            } else {
                $("#InterestRate").attr('readonly', 'readonly');
            }
            $("#MinimumBalance").val(result.ProductDetails.MinimumAmount);
            if (result.ChkProductType.IsIndividualLimit) {
                $("#MinimumBalance").removeAttr('readonly');
            } else {
                $("#MinimumBalance").attr('readonly', 'readonly');
            }
            debugger;
            if (result.IsChargeAvailable) {
                $.ajax({
                    type: "GET",
                    url: "/FinanceParameter/_ChargeAutoTriggeredForAccountOpen",
                    dataType: "html",
                    data: {
                        productId: productId, modevent: 4,
                    },
                    success: function (result) {
                        debugger;
                        $("#AccountOpen-form").find(".chargeDetails").html("");
                        $("#AccountOpen-form").find(".chargeDetails").html(result);

                    }


                });
            }
            else
            {
                $("#AccountOpen-form").find(".chargeDetails").html("");
            }
            
        }
    });
});

$("#ContributionAmount").on("change", function () {
    debugger;
    var durationId = $("#Duration option:selected").val();
    var productId = $("#PID option:selected").val();
    var contrubAmount = $("#ContributionAmount").val();
    $.ajax({
        type: "GET",
        url: "/Teller/GetRecurringBasic",
        datatype: "Json",
        data: {
            contrubAmount: contrubAmount,
            productId: productId,
            durationId: durationId,

        },
        success: function (result) {
            debugger;
            var durationBasic = "";

            $.each(result, function (index, item) {
                durationBasic = durationBasic + "<option value='" + item.Id + "'>" + item.Duration + "</option>"
            });
            //$('#Basic').html(durationBasic);
            $('#Basic').html(durationBasic).prepend('<option value = "0" selected ="selected">-Select-Basic-</option>');
            $('#InterestCapitalize').val("");
            //$("#Basic").trigger('change')
        }
    });
});
$("#Basic").on("change", function () {
    debugger;
    var durationId = $("#Duration option:selected").val();
    var productId = $("#PID option:selected").val();
    var contrubAmount = $("#ContributionAmount option:selected").val();
    var basicId = $("#Basic option:selected").val();
    $.ajax({
        type: "GET",
        url: "/Teller/GetInterestCapitalizeForRecurringProduct",
        datatype: "Json",
        data: {
            durationId: durationId,
            productId: productId,
            basicId: basicId,
            value: contrubAmount,
        },
        success: function (result) {
            var InterestCapitalizeList = "";
            $.each(result, function (index, item) {
                InterestCapitalizeList = InterestCapitalizeList + "<option value='" + item.InterestCapitalizeId + "'>" + item.InterestCapitalizeName + "</option>"
            });
            $('#InterestCapitalize').html(InterestCapitalizeList);
            $("#InterestCapitalize").trigger("change");
        }
    });
});
$("#Duration").on("change", function () {
    debugger;
    var durationId = $("#Duration option:selected").val();
    var regDate = formatDate($("#RDate").val());
    var productId = $("#PID option:selected").val();
    var dateAd = $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD");
    var dateBs = $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS");
    var datetype = $('#DateType').is(":checked")

    if (durationId == "0") {

        $('#ContributionAmount').empty();
        $('#Basic').empty();
        $('#InterestCapitalize').empty();

        $('#InterestRate').val("");
        //$('#MinimumBalance').val("");
        $.ajax({
            type: "GET",
            url: "/Teller/GetTransactionDate",
            //datatype: "Json",
            success: function (result) {
                debugger;

                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").removeAttr('disabled');
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").removeAttr('disabled');
                $("#MaturationDate").val(result.duration.Date);

                $('#DateType').prop('readonly', true);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateAD").val(result.duration.EnglishDate);
                $("#MaturationDate").closest(".chdPickerMain").find(".txtDateBS").val(result.duration.NepaliDate);


            }
        })
    }
    else {


        $.ajax({
            type: "GET",
            url: "/Teller/GetCaptRuleByProductAndMatDuration",
            datatype: "Json",
            data: {
                registerDate: regDate,
                productId: productId,
                durationId: durationId,
                datetype: datetype
            },
            success: function (result) {
                debugger;
                $("#MaturationDate").val(result.duration.Date);
                $('#DateType').prop('readonly', true);

                dateAd.val(result.duration.EnglishDate);
                dateBs.val(result.duration.NepaliDate)
                if (result.ChkProductType.IsFixed) {
                    debugger
                    var InterestCapital = "";

                    $.each(result.InterestCapital, function (index, item) {

                        InterestCapital = InterestCapital + "<option value='" + item.InterestCapitalizeId + "'>" + item.InterestCapitalizeName + "</option>"
                    })
                    $('#InterestCapitalize').html(InterestCapital);
                    $("#InterestCapitalize").trigger("change");

                } else if (result.ChkProductType.IsRecurring || result.ChkProductType.IsOtherTypeRecurring) {
                    var recValue = "";

                    $.each(result.InterestCapital, function (index, item) {

                        recValue = recValue + "<option value='" + item.contributionValue + "'>" + item.contributionValue + "</option>"
                    })
                    ////$('#ContributionAmount').html(recValue);
                    $('#ContributionAmount').html(recValue).prepend('<option value = "0" selected ="selected">-Select-ContributionAmount-</option>');
                    var durationBasic = "";
                    $('#Basic').html(durationBasic).prepend('<option value = "0" selected ="selected">-Select-Basic-</option>');
                    var InterestCapitalizeList = "";
                    $('#InterestCapitalize').html(InterestCapitalizeList);
                    $("#AccountOpen-form").find("#InterestRate").val("");


                    //$("#ContributionAmount").trigger('change')

                }

            }
        });
    }
});

$('input[type="radio"][name="DateType"]').on('change', function () {
    debugger;
    //var radiovalue = $('input[type="radio"][name="DateType"]:checked').val();
    var selected_value = $('input[type="radio"][name="DateType"]:checked').val();
    //var radiovalue1 = $('input[type="radio"][name="DateType"]:checked').find("#BS").val();
    //var bsvalue = $("#BS option:checked").val();
    //var advalue = $("#AD option:checked").val();
    //var isChecked = $(this).prop("checked");
    var parent = $(this).parents();
    var engdate = $(parent).find(".chdPickerMain").find(".chdPicker").find(".txtDateAD");
    var nepDate = $(parent).find(".chdPickerMain").find(".chdPicker").find(".txtDateBS");
    //var calDateType = $(parent).find(".chdPickerMain").find(".chdPicker").find(".calDateType");
    var calDateType = $(parent).find(".chdPickerMain").find(".chdPicker").find('.dpDFormat').find("#DateTypeADBS");

    if (selected_value == "1") {

        $(calDateType).val(1);

        $(engdate).show();
        $(nepDate).hide();
        $(engdate).focus();
    } else {
        $(nepDate).show();
        $(engdate).hide();
        $(nepDate).focus();
        $(calDateType).val(2);
    }
})
//$("#InterestCalRule").on("change", function () {

//    var durationId = $("#Duration option:selected").val();

//    if (durationId == 0) {
//        $.MessageBox({
//            buttonDone: "OK",
//            message: "Please select duration!!"
//        }).done(function () {

//            $("#Duration").focus();

//        }).fail(function () {
//        })
//        return;
//    }
//    $.ajax({
//        type: "GET",
//        url: "/Teller/GetCaptRuleByProductAndDurationId",
//        datatype: "Json",
//        data: {
//            productId: productId,
//            durationId: durationId,

//        },
//        success: function (result) {

//            var InterestCapital = "";
//            InterestCapital = "<option value=''>-Select Interest Capitalize Rule-</option>";
//            $.each(result.InterestCapital, function (index, item) {

//                InterestCapital = InterestCapital + "<option value='" + item.InterestCapitalizeId + "'>" + item.InterestCapitalizeName + "</option>"
//            })
//            $('#InterestCapitalize').html(InterestCapital);
//        }
//    });
//});
$("#InterestCapitalize").on("change", function () {
    debugger;
    var captId = $("#InterestCapitalize option:selected").val();
    var durationId = $("#Duration option:selected").val();
    var contrubAmount = $("#ContributionAmount option:selected").val();
    var basicId = $("#Basic option:selected").val();
    var productId = $("#PID option:selected").val();
    if (captId == 0) {
        return;
    }
    $.ajax({
        type: "GET",
        url: "/Teller/GetCaptRuleIntRateByProductAndDurationId",
        datatype: "Json",
        data: {
            captId: captId,
            productId: productId,
            contrubAmount: contrubAmount,
            durationId: durationId,
            basicId: basicId
        },
        success: function (result) {
            debugger;
            $("#AccountOpen-form").find("#InterestRate").val(result);
        }
    });
});
function getTotalShare() {
    debugger;
    var items = $('table#Nominee-table-div').find("tr").find(".share");
    var total = 0;
    total = total + parseFloat($("#AccountNominee_Share").val())
    $.map(items, function (value, index) {
        total += parseFloat($(value).val());
        if (total > 100) {
            total -= parseFloat($("#AccountNominee_Share").val());
            $.MessageBox({
                buttonDone: "OK",
                message: "share not more than 100%!!"
            }).done(function () {
                $("#AccountNominee_Share").val(0);
                $("#AccountNominee_Share").focus();
                return false;
            }).fail(function () {
            })
            return;
        }
    });
    if ($("#AccountNominee_Share").val() == "" || $("#AccountNominee_Share").val() > 100) {
        $("#calShare").val(0);
    } else {
        $("#calShare").val(total);
    }
}

$(function () {
    $.validator.unobtrusive.parse('#AccountOpen-form');
    $(".btn-account-open-save").on("click", function (e) {
        debugger;

        var strictVerifiable = $('.account-create').find('#strictlyverifiable').prop('checked');

        e.stopImmediatePropagation();
        eventid = $(this).data('eventid');

        ismultiVerify = $(this).attr('isMultiVerifier');

        $('#AccountOpen-form').removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($('#AccountOpen-form'));
        if ($("#AccountOpen-form").valid()) {
            var Referred = $('.Referred-By').find("div.CommonSearchDiv").find(".multiselectCustomer").val()
            var Brought = $('.Brought-By').find('.CommonSearchDiv').find('.employeeId').val()
            var customer = $('.account-open-customer').find("div.CommonSearchDiv").find(".multiselectCustomer").val()
            if (customer == null) {
                ErrorAlert("Customer  is required!!", 5000);
                document.getElementById('alert-error').scrollIntoView();
                return false
            }
            var isfixed = $(".AggrementForFixed").find('#IsFiexedAggrAmt').val();
            if (isfixed == "true") {
                var aggrementAmt = $('#AgreementAmount').val()

                if (aggrementAmt == 0 || aggrementAmt == '') {
                    ErrorAlert("Aggrement Amount is required!!", 5000);
                    document.getElementById('alert-error').scrollIntoView();
                    return false;
                }
            }

            if (Referred == null) {
                ErrorAlert("Referred By is required!!", 5000);
                document.getElementById('alert-error').scrollIntoView();
                return false
            }
            if (Brought == 0) {
                ErrorAlert("Collector is required!!", 5000);
                document.getElementById('alert-error').scrollIntoView();
                return false;
            }
            var tr = $('table#Nominee-table-div').find("tr").find(".share");
            var total = 0;
            $.each(tr, function (index, item) {
                total += parseFloat($(item).val())
            });

            if (total != 100) {
                $.MessageBox({
                    buttonDone: "OK",
                    message: "Please add 100% share in table!!"
                }).done(function () {

                    return false;
                }).fail(function () {
                })
                return false;
            }
            if (strictVerifiable == true) {
                taskVerifierList(eventid, ismultiVerify);
            }
            else {
                CreateNewAccount();
            }
        }
        else {

            return false;
        }



        //    $.MessageBox({
        //        buttonDone: "Yes",
        //        buttonFail: "No",
        //        message: "Are you sure?"
        //    }).done(function () {
        //        $('#AccountOpen-form').ajaxSubmit({
        //            beforeSubmit: function () {
        //            },
        //            success: function (result) {
        //                if (result.Success) {
        //                    $.ajax({
        //                        url: "/Teller/AccountOpenIndex",
        //                        type: 'GET',
        //                        async: false,
        //                        success: function (result) {
        //                            $('section.content').html(result);
        //                        }
        //                    })
        //                    SuccessAlert(result.Msg, 5000);
        //                    document.getElementById('alert-success').scrollIntoView();

        //                } else {

        //                    ErrorAlert(result.Msg, 5000);
        //                    document.getElementById('alert-error').scrollIntoView();
        //                }
        //            },
        //            error: function () {
        //                ErrorAlert(data.responseText, 15000)
        //            }
        //        });
        //    }).fail(function () {

        //    });
    })

});

function CreateNewAccount() {
    //estopImmediatePropagation();
    debugger;
    $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        message: "Are you sure?"
    }).done(function () {
        debugger;
        $('#AccountOpen-form').ajaxSubmit({
            beforeSubmit: function () {
            },
            success: function (result) {
                debugger;
                if (result.Success) {
                    $.ajax({
                        url: "/Teller/CreateEditAccount",
                        type: 'GET',
                        async: false,
                        success: function (result) {
                            $('section.content').html(result);
                        }
                    });
                    SuccessAlert(result.Msg, 15000);
                    //  $(".content-wrapper").find("#account-pop-up-div1").modal('toggle');
                    document.getElementById('alert-success').scrollIntoView();

                } else {

                    ErrorAlert(result.Msg, 15000);
                    document.getElementById('alert-error').scrollIntoView();
                }
            },
            error: function () {
                ErrorAlert(data.responseText, 15000)
            }
        });
    }).fail(function () {

    });
}
//$(".addandClose").on(click, function () {
//   
//    var id = 18;
//    $.ajax({
//        url: "/Customer/GetEventListByCustomerId",
//        type: 'GET',
//        data: { id: id },
//        success: function (result) {
//           
//            $(".event-message-pop-up").html(result);
//        }
//    })
//})
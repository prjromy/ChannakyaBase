var _globalObject;
var _globalAccountValue;
$("button.btn-account-open-search").on('click', function (e) {
    debugger;
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber")
    _globalAccountValue = $(this).closest("div.account-number-div");
    var filterAccount = $(_globalObject).attr("accountFilter")
    var accountType = $(_globalObject).attr("accountType")
    var accountNumber = $(_globalObject).val();
    $.ajax({
        type: 'GET',
        url: "/Teller/AccountNumberSearch",
        data: {
            accountNumber: accountNumber,
            filterAccount: filterAccount,
            accountType: accountType
        },
        success: function (result) {
            debugger;
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
    debugger;
    //if ($(this).val() == "") {
    //    debugger;
    //    $(_globalObject).val("");
    //    $(_globalAccountValue).find(".account-id").val("");
    //    $('.img-container').hide();
    //    $('.view-history-image').hide();
    //    $('.view-signature-div').hide();
    //    //$("#adjustment-type").prop("selectedIndex", 0);



    //    return;
    //}

    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber");
    _globalAccountValue = $(this).closest("div.account-number-div");
    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $(_globalAccountValue).find("button#btn-account-view-details-user-Account");
    $.ajax({
        type: 'GET',
        url: "/Teller/GetAccountNumber",
        data: {
            accountNumber: accountNumber,
            filterAccount: filterAccount,
            accountType: accountType
        },
        success: function (result) {
            debugger;

            var count = result.length;
            if (count == 0) {
                ErrorAlert("Account not found", 5000);
                document.getElementById('alert-error').scrollIntoView();
                $(_globalObject).val("");
                $(_globalAccountValue).find(".account-id").val("");
                if (data == "ChequeTransHistory") {
                    debugger;


                }
            }
            if (count == 1) {
                var eventid = $("#MessageEventID").val();
                if (eventid == undefined || eventid == "") {
                    eventid = 0;
                }
                $.ajax({
                    //reterive customer from accno and display
                    type: 'GET',
                    url: '/Customer/GetCustomerEventListByAccountId',
                    data: {
                        accountid: result[0].IAccno,
                        eventid: eventid,
                    },
                    success: function (CustomerInfo) {
                        debugger;
                        $(".customer-event-message").html(CustomerInfo);
                    }
                });
                $(_globalObject).val(result[0].Accno);
                $(_globalAccountValue).find(".account-id").val(result[0].IAccno);
                CheckAccountStatus(result[0].IAccno)

                if ((data != "NoDisplay") && (data != "CollateralDisplay") && (data != "Report") && (data != "IsFDLoan") && (data != "AccountClose")) {

                    $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                    $(viewDetailsBtn).attr('Action', 'Show')
                    $(viewDetailsBtn).removeClass('hidden');
                    $('#account-details-show-div').addClass('hidden');
                    $(viewDetailsBtn).removeClass('hidden');
                }
                if (data == "AccountClose") {
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/_AcountDetailsViewShow',
                        data: {
                            accountId: result.AccountId,
                            showType: "AccountClose"
                        },
                        success: function (accountDetailsShow) {
                            debugger;
                            $('#account-details-show-div').removeClass('hidden');
                            $('#account-details-show-div').html(accountDetailsShow);
                        }
                    })
                }
                if (data == "ChargableAccounts") {
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/GetFixedAccountsByAccountId',
                        data: {
                            Iaccno: result[0].IAccno,
                        },
                        success: function (Isfixed) {
                            debugger;
                            if (Isfixed) {
                                $.MessageBox({
                                    buttonDone: "OK",
                                    message: "Fixed Account Cannot be Chargeable"
                                }).done(function () {

                                }).fail(function () {
                                    //return false;
                                })
                            }

                        },
                    });

                }
                //if (data == "ChequeTransHistory") {

                //    $.ajax({
                //        type: 'GET',
                //        url: '/Information/StatusChequeList',
                //        data: {
                //            iAccNo: result[0].IAccno,
                //        },
                //        success: function (chequeTrans) {
                //            $("#cheque-transaction-status-change").html(chequeTrans);

                //        },
                //    });
                //}

                if (data == "ChequeTransHistory") {
                    debugger;
                    $.ajax({
                        type: 'GET',
                        url: '/Information/StatusChequeList',
                        data: {
                            iAccNo: result.AccountId,
                        },
                        success: function (chequeTrans) {
                            $("#cheque-transaction-status-change").html(chequeTrans);
                            //$("#cheque-transaction-status-change").show();

                        },
                    });

                }
                if (data == "WithdrawWithIntPay") {
                    CheckAccountStatus(result[0].IAccno)

                    $('.view-signature-div').show();
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/InterestPayable',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (intPayable) {
                            if (intPayable.IsNomee == true) {
                                if (intPayable.Amount != 0) {

                                    $('a.intPayable').removeClass('hidden')
                                    $.MessageBox({
                                        buttonDone: "OK",
                                        message: "Available Interest Payable Amount=NRS." + intPayable.Amount
                                    }).done(function () {
                                        
                                    }).fail(function () {
                                        //return false;
                                    })

                                    $('.payableamount').html("NRS. " + intPayable.Amount); 

                                }
                            } else {
                                $('a.intPayable').addClass('hidden')
                                $('.btn-cheque-clearence-save').prop('disabled', false);
                            }
                        },
                    });
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/CheckWithdrawPopUpValidation',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (popValidation) {

                            if (!popValidation.Success) {

                                //$(idAttribute).val("");
                                //$(_globalAccountValue).find(".account-id").val("");
                                ErrorAlert(popValidation.Msg, 5000);
                                document.getElementById('alert-error').scrollIntoView();
                              
                            } else {
                                if (popValidation.ReturnId != 1) {
                                    $.MessageBox({
                                        buttonDone: "OK",
                                        message: popValidation.Msg
                                    }).done(function () {
                                      
                                    }).fail(function () {
                                        //return false;
                                    })
                                }
                               
                            }
                        },
                    });

                }
                if (data == "Deposit") {
                    CheckAccountStatus(result[0].IAccno)
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/CheckDepositPopUpSelectCondition',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (popValidation) {

                            if (!popValidation.Success) {

                                //$(idAttribute).val("");
                                //$(_globalAccountValue).find(".account-id").val("");
                                ErrorAlert(popValidation.Msg, 5000);
                                document.getElementById('alert-error').scrollIntoView();
                            }
                        },
                    });
                }
                if (data == "LoanDetails") {

                    var isOtherLoan = $(".disburse-loan-index").find(".isOtherLoan").val();
                    $.ajax({
                        type: 'GET',
                        url: '/Credit/CheckPreviousLoanDisburse',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (existingdetails) {
                            if (existingdetails.Success == false) {
                                $.ajax({
                                    type: 'GET',
                                    url: '/credit/LoanDisburseDetailsByIaccno',
                                    data: {
                                        accountId: result[0].IAccno, isOtherLoan: isOtherLoan
                                    },
                                    success: function (loanDisburseDetails) {
                                        debugger;
                                        $(".loan-disburse-details").html(loanDisburseDetails)
                                        $(".loan-disburse-details").show()
                                        $('.disbursement-schedule-final').show();
                                        $('.loan-disburse-details').show();
                                        $('.chargeDeductionType').show();
                                        var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                        var productId = $(".loan-disburse-details").find("#Product_ProductId").val();
                                        $('disbursement-schedule-final').hide();

                                        if (isOtherLoan == "False") {
                                            if (ischargeable == "True") {
                                                $.ajax({
                                                    type: "GET",
                                                    url: "/FinanceParameter/_ChargeAutoTriggered",
                                                    datatype: "html",
                                                    data: {
                                                        productId: productId, modevent: 20
                                                    },
                                                    success: function (result) {
                                                        $(".disburse-loan-index").find(".chargeDetails").html("");
                                                        $(".disburse-loan-index").find(".chargeDetails").html(result);

                                                    }
                                                })
                                            }
                                        }

                                    }
                                })
                            }
                            else {
                                ErrorAlert(existingdetails.Msg, 5000)
                       
                                $('.disbursement-schedule-final').hide();
                                $('.loan-disburse-details').hide();
                                $('.chargeDeductionType').hide();
                                return false;
                            }
                        }
                    })
                }
                if (data == "LoanPayment") {

                    $.ajax({
                        type: 'GET',
                        url: '/Credit/LoanAccountPayment',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (payment) {

                            $('.load-loan-account-payment-data').html(payment)
                        },
                    });
                    $.ajax({
                        type: 'GET',
                        url: '/Credit/_LoanAccountDetailsInformationShow',
                        data: {
                            accountId: result[0].IAccno,
                        },
                        success: function (paymentdetails) {

                            $('.load-loan-account-details-data').html(paymentdetails)
                        },
                    });
                    if (
                        $("#ManualDistribute").prop('checked')) {
                        $("#ManualDistribute").prop('checked', false)
                        $('.loan-total-amount-payment').prop("readonly", false)
                        $('.Rebate-loan-payment').prop("readonly", false)
                    }
                    $('.loan-total-amount-payment').val(0)
                    $('.Rebate-loan-payment').val(0)
                    $("#ManualDistribute").prop("disabled", false);

                }
                if (data != "Report") {
                    CheckAccountStatus(result[0].IAccno)
                }

                if (data == "CollateralDisplay") {
                    $.ajax({
                        type: 'GET',
                        url: '/Credit/CollateralInternalAccountNoDetails',
                        data: {
                            iAccNo: result[0].IAccno,
                        },
                        success: function (details) {

                            $("#Balance").val(details.Amount);
                            $('.internal-account-mat').find('.txtDateAD').val(details.endDate);
                            $('.internal-account-mat').find('.txtDateBS').val(details.nepDate);
                            $('.internal-account-mat').find('.txtDateValue').val(details.date);

                        },
                    });
                }
                if (data == "IsFDLoan") {
                    debugger;
                    var Pid = $('#PID').val();
                    $.ajax({
                        type: 'GET',
                        url: '/Credit/GetFDLoanDetails',
                        data: {
                            accountId: result.AccountId,
                            Pid: Pid,
                            
                                Modelfrom:"fromfd",
                        },
                        success: function (fdloan) {
                            debugger;
                            if (fdloan.IsGurantor == true && fdloan.hasBalance == true) {
                                debugger
                                $.MessageBox({
                                    buttonDone: "Yes",
                                    buttonFail: "No",
                                    message: "<span style='color:red;'><center>This FD Account is already a guarantor!!</center></span> <br/> <span style='padding:4px;'><center>Are you sure?</center></span>"
                                }).done(function () {

                                    debugger;
                                    $('#guarantor-Entry-table-div >tbody').empty();
                                    $('#link-account-table-div .MoveableRow').empty();
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                                    $(".guaranty-account").find('.account-id').val(result.AccountId)
                                    $(".guaranty-account").find(".account-aumber").val(result.AccountNumber);
                                    $(".link-accounts-search").find(".account-id").val(result.AccountId).prop("readonly", true);
                                    $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber).prop("readonly", true);
                                    $('#LAmt').val(fdloan.Balance);
                                    $('#InterestRate').val(fdloan.IRate);
                                    //var allObj = $('.loan-registration-details').find('.day-month');
                                    //$(allObj).each(function (index, item) {
                                    // if (index == 0) {
                                    //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                                    //          }
                                    //    else {
                                    //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                                    //        }
                                    //})

                                    //$(".day-month").text("Payment-Description[Day(s)]")
                                    $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                                    $("#GrantedDuration").val(fdloan.fixedDuration)
                                    $("#Payment_Mode").val(fdloan.PaymentMode)
                                    //$("#IFreq").val(fdloan.IFreq)
                                    $("#IFreq").val(1)
                                    $("#PFreq").val(fdloan.fixedDuration)
                                    $("#PAYSID").val(fdloan.ScheduleType)
                                    // $(".loan-registration-details").find("#Duration").prop('readonly', true);
                                });
                            }
                            else if (fdloan.IsGurantor == true || fdloan.hasBalance == false) {
                                ErrorAlert("This guarantor doenot have sufficient balance", 5000);
                                return false;
                            }
                            else {
                                debugger;
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                                $(".guaranty-account").find('.account-id').val(result.AccountId)
                                $(".guaranty-account").find(".account-aumber").val(result.AccountNumber);
                                $(".link-accounts-search").find(".account-id").val(result.AccountId);
                                $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber);
                                $('#LAmt').val(fdloan.Balance);
                                $('#InterestRate').val(fdloan.IRate);
                                //var allObj = $('.loan-registration-details').find('.day-month');
                                //$(allObj).each(function (index, item) {
                                // if (index == 0) {
                                //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                                //          }
                                //    else {
                                //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                                //        }
                                //})

                                //$(".day-month").text("Payment-Description[Day(s)]")
                                $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                                $("#GrantedDuration").val(fdloan.fixedDuration)
                                $("#Payment_Mode").val(fdloan.PaymentMode)
                                //$("#IFreq").val(fdloan.IFreq)
                                $("#IFreq").val(1)
                                $("#PFreq").val(fdloan.fixedDuration)
                                $("#PAYSID").val(fdloan.ScheduleType)
                            }
                         
                        },

                    });
                }
                if (data == "WithOutAccountSearch") {
                    debugger;
                    var accountNumber = $('#cheque-registration-list').find('#Iaccno').val();
                    $.ajax({
                        type: 'GET',
                        url: '/Information/_NewChequeIssueIndex',
                        data: {
                            accountNumber: accountNumber,

                        },
                        success: function (result) {
                            debugger;
                            $('.table-cheque-issue-registration').html(result);

                        },

                    });
                }
                var commonAccountName = $('.commonName').find('#AccountName');
                $(commonAccountName).val(result[0].AccountName)


            }
            else {
                $.ajax({
                    type: 'GET',
                    url: "/Teller/AccountNumberSearch",
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
    debugger;
    e.stopImmediatePropagation();

    _globalObject = $(this).closest("div.account-number-div").find(".account-aumber");
    _globalAccountValue = $(this).closest("div.account-number-div");
    var accountNumber = $(this).val();
    var data = $(this).attr('showwith');
    var filterAccount = $(this).attr('accountFilter');
    var accountType = $(this).attr('accounttype');
    var viewDetailsBtn = $(_globalAccountValue).find("button#btn-account-view-details-user-Account");
    if (e.keyCode == 8) {

        $(_globalObject).val("");
        $(_globalAccountValue).find(".account-id").val("");
        $('.img-container').hide();
        $('.view-history-image').hide();
        $('.view-signature-div').hide();
        //$('.table-cheque-block-unblock-deactive-status').hide();
        //$('#cheque-transaction-status-change').hide();

        $(viewDetailsBtn).addClass('hidden');
        $("#cheque-transaction-status-change").html('');
        $('#cheque-transaction-status-change').show();
        var commonAccountName = $('.commonName').find('#AccountName');
        $(commonAccountName).val(null);
    }
    if (e.keyCode == 13) {

        $.ajax({
            type: 'GET',
            url: "/Teller/GetAccountNumber",
            data: {
                accountNumber: accountNumber,
                filterAccount: filterAccount,
                accountType: accountType
            },
            success: function (result) {
                debugger;
                $("#cheque-transaction-status-change").html('');
                $('#cheque-transaction-status-change').show();
                var count = result.length;
                if (count == 0) {
                    ErrorAlert("Account not found", 5000);
                    document.getElementById('alert-error').scrollIntoView();
                    $(_globalObject).val("");
                    $(_globalAccountValue).find(".account-id").val("");
                }
                if (count == 1) {
                    var eventid = $("#MessageEventID").val();
                    if (eventid == undefined || eventid == "") {
                        eventid = 0;
                    }
                    $.ajax({
                        //reterive customer from accno and display
                        type: 'GET',
                        url: '/Customer/GetCustomerEventListByAccountId',
                        data: {
                            accountid: result[0].IAccno,
                            eventid: eventid,
                        },
                        success: function (CustomerInfo) {
                            $(".customer-event-message").html(CustomerInfo);
                        }
                    });

                    $(_globalObject).val(result[0].Accno);
                    $(_globalAccountValue).find(".account-id").val(result[0].IAccno);
                    CheckAccountStatus(result[0].IAccno)


                    if ((data != "NoDisplay") && (data != "CollateralDisplay") && (data != "Report") && (data != "IsFDLoan") && (data != "AccountClose")) {

                        $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                        $(viewDetailsBtn).attr('Action', 'Show')
                        $(viewDetailsBtn).removeClass('hidden');
                        $('#account-details-show-div').addClass('hidden');
                        $(viewDetailsBtn).removeClass('hidden');
                    }

                    if (data == "AccountClose") {
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/_AcountDetailsViewShow',
                            data: {
                                accountId: result.AccountId,
                                showType: "AccountClose"
                            },
                            success: function (accountDetailsShow) {
                                $('#account-details-show-div').removeClass('hidden');
                                $('#account-details-show-div').html(accountDetailsShow);
                            }
                        })
                    }
                    if (data == "ChargableAccounts") {
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/GetFixedAccountsByAccountId',
                            data: {
                                Iaccno: result[0].IAccno,
                            },
                            success: function (Isfixed) {
                                debugger;
                                if (Isfixed) {
                                    $.MessageBox({
                                        buttonDone: "OK",
                                        message: "Fixed Account Cannot be Chargeable"
                                    }).done(function () {

                                    }).fail(function () {
                                        //return false;
                                    })
                                }

                            },
                        });

                    }
                    if (data == "ImageEvent") {

                        $('.img-container').hide();
                        $('.view-history-image').hide();
                        var flag = $('.signature-display').find('#Display:checked').val();
                        if (flag == 1) {
                            var id = $('.display-account-signature').find('.account-signature-create').find('.account-id').val();
                        }

                        else if (flag == 3) {
                            var id = $('.display-account-signature').find('.share-signature-create').find('.employeeId').val();
                        }
                        else {
                            var id = $('.display-account-signature').find('.customer-photo-create').find('.multiselectCustomer').find('option:selected').val();
                        }


                        $.ajax({
                            url: "/Signature/GetAll",
                            type: "GET",

                            contentType: "application/json; charset=utf-8",
                            dataType: "html",
                            data: { flag: flag, id: id },
                            failure: function (response) {
                                console.log(response);
                            },
                            error: function (response) {
                                console.log(response);
                            },
                            success: function (data) {
                                debugger;
                                $('.img-container').html(data);

                                $('.img-container').fadeIn(3000);
                                //$('.view-history-image').show();

                            },


                        });


                    }

                    //if (data == "ChequeTransHistory") {

                    //    $.ajax({
                    //        type: 'GET',
                    //        url: '/Information/StatusChequeList',
                    //        data: {
                    //            iAccNo: result[0].IAccno,
                    //        },
                    //        success: function (chequeTrans) {
                    //            $("#cheque-transaction-status-change").html(chequeTrans);

                    //        },
                    //    });
                    //}



                    //if (data === "ChequeTransHistory") {
                    //    $('.table-cheque-block-unblock-deactive-status').hide();
                    //    $('#cheque-transaction-status-change').hide();
                    //    $.ajax({
                    //        type: 'GET',
                    //        url: '/Information/StatusChequeList',
                    //        data: {
                    //            iAccNo: result.AccountId,
                    //        },
                    //        success: function (chequeTrans) {
                    //            $("#cheque-transaction-status-change").html(chequeTrans);
                    //            $('.table-cheque-block-unblock-deactive-status').show();
                    //            //$("#cheque-transaction-status-change").show();

                    //        },
                    //    });

                    //}


                    if (data == "WithdrawWithIntPay") {
                        CheckAccountStatus(result[0].IAccno)
                        $('.view-signature-div').show();
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/InterestPayable',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (intPayable) {
                                debugger;
                                if (intPayable.IsNomee == true) {
                                    if (intPayable.Amount != 0) {

                                        $('a.intPayable').removeClass('hidden')
                                        $.MessageBox({
                                            buttonDone: "OK",
                                            message: "Available Interest Payable Amount=NRS." + intPayable.Amount
                                        }).done(function () {
                                           
                                        }).fail(function () {
                                            //return false;
                                        })

                                        $('.payableamount').html("NRS. " + intPayable.Amount);
                                        
                                    }
                                } else {
                                    $('a.intPayable').addClass('hidden')
                                 
                                }
                            },
                        });
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/CheckWithdrawPopUpValidation',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (popValidation) {

                                if (!popValidation.Success) {

                                    //$(idAttribute).val("");
                                    //$(_globalAccountValue).find(".account-id").val("");
                                    ErrorAlert(popValidation.Msg, 5000);
                                    document.getElementById('alert-error').scrollIntoView();
                                } else {
                                    if (popValidation.ReturnId != 1) {
                                        $.MessageBox({
                                            buttonDone: "OK",
                                            message: popValidation.Msg
                                        }).done(function () {
                                          
                                        }).fail(function () {
                                            //return false;
                                        })
                                    }
                                  
                                }

                                
                            },
                        });

                    }
                    if (data == "Deposit") {
                        CheckAccountStatus(result[0].IAccno)
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/CheckDepositPopUpSelectCondition',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (popValidation) {

                                if (!popValidation.Success) {

                                    //$(idAttribute).val("");
                                    //$(_globalAccountValue).find(".account-id").val("");
                                    ErrorAlert(popValidation.Msg, 5000);
                                    document.getElementById('alert-error').scrollIntoView();
                                }
                            },
                        });
                    }
                    if (data == "LoanDetails") {
                        debugger;
                        var isOtherLoan = $(".disburse-loan-index").find(".isOtherLoan").val();
                        $.ajax({
                            type: 'GET',
                            url: '/Credit/CheckPreviousLoanDisburse',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (existingdetails) {
                                if (existingdetails.Success == false) {
                                    $.ajax({
                                        type: 'GET',
                                        url: '/credit/LoanDisburseDetailsByIaccno',
                                        data: {
                                            accountId: result[0].IAccno, isOtherLoan: isOtherLoan
                                        },
                                        success: function (loanDisburseDetails) {
                                            debugger;
                                            $(".loan-disburse-details").html(loanDisburseDetails)
                                            $(".loan-disburse-details").show();
                                            $('.disbursement-schedule-final').show();
                                            $('.loan-disburse-details').show();
                                            $('.chargeDeductionType').show();
                                            var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                            var productId = $(".loan-disburse-details").find("#Product_ProductId").val();

                                            if (isOtherLoan == "False") {
                                                if (ischargeable == "True") {
                                                    $.ajax({
                                                        type: "GET",
                                                        url: "/FinanceParameter/_ChargeAutoTriggered",
                                                        datatype: "html",
                                                        data: {
                                                            productId: productId, modevent: 20
                                                        },
                                                        success: function (result) {
                                                            debugger;
                                                            $(".disburse-loan-index").find(".chargeDetails").html("");
                                                            $(".disburse-loan-index").find(".chargeDetails").html(result);

                                                        }
                                                    })
                                                }
                                            }

                                        }
                                    })
                                }
                                else {
                                    ErrorAlert(existingdetails.Msg, 5000)
                                 
                                    $('.disbursement-schedule-final').hide();
                                    $('.loan-disburse-details').hide();
                                    $('.chargeDeductionType').hide();
                                    return false;

                                }
                            }
                        })
                    }
                    if (data == "LoanPayment") {

                        $.ajax({
                            type: 'GET',
                            url: '/Credit/LoanAccountPayment',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (payment) {
                                debugger;
                                $('.load-loan-account-payment-data').html(payment)
                            },
                        });
                        $.ajax({
                            type: 'GET',
                            url: '/Credit/_LoanAccountDetailsInformationShow',
                            data: {
                                accountId: result[0].IAccno,
                            },
                            success: function (paymentdetails) {
                                debugger;
                                $('.load-loan-account-details-data').html(paymentdetails)
                            },
                        });
                        if (
                            $("#ManualDistribute").prop('checked')) {
                            $("#ManualDistribute").prop('checked', false)
                            $('.loan-total-amount-payment').prop("readonly", false)
                            $('.Rebate-loan-payment').prop("readonly", false)
                        }
                        $('.loan-total-amount-payment').val(0)
                        $('.Rebate-loan-payment').val(0)
                        $("#ManualDistribute").prop("disabled", false);

                    }


                    if (data == "CollateralDisplay") {
                        $.ajax({
                            type: 'GET',
                            url: '/Credit/CollateralInternalAccountNoDetails',
                            data: {
                                iAccNo: result[0].IAccno,
                            },
                            success: function (details) {

                                $("#Balance").val(details.Amount);
                                $('.internal-account-mat').find('.txtDateAD').val(details.endDate);
                                $('.internal-account-mat').find('.txtDateBS').val(details.nepDate);
                                $('.internal-account-mat').find('.txtDateValue').val(details.date);

                            },
                        });
                    }

                    if (data == "Adjustment") {

                        var iccno = $('.account-id').val();
                        $.ajax({
                            url: "/Correction/GetAdjustmentType",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: { accNo: iccno },
                            success: function (adjustmentType) {

                                var optionList = '';
                                $.each(adjustmentType, function (i, adjustment) {
                                    optionList = optionList + "<option value='" + adjustment.Value + "'>" + adjustment.Text + "</option>"
                                });
                                $('#adjustment-type').html(optionList);

                            },
                        });

                        $('#adjustment-type').on('change', function (e) {

                            e.stopPropagation;
                            var interestSelected = $('#adjustment-type option:selected').val();
                            if (interestSelected === "-1") {
                                ErrorAlert("Please select Adjustment Type", 5000);

                            }
                            else {
                                var iccno = $('.account-id').val();
                                $.ajax({
                                    url: "/Correction/GetOldBalance",
                                    type: "GET",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "JSON",
                                    data: { accNo: iccno, index: interestSelected },
                                    success: function (getbalance) {

                                        $('.oldbalance').val(getbalance).prop('disabled', true);

                                    },


                                });
                            }

                        });
                    }
                    if (data == "IsFDLoan") {
                        debugger;
                        var Pid = $('#PID').val();
                        $.ajax({
                            type: 'GET',
                            url: '/Credit/GetFDLoanDetails',
                            data: {
                                accountId: result.AccountId,
                                Pid: Pid,
                                Modelfrom:"fromfd",
                            },
                            success: function (fdloan) {
                                if (fdloan.IsGurantor == true && fdloan.hasBalance == true) {
                                    debugger
                                    $.MessageBox({
                                        buttonDone: "Yes",
                                        buttonFail: "No",
                                        message: "<span style='color:red;'><center>This FD Account is already a guarantor!!</center></span> <br/> <span style='padding:4px;'><center>Are you sure?</center></span>"
                                    }).done(function () {

                                        debugger;
                                        $('#guarantor-Entry-table-div >tbody').empty();
                                        $('#link-account-table-div .MoveableRow').empty();
                                        $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                                        $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                                        $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                                        $(".guaranty-account").find('.account-id').val(result.AccountId)
                                        $(".guaranty-account").find(".account-aumber").val(result.AccountNumber);
                                        $(".link-accounts-search").find(".account-id").val(result.AccountId).prop("readonly", true);
                                        $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber).prop("readonly", true);
                                        $('#LAmt').val(fdloan.Balance);
                                        $('#InterestRate').val(fdloan.IRate);
                                        //var allObj = $('.loan-registration-details').find('.day-month');
                                        //$(allObj).each(function (index, item) {
                                        // if (index == 0) {
                                        //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                                        //          }
                                        //    else {
                                        //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                                        //        }
                                        //})

                                        //$(".day-month").text("Payment-Description[Day(s)]")
                                        $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                                        $("#GrantedDuration").val(fdloan.fixedDuration)
                                        $("#Payment_Mode").val(fdloan.PaymentMode)
                                        //$("#IFreq").val(fdloan.IFreq)
                                        $("#IFreq").val(1)
                                        $("#PFreq").val(fdloan.fixedDuration)
                                        $("#PAYSID").val(fdloan.ScheduleType)
                                        // $(".loan-registration-details").find("#Duration").prop('readonly', true);
                                    });
                                }
                                else if (fdloan.IsGurantor == true || fdloan.hasBalance == false) {
                                    ErrorAlert("This guarantor doenot have sufficient balance", 5000);
                                    return false;
                                }
                                else {
                                    debugger;
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                                    $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                                    $(".guaranty-account").find('.account-id').val(result.AccountId)
                                    $(".guaranty-account").find(".account-aumber").val(result.AccountNumber);
                                    $(".link-accounts-search").find(".account-id").val(result.AccountId);
                                    $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber);
                                    $('#LAmt').val(fdloan.Balance);
                                    $('#InterestRate').val(fdloan.IRate);
                                    //var allObj = $('.loan-registration-details').find('.day-month');
                                    //$(allObj).each(function (index, item) {
                                    // if (index == 0) {
                                    //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                                    //          }
                                    //    else {
                                    //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                                    //        }
                                    //})

                                    //$(".day-month").text("Payment-Description[Day(s)]")
                                    $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                                    $("#GrantedDuration").val(fdloan.fixedDuration)
                                    $("#Payment_Mode").val(fdloan.PaymentMode)
                                    //$("#IFreq").val(fdloan.IFreq)
                                    $("#IFreq").val(1)
                                    $("#PFreq").val(fdloan.fixedDuration)
                                    $("#PAYSID").val(fdloan.ScheduleType)
                                }
                            },

                        });
                    }

                    if (data == "WithOutAccountSearch") {
                        debugger;
                        var accountNumber = $('#cheque-registration-list').find('#Iaccno').val();
                       
                        $.ajax({
                            type: 'GET',
                            url: '/Information/_NewChequeIssueIndex',
                            data: {
                                accountNumber: accountNumber,

                            },
                            success: function (result) {
                                debugger;
                                $('.table-cheque-issue-registration').html(result);

                            },

                        });
                    }

                    var commonAccountName = $('.commonName').find('#AccountName');

                    $(commonAccountName).val(result[0].AccountName)

                } else {
                    $.ajax({
                        type: 'GET',
                        url: "/Teller/AccountNumberSearch",
                        data: {
                            accountNumber: accountNumber,
                            filterAccount: filterAccount,
                            accountType: accountType
                        },
                        success: function (result) {
                            debugger;
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
    var accountNumber = $(this).closest('tr').attr('id');
    var parent = $(this).parents();
    var idAttribute = _globalObject;
    var data = $(idAttribute).attr('showwith');
    var eventid = $("#MessageEventID").val();
    //$(parent).find(".account-number-div").find(".account-aumber#" + idAttribute).attr('showwith');
    var withDetails = $(idAttribute).attr("withDetails");
  
    var viewDetailsBtn = $(_globalAccountValue).find("button#btn-account-view-details-user-Account");
    $(".account-related-charge").find(".depositName").val(null);
    $(".account-related-charge").find(".loanName").val(null);
    $.ajax({
        type: 'GET',
        url: '/Teller/GetSelectAccountNumber',
        data: {
            accountNumber: accountNumber
        },
        success: function (result) {
            debugger;
            // $('.account-number-div').find('#AccountNameByNumber').html("");
            //  $('#AccountName').val(result.AccountName);
            if (eventid == undefined || eventid == "") {
                eventid = 0;
            }
            $.ajax({
                //reterive customer from accno and display
                type: 'GET',
                url: '/Customer/GetCustomerEventListByAccountId',
                data: {
                    accountid: result.AccountId,
                    eventid: eventid,
                },
                success: function (CustomerInfo) {
                    debugger;
                    $(".customer-event-message").html(CustomerInfo);
                }
            });

            //var tempData='@TempData["Key"]';
            if (data == "IsFDLoan" || data == "GuarantorDetail") {
                $.ajax({
                    //reterive customer from accno and display
                    async: false,
                    type: 'GET',
                    url: '/Credit/GetGuarantorDetail',

                    data: {
                        accountid: result.AccountId,

                    },
                    contentType: "application/json;charset=utf-8",
                    datatype: "json",
                    success: function (guarantorModel) {

                        debugger;
                        //console.log('@TempData["Key"]')

                        //if (tempData==true) {
                  
                        $('#display-Entry-table-div >tbody').empty();
                        $('#display-Entry-table-div >tbody').append(guarantorModel.htmlToShow);


                        //$('.Guarantor-Entry-Details').find('.guaranty-account').find('.account-id').val("")
                        //$('.Guarantor-Entry-Details').find('.guaranty-account').find('.account-aumber').val("")

                        // $("#BlockedAmt").val("");

                        //} else {
                        //    ErrorAlert(result.Message, 5000);
                        //    document.getElementById('alert-error').scrollIntoView();
                        //}
                    }
                });

            }


            debugger;
            $(idAttribute).val(result.AccountNumber);
            var id = $("#Modules option:selected").val();
            if (id == 1)
            {
                $(".account-related-charge").find(".depositName").val(result.AccountName);
            }

            if (id == 2) {
                $(".account-related-charge").find(".loanName").val(result.AccountName);
            }
               
            

            
                
            

            $(_globalAccountValue).find(".account-id").val(result.AccountId);
            if (withDetails == "True") {
                if ((data != "NoDisplay") && (data != "CollateralDisplay") && (data != "Report") && (data != "IsFDLoan") && (data != "AccountClose")) {

                    $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                    $(viewDetailsBtn).attr('Action', 'Show')
                    $(viewDetailsBtn).removeClass('hidden');
                    $('#account-details-show-div').addClass('hidden');
                    $(viewDetailsBtn).removeClass('hidden');
                }
            }
            //else {
            //    $(viewDetailsBtn).hide();
            //}
            debugger;
            if (data == "AccountClose") {
                $.ajax({
                    type: 'GET',
                    url: '/Teller/_AcountDetailsViewShow',
                    data: {
                        accountId: result.AccountId,
                        showType: "AccountClose"
                    },
                    success: function (accountDetailsShow) {
                        debugger;
                        $('#account-details-show-div').removeClass('hidden');
                        $('#account-details-show-div').html(accountDetailsShow);
                        $('#btn-submit-force-close-account').show();
                    }
                })
            }
            if (data == "ChargableAccounts") {

                $.ajax({
                    type: 'GET',
                    url: '/Teller/GetFixedAccountsByAccountId',
                    data: {
                        Iaccno: result.AccountId,
                    },
                    success: function (Isfixed) {

                        if (Isfixed) {
                            $.MessageBox({
                                buttonDone: "OK",
                                message: "Fixed Account Cannot be Chargeable"
                            }).done(function () {

                            }).fail(function () {
                                //return false;
                            })
                        }

                    },
                });

            }
            if (data == "ChequeTransHistory") {
                debugger;
                $.ajax({
                    type: 'GET',
                    url: '/Information/StatusChequeList',
                    data: {
                        iAccNo: result.AccountId,
                    },
                    success: function (chequeTrans) {
                        $("#cheque-transaction-status-change").html(chequeTrans);
                        $('#cheque-transaction-status-change').show();
                    },
                });
            }
            if (data == "ImageEvent") {
                debugger;
                $('.img-container').hide();
                $('.view-history-image').hide();
                var flag = $('.signature-display').find('#Display:checked').val();
                if (flag == 1) {
                    var id = $('.display-account-signature').find('.account-signature-create').find('.account-id').val();
                }

                else if (flag == 3) {
                    var id = $('.display-account-signature').find('.share-signature-create').find('.employeeId').val();
                }
                else {
                    var id = $('.display-account-signature').find('.customer-photo-create').find('.multiselectCustomer').find('option:selected').val();
                }


                $.ajax({
                    url: "/Signature/GetAll",
                    type: "GET",

                    contentType: "application/json; charset=utf-8",
                    dataType: "html",
                    data: { flag: flag, id: id },
                    failure: function (response) {
                        console.log(response);
                    },
                    error: function (response) {
                        console.log(response);
                    },
                    success: function (data) {
                        debugger;
                        $('.img-container').html(data);

                        $('.img-container').fadeIn(3000);
                        //$('.view-history-image').show();

                    },


                });


            }

            if (data == "WithdrawWithIntPay") {
                debugger;
                CheckAccountStatus(result.AccountId)
                $('.view-signature-div').show();
                $.ajax({
                    type: 'GET',
                    url: '/Teller/InterestPayable',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (intPayable) {
                        debugger;
                        if (intPayable.IsNomee == true) {
                            if (intPayable.Amount != 0) {

                                $('a.intPayable').removeClass('hidden')
                                $.MessageBox({
                                    buttonDone: "OK",
                                    message: "Available Interest Payable Amount=NRS." + intPayable.Amount
                                }).done(function () {
                                  
                                    //return false;
                                })

                                $('.payableamount').html("NRS. " + intPayable.Amount);
                               
                            }
                        } else {
                            $('a.intPayable').addClass('hidden');
                           
                        }
                    },
                });
                debugger
                $.ajax({
                    type: 'GET',
                    url: '/Teller/CheckWithdrawPopUpValidation',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (popValidation) {
                        debugger;
                        if (!popValidation.Success) {

                            //$(idAttribute).val("");
                            //$(_globalAccountValue).find(".account-id").val("");
                            ErrorAlert(popValidation.Msg, 5000);
                            document.getElementById('alert-error').scrollIntoView();
                        } else {
                            if (popValidation.ReturnId != 1) {
                                $.MessageBox({
                                    buttonDone: "OK",
                                    message: popValidation.Msg
                                }).done(function () {
                                   
                                }).fail(function () {
                                    //return false;
                                })
                                //$('.btn-cheque-clearence-save').prop('disabled', true);
                            }

                        }
                       
                    },
                });

            }
            if (data == "Deposit") {
                CheckAccountStatus(result.AccountId)
                $.ajax({
                    type: 'GET',
                    url: '/Teller/CheckDepositPopUpSelectCondition',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (popValidation) {
                        debugger;
                        if (!popValidation.Success) {

                            //$(idAttribute).val("");
                            //$(_globalAccountValue).find(".account-id").val("");
                            ErrorAlert(popValidation.Msg, 5000);
                            document.getElementById('alert-error').scrollIntoView();
                        }
                        else {
                            var accountId = result.AccountId;
                            debugger;
                            $.ajax({

                                type: 'GET',
                                url: '/Teller/DisplayDefaultCurrency',
                                data: {
                                    accountId: accountId
                                },
                                success: function (result) {
                                    debugger;
                                    var dropdownlist = $('#AmountCurrency option');
                                    //$.each(dropdownlist, function (i, val) {
                                    //    debugger;
                                    //    if (result == this.val()) {

                                    //        $(dropdownlist).remove();
                                    //    }
                                    //})

                                

                                    $(dropdownlist).each(function () {
                                        debugger;
                                        var optionText=$(this).text();
                                        var optionTextValue=$(this).val();
                                        if (result == optionText) {
                                            //dropdownlist[optionTextValue].remove();
                                            debugger;
                                            dropdownlist[0].selectedIndex = 0;

                                        }
                                        //else {

                                        //    dropdownlist[optionTextValue].selectedIndex = 0;
                                         
                                        //}

                                    });

                                  
                                    
                                }

                            })
                        }
                    }
                });


            }
            if (data == "LoanDetails") {
                debugger;
                var isOtherLoan = $(".disburse-loan-index").find(".isOtherLoan").val();
                $.ajax({
                    type: 'GET',
                    url: '/Credit/CheckPreviousLoanDisburse',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (existingdetails) {
                        debugger;
                        if (existingdetails.Success == false) {
                            $.ajax({
                                type: 'GET',
                                url: '/credit/LoanDisburseDetailsByIaccno',
                                data: {
                                    accountId: result.AccountId, isOtherLoan: isOtherLoan
                                },
                                success: function (loanDisburseDetails) {
                                    debugger;
                                    $(".loan-disburse-details").html(loanDisburseDetails)
                                    $(".loan-disburse-details").show();
                                    $('.disbursement-schedule-final').show();
                                    $('.loan-disburse-details').show();
                                    $('.chargeDeductionType').show();
                                    var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                    var productId = $(".loan-disburse-details").find("#Product_ProductId").val();
                                    var AmountCharge = $(".loan-disburse-details").find("#DisbursableAmount").val();
                                    var checkSanction = $(".loan-disburse-details").find("#ChargeDeductOnSanction").prop('checked');
                                    var RegularLoanCharge = $(".loan-disburse-details").find("#RegularLoan").val();
                                    //    var accountNumber = $(".table-click-account-number-search table tr").closest('tr').attr('id');
                                    var accountNumber = result.AccountId;
                                    

                                    


                                    if (isOtherLoan == "False") {
                                        debugger;
                                        if (ischargeable == "True") {
                                            $.ajax({
                                                type: "GET",
                                                url: "/FinanceParameter/_ChargeAutoTriggered",
                                                datatype: "html",
                                                data: {
                                                    productId: productId, modevent: 20, AmountCharge: AmountCharge, checkSanction: checkSanction, RegularLoanCharge: RegularLoanCharge, accountNumber: accountNumber
                                                },
                                                success: function (result) {
                                                    debugger;
                                                    $(".disburse-loan-index").find(".chargeDetails").html("");
                                                    $(".disburse-loan-index").find(".chargeDetails").html(result);

                                                }
                                            })
                                        }
                                    }

                                }
                            })
                        }
                        else {
                            ErrorAlert(existingdetails.Msg, 5000)
                           
                            $('.disbursement-schedule-final').hide();
                            $('.loan-disburse-details').hide();
                            $('.chargeDeductionType').hide();
                            return false;
                        }
                    }
                })
            }
            if (data == "LoanPayment") {
                debugger;
                $.ajax({
                    type: 'GET',
                    url: '/Credit/LoanAccountPayment',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (payment) {
                        debugger;
                        $('.load-loan-account-payment-data').html(payment)
                    },
                });
                $.ajax({
                    type: 'GET',
                    url: '/Credit/_LoanAccountDetailsInformationShow',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (paymentdetails) {
                        debugger;
                        $('.load-loan-account-details-data').html(paymentdetails)
                    },
                });
                if (
                    $("#ManualDistribute").prop('checked')) {
                    $("#ManualDistribute").prop('checked', false)
                    $('.loan-total-amount-payment').prop("readonly", false)
                    $('.Rebate-loan-payment').prop("readonly", false)
                }
                $('.loan-total-amount-payment').val(0)
                $('.Rebate-loan-payment').val(0)
                $("#ManualDistribute").prop("disabled", false);

            }

            if (data == "LoanRebate") {

                $.ajax({
                    type: 'GET',
                    url: '/Credit/LoanRebateDetails',
                    data: {
                        accountId: result.AccountId,
                    },
                    success: function (rebate) {

                        $('.load-rebate-data').html(rebate)
                    },
                });


            }

            if (data == "CollateralDisplay") {
                $.ajax({
                    type: 'GET',
                    url: '/Credit/CollateralInternalAccountNoDetails',
                    data: {
                        iAccNo: result.AccountId,
                    },
                    success: function (details) {

                        $("#Balance").val(details.Amount);
                        $('.internal-account-mat').find('.txtDateAD').val(details.endDate);
                        $('.internal-account-mat').find('.txtDateBS').val(details.nepDate);
                        $('.internal-account-mat').find('.txtDateValue').val(details.date);

                    },
                });
            }
            if (data == "AdjustmentAccept") {
                debugger;
                var iccno = $('.account-id').val();
                $.ajax({
                    url: "/Correction/GetAdjustmentType",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: { accNo: iccno },
                    success: function (adjustmentType) {

                        var optionList = '';
                        $.each(adjustmentType, function (i, adjustment) {
                            optionList = optionList + "<option value='" + adjustment.Value + "'>" + adjustment.Text + "</option>"
                        });
                        $('#adjustment-type').html(optionList);

                    },
                });

                $('#adjustment-type').on('change', function (e) {
                    debugger;
                    e.stopPropagation;
                    var interestSelected = $('#adjustment-type option:selected').val();
                    if (interestSelected === "-1") {
                        ErrorAlert("Please select Adjustment Type", 5000);

                    }
                    else {
                        var iccno = $('.account-id').val();
                        $.ajax({
                            url: "/Correction/GetOldBalance",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "JSON",
                            data: { accNo: iccno, index: interestSelected },
                            success: function (getbalance) {

                                $('.oldbalance').val(getbalance).prop('disabled', true);

                            },


                        });
                    }

                });
            }

            if (data == "Adjustment") {

                var iccno = $('.account-id').val();
                $.ajax({
                    url: "/Correction/GetAdjustmentType",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: { accNo: iccno },
                    success: function (adjustmentType) {

                        var optionList = '';
                        $.each(adjustmentType, function (i, adjustment) {
                            optionList = optionList + "<option value='" + adjustment.Value + "'>" + adjustment.Text + "</option>"
                        });
                        $('#adjustment-type').html(optionList);

                    },
                });

                $('#adjustment-type').on('change', function (e) {
                    debugger;
                    e.stopPropagation;
                    var interestSelected = $('#adjustment-type option:selected').val();
                    if (interestSelected === "-1") {
                        ErrorAlert("Please select Adjustment Type", 5000);

                    }
                    else {
                        var iccno = $('.account-id').val();
                        $.ajax({
                            url: "/Correction/GetOldBalance",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "JSON",
                            data: { accNo: iccno, index: interestSelected },
                            success: function (getbalance) {
                                debugger;
                                $('.oldbalance').val(getbalance).prop('disabled', true);

                            },


                        });
                    }

                });
            }
            if (data == "IsFDLoan") {
                debugger;
                var Pid = $('#PID').val();
                $.ajax({
                    type: 'GET',
                    url: '/Credit/GetFDLoanDetails',
                    data: {
                        accountId: result.AccountId,
                        Pid: Pid,
                        Modelfrom:"fromfd",
                            
                    },
                    success: function (fdloan) {
                        if (fdloan.IsGurantor == true && fdloan.hasBalance == true) {
                            debugger
                            $.MessageBox({
                                buttonDone: "Yes",
                                buttonFail: "No",
                                message: "<span style='color:red;'><center>This FD Account is already a guarantor!!</center></span> <br/> <span style='padding:4px;'><center>Are you sure?</center></span>"
                            }).done(function () {

                                debugger;
                                      $('#guarantor-Entry-table-div >tbody').empty();
                                    $('#link-account-table-div .MoveableRow').empty();
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                                $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                                $(".guaranty-account").find('.account-id').val(result.AccountId).prop("readonly", false);
                                $(".guaranty-account").find(".account-aumber").val(result.AccountNumber).prop("readonly", false);;
                                $(".link-accounts-search").find(".account-id").val(result.AccountId).prop("readonly", true);
                                $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber).prop("readonly", true);
                                $('#LAmt').val(fdloan.Balance);
                                $('#InterestRate').val(fdloan.IRate);
                                //var allObj = $('.loan-registration-details').find('.day-month');
                                //$(allObj).each(function (index, item) {
                                // if (index == 0) {
                                //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                                //          }
                                //    else {
                                //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                                //        }
                                //})

                                //$(".day-month").text("Payment-Description[Day(s)]")
                                $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                                $("#GrantedDuration").val(fdloan.fixedDuration)
                                $("#Payment_Mode").val(fdloan.PaymentMode)
                                //$("#IFreq").val(fdloan.IFreq)
                                $("#IFreq").val(1)
                                $("#PFreq").val(fdloan.fixedDuration)
                                $("#PAYSID").val(fdloan.ScheduleType)
                                // $(".loan-registration-details").find("#Duration").prop('readonly', true);
                            });
                        }
                            else if(fdloan.IsGurantor == true || fdloan.hasBalance == false) {
                             ErrorAlert("This guarantor doenot have sufficient balance",5000);
                             return false;
}
                        else {
                            debugger;
                            $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateValue").val(fdloan.Date);
                            $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateAD").val(fdloan.EnglishDate)
                            $('.MaturationDate').find('.chdPickerMain').find("#chdPickerId").find(".txtDateBS").val(fdloan.NepaliDate)
                            $(".guaranty-account").find('.account-id').val(result.AccountId).prop("readonly", false)
                            $(".guaranty-account").find(".account-aumber").val(result.AccountNumber).prop("readonly", false);
                            $(".link-accounts-search").find(".account-id").val(result.AccountId);
                            $(".link-accounts-search").find("#LinkAccountNumber").val(result.AccountNumber);
                            $('#LAmt').val(fdloan.Balance);
                            $('#InterestRate').val(fdloan.IRate);
                            //var allObj = $('.loan-registration-details').find('.day-month');
                            //$(allObj).each(function (index, item) {
                            // if (index == 0) {
                            //     $(item).text("Granted Payment-Description [ Day(s)]")///changed to maximum duration
                            //          }
                            //    else {
                            //     $(item).text("Maximum Payment-Description[ Day(s) ]").prop("readonly", true);//added property
                            //        }
                            //})

                            //$(".day-month").text("Payment-Description[Day(s)]")
                            $("#Duration").val(fdloan.fixedDuration).prop("readonly", true)
                            $("#GrantedDuration").val(fdloan.fixedDuration)
                            $("#Payment_Mode").val(fdloan.PaymentMode)
                            //$("#IFreq").val(fdloan.IFreq)
                            $("#IFreq").val(1)
                            $("#PFreq").val(fdloan.fixedDuration)
                            $("#PAYSID").val(fdloan.ScheduleType)
                        }
                    },

                });
            }
            if (data == "WithOutAccountSearch") {
                debugger;
                var accountNumber = $('#cheque-registration-list').find('.account-id').val();
                $.ajax({
                    type: 'GET',
                    url: '/Information/_NewChequeIssueIndex',
                    data: {
                        accountNumber: accountNumber,

                    },
                    success: function (result) {
                        debugger;
                        $('.table-cheque-issue-registration').html(result);

                    },

                });
            }
            if (data == "NoDisplay") {
                var commonAccountName = $('.commonName').find('#AccountName');
                $(commonAccountName).val(result.AccountName);
            }

            //if (data == "DepositList") {
            //    debugger;
            //    var accountNumber = $('#IAccno').val();

            //    $.ajax({
            //        type: 'GET',
            //        url: '/Teller/_AccountList',
            //        data: {
            //            accountNumber: accountNumber,

            //        },
            //        success: function (result) {
            //            debugger;
            //            $('.table-cheque-issue-registration').html(result);

            //        },

            //    });
            //}

            //if (data == "RegisteredLoanAccount") {
            //    debugger;
            //    var accountNumber = $('#registered-loan-accountnumber').find('#iAccno').val();
            //    $.ajax({
            //        type: 'GET',
            //        url: '/Credit/VerifiedRegisteredLoanAccounts',
            //        data: {
            //            accountNumber: accountNumber,

            //        },
            //        success: function (result) {
            //            debugger;
            //            if (result.Success) {
            //                $.ajax({
            //                    url: "/Credit/VerifiedRegisteredLoanAccountsIndex",
            //                    type: 'GET',
            //                    async: false,
            //                    success: function (result) {
            //                        debugger;
            //                        $('section.content').html(result);
            //                    }
            //                })
            //                //$('#registered-loan-accountnumber-list').html(result);
            //            }

            //        else{

            //    }
            //        },

            //    });
            //}


            //var commonAccountName = $('.commonName').find('#AccountName');
            //$(commonAccountName).val(result.AccountName);


            $($(parent).find("#account-pop-up-div")).modal('toggle');


            $(idAttribute).focus();
            $(_globalAccountValue).find(".account-id").focus();
        },

    });

});

$('.btn-account-view-details-button').on('click', function (e) {
    debugger;
    e.stopImmediatePropagation();
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


    var idAttribute = _globalAccountValue;
    var showType = $(idAttribute).find(".account-aumber").attr('showwith');
    var AccountId = $(idAttribute).find(".account-id").val();
    if (AccountId == 0 || AccountId == "") {
        return;
    }
    $.ajax({
        type: 'GET',
        url: '/Teller/_AcountDetailsViewShow',
        data: {
            accountId: AccountId,
            showType: showType
        },
        success: function (accountDetailsShow) {
            debugger;
            $('#account-details-show-div').removeClass('hidden');
            $('#account-details-show-div').html(accountDetailsShow);
        }
    });
});

function CheckAccountStatus(accountId) {
    debugger;
    $.ajax({
        type: 'GET',
        url: '/Teller/CheckCurrentAccountStatus',
        data: {
            accountId: accountId,
        },
        success: function (resultSuccess) {
            if (!resultSuccess.Success) {
                ErrorAlert(resultSuccess.Msg, 5000);
                document.getElementById('alert-error').scrollIntoView();
            }

        }
    });
}



// var accountNumber = $(".table-click-account-number-search table tr").closest('tr').attr('id');
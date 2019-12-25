var _globalObject;
var _globalAccountValue;
$("button.btn-account-open-search").on('click', function (e) {

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
            accountType: accountTypes
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
        $(_globalObject).val("");
        $(_globalAccountValue).find(".account-id").val("");
        return;
    }

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

            var count = result.length;
            if (count == 0) {
                ErrorAlert("Account not found", 5000);
                document.getElementById('alert-error').scrollIntoView();
                $(_globalObject).val("");
                $(_globalAccountValue).find(".account-id").val("");
            }
            if (count == 1) {
                $(_globalObject).val(result[0].Accno);
                $(_globalAccountValue).find(".account-id").val(result[0].IAccno);
                CheckAccountStatus(result[0].IAccno)
                if ((data != "NoDisplay") && (data != "CollateralDisplay")) {

                    $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                    $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                    $(viewDetailsBtn).attr('Action', 'Show')
                    $(viewDetailsBtn).removeClass('hidden');
                    $('#account-details-show-div').addClass('hidden');
                    $(viewDetailsBtn).removeClass('hidden');

                    if (data == "ChargableAccounts") {
                        $.ajax({
                            type: 'GET',
                            url: '/Teller/GetFixedAccountsByAccountId',
                            data: {
                                Iaccno: result[0].IAccno,
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

                        $.ajax({
                            type: 'GET',
                            url: '/Information/StatusChequeList',
                            data: {
                                iAccNo: result[0].IAccno,
                            },
                            success: function (chequeTrans) {
                                $("#cheque-transaction-status-change").html(chequeTrans);

                            },
                        });
                    }
                    if (data == "WithdrawWithIntPay") {

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
                                            $(".loan-disburse-details").html(loanDisburseDetails)
                                            var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                            var productId = $(".loan-disburse-details").find("#Product_ProductId").val();
                                            debugger;
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
                } else {
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
                    var commonAccountName = $('.commonName').find('#AccountName');

                    $(commonAccountName).val(result[0].AccountName)

                }
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
                var count = result.length;
                if (count == 0) {
                    ErrorAlert("Account not found", 5000);
                    document.getElementById('alert-error').scrollIntoView();
                    $(_globalObject).val("");
                    $(_globalAccountValue).find(".account-id").val("");
                }
                if (count == 1) {
                    $(_globalObject).val(result[0].Accno);
                    $(_globalAccountValue).find(".account-id").val(result[0].IAccno);
                    CheckAccountStatus(result[0].IAccno)
                    if ((data != "NoDisplay") && (data != "CollateralDisplay")) {

                        $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                        $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                        $(viewDetailsBtn).attr('Action', 'Show')
                        $(viewDetailsBtn).removeClass('hidden');
                        $('#account-details-show-div').addClass('hidden');
                        $(viewDetailsBtn).removeClass('hidden');

                        if (data == "ChargableAccounts") {
                            $.ajax({
                                type: 'GET',
                                url: '/Teller/GetFixedAccountsByAccountId',
                                data: {
                                    Iaccno: result[0].IAccno,
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

                            $.ajax({
                                type: 'GET',
                                url: '/Information/StatusChequeList',
                                data: {
                                    iAccNo: result[0].IAccno,
                                },
                                success: function (chequeTrans) {
                                    $("#cheque-transaction-status-change").html(chequeTrans);

                                },
                            });
                        }
                        if (data == "WithdrawWithIntPay") {

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
                                                $(".loan-disburse-details").html(loanDisburseDetails)
                                                var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                                var productId = $(".loan-disburse-details").find("#Product_ProductId").val();
                                                debugger;
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
                    } else {
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
                        var commonAccountName = $('.commonName').find('#AccountName');

                        $(commonAccountName).val(result[0].AccountName)

                    }
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

    e.stopImmediatePropagation();
    var accountNumber = $(this).closest('tr').attr('id');
    var parent = $(this).parents();
    var idAttribute = _globalObject;
    var data = $(idAttribute).attr('showwith');

    //$(parent).find(".account-number-div").find(".account-aumber#" + idAttribute).attr('showwith');
    var viewDetailsBtn = $(_globalAccountValue).find("button#btn-account-view-details-user-Account");
    $.ajax({
        type: 'GET',
        url: '/Teller/GetSelectAccountNumber',
        data: {
            accountNumber: accountNumber
        },
        success: function (result) {

            $(idAttribute).val(result.AccountNumber);
            $(_globalAccountValue).find(".account-id").val(result.AccountId);

            if ((data != "NoDisplay") && (data != "CollateralDisplay")) {

                $(viewDetailsBtn).find('.fa-toggle-off').addClass('hidden');
                $(viewDetailsBtn).find('.fa-toggle-on').removeClass('hidden');
                $(viewDetailsBtn).attr('Action', 'Show')
                $(viewDetailsBtn).removeClass('hidden');
                $('#account-details-show-div').addClass('hidden');
                $(viewDetailsBtn).removeClass('hidden');

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

                    $.ajax({
                        type: 'GET',
                        url: '/Information/StatusChequeList',
                        data: {
                            iAccNo: result.AccountId,
                        },
                        success: function (chequeTrans) {
                            $("#cheque-transaction-status-change").html(chequeTrans);

                        },
                    });
                }
                if (data == "WithdrawWithIntPay") {

                    $.ajax({
                        type: 'GET',
                        url: '/Teller/InterestPayable',
                        data: {
                            accountId: result.AccountId,
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
                            }
                        },
                    });
                    $.ajax({
                        type: 'GET',
                        url: '/Teller/CheckWithdrawPopUpValidation',
                        data: {
                            accountId: result.AccountId,
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

                    $.ajax({
                        type: 'GET',
                        url: '/Teller/CheckDepositPopUpSelectCondition',
                        data: {
                            accountId: result.AccountId,
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
                            accountId: result.AccountId,
                        },
                        success: function (existingdetails) {
                            if (existingdetails.Success == false) {
                                $.ajax({
                                    type: 'GET',
                                    url: '/credit/LoanDisburseDetailsByIaccno',
                                    data: {
                                        accountId: result.AccountId, isOtherLoan: isOtherLoan
                                    },
                                    success: function (loanDisburseDetails) {
                                        $(".loan-disburse-details").html(loanDisburseDetails)
                                        var ischargeable = $(".loan-disburse-details").find("#IsChargeAvailable").val();
                                        var productId = $(".loan-disburse-details").find("#Product_ProductId").val();
                                        debugger;
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
                            }
                        }
                    })
                }
                if (data == "LoanPayment") {

                    $.ajax({
                        type: 'GET',
                        url: '/Credit/LoanAccountPayment',
                        data: {
                            accountId: result.AccountId,
                        },
                        success: function (payment) {

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
            } else {
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
                var commonAccountName = $('.commonName').find('#AccountName');

                $(commonAccountName).val(result.AccountName)

            }
            $($(parent).find("#account-pop-up-div")).modal('toggle');

            CheckAccountStatus(result.AccountId)
            $(idAttribute).focus()
            $(_globalAccountValue).find(".account-id").focus()
        },

    });

});

$('.btn-account-view-details-button').on('click', function (e) {

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
            $('#account-details-show-div').removeClass('hidden');
            $('#account-details-show-div').html(accountDetailsShow);
        }
    })
});

function CheckAccountStatus(accountId) {
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
    })
}

/*Created By: Bishal Thapaliya

Created On:11/1/2017

Modified By:Bishal Thapaliya

Modified On:12/14/2017
*/




//Get Voucher Batch Description

//var currentVoucherEvent;

//var voucherType = $("select#VTypeId option:selected").val();
//var container = $('.Voucher1-create').find('.batch-number');

//var form = $(container).closest("form");
//debugger;
//if (voucherType != null)
//{
//    $.ajax({
//        url: '/Voucher1/GetBatch/',
//        data: { type: voucherType },
//        dataType: "html",
//        success: function (data) {
//            $(container).show();
//            $(container).html(data);
//            $(form).removeData("validator");
//            $(form).removeData("unobtrusiveValidation");

//        }
//    });
//}


//End GetBatch Description


//Start Add Voucher Row For Input

$('#mainVoucher').on('click', '#AddTableRow', function (e) {
    e.stopImmediatePropagation()
    debugger;
    var container = $('.Voucher1-create').find('.voucher2-container')
    var form = $(container).closest("form");
    $.ajax({
        url: '/Voucher1/AddTableRow/',
        dataType: "html",
        success: function (data) {
            debugger
            $(container).show();
            if (data.includes('<ul') || data.includes('<li')) {
                var newdata = data.replace('<ul>', '').replace('<li>', '').replace('</ul>', '').replace('</li>', '');
                newdata = newdata.slice(160);
                $(container).append(newdata);
            }
            else {
                $(container).append(data);
            }
            var voucherType = $('select#VTypeId').val();
            var debitcontainer = $('table#voucher2Table').find('input.dbamnt');
            var creditcontainer = $('table#voucher2Table').find('input.cramnt');

            if (voucherType == 1) {
                $('select#voucherSelect').val(0);
                $('select#voucherSelect').prop("disabled", false);
                $(creditcontainer).hide();
                $(creditcontainer).val('');
                $(debitcontainer).show();

            }
            else if (voucherType == 2) {
                $('select#voucherSelect').val(0);
                $('select#voucherSelect').prop("disabled", true);
                $(creditcontainer).hide();
                $(creditcontainer).val('');
                $(debitcontainer).show();

            }
            else if (voucherType == 3) {
                $('select#voucherSelect').val(1);
                $('select#voucherSelect').prop("disabled", true);
                $(debitcontainer).hide();
                $(debitcontainer).val('');
                $(creditcontainer).show();
            }


            $(form).removeData("validator");
            $(form).removeData("unobtrusiveValidation");
        }
    });

});

//End Add Voucher Row


//Start Ledger Search Button

$('tbody.voucher2-container').on('keydown ', 'input#ledgerSearch', function (event) {
    debugger;
    voucherEvent = $(this);
    var currentEvent = $(this);
    var text = $(this).val();
    if (event.type == "keydown") {
        var checkevent = event.keyCode;
        if (checkevent == 9 || checkevent == 13) {
            $.fn.LoadVoucher(currentEvent, text);

        }
    }
    else {
        $.fn.LoadVoucher(currentEvent, text);
    }


})
//.on('focusout', 'input#ledgerSearch', function (event) {
//    debugger;
//    var currentEvent = $(this);
//    var text = $(this).val();
//    $.fn.LoadVoucher(currentEvent, text);
//});

//End Ledger Search


//Function to Search and Load Voucher

$.fn.LoadVoucher = function (trapmyEvent, text) {
    var currentEvent = trapmyEvent;

    $.ajax({
        type: "POST",
        url: '/Voucher1/SearchVoucherList/',
        dataType: "json",
        data: { text: text }
    })
        .done(function (ledgerName) {
            debugger;
            var count = ledgerName.length;
            if (count == 1) {
                var currentText = ledgerName[0].FName;
                var currentId = ledgerName[0].Fid;

                if (ledgerName[0].HasBankInfo == true || ledgerName[0].HasSubsiList == true || ledgerName[0].HasDimension) {
                    var url = "/Voucher1/EnterLedgerDetails";
                    var data = { "ledgerId": currentId };
                    $.get(url, data, function (data) {

                        $('#getVoucherDetails').html(data);
                        $('#getVoucherDetails').find('.modal-body').find('.Search').val(text);
                        debugger;

                        $('#getVoucherDetails').modal('show');
                    });
                }

                else {
                    currentEvent.val(currentText);
                    currentEvent.closest('.voucher2-container').find('input#VLedger-Id').val(currentId);
                }
            }
            else {

                //$.each(ledgerName.data, function (key, value) {
                debugger;
                var url = "/Voucher1/AddSearchedVoucherDetails";
                var data = { "query": text };
                $.get(url, data, function (data) {
                    debugger;
                    $('#getVoucherDetails').html(data);
                    $('#getVoucherDetails').find('.modal-body').find('.Search').val(text);
                    debugger;

                    $('#getVoucherDetails').modal('show');
                });

                // });
            }
        })
        .fail(function (xhr) {
            alert(xhr.responseText);
        });

};

//End Function

//Start Voucher Type Change Function

$('select#VTypeId').change(function () {
    debugger
    var voucherType = $(this).val();
    var container = $(this).closest('.Voucher1-create').find('.batch-number');
    if (voucherType !== undefined && voucherType !== "") {

        var debitcontainer = $('table#voucher2Table').find('input.dbamnt');
        var creditcontainer = $('table#voucher2Table').find('input.cramnt');

        if (voucherType == 1) {
            $('select#voucherSelect').val(0);
            $('select#voucherSelect').prop("disabled", false);
            $(creditcontainer).hide();
            $(creditcontainer).val('');
            $(debitcontainer).show();

        }
        else if (voucherType == 2) {
            $('select#voucherSelect').val(0);
            $('select#voucherSelect').prop("disabled", true);
            $(creditcontainer).hide();
            $(creditcontainer).val('');
            $(debitcontainer).show();

        }
        else if (voucherType == 3) {
            $('select#voucherSelect').val(1);
            $('select#voucherSelect').prop("disabled", true);
            $(debitcontainer).hide();
            $(debitcontainer).val('');
            $(creditcontainer).show();
        }


        var form = $(container).closest("form");
        $.ajax({
            url: '/Voucher1/GetBatch/',
            data: { type: voucherType },
            dataType: "html",
            async: false,
            success: function (data) {
                debugger
                $(container).show();
                $(container).html(data);
                $(form).removeData("validator");
                $(form).removeData("unobtrusiveValidation");

            }
        });


    }
    else {
        ErrorAlert("Please Select Voucher Type", 5000);
    }


});

//End Voucher Type Change Function


//Start ledger Search Inside Modal Popup-FinAcc Ledger Search

$('.main-voucher-table-container').on('click', 'i#btnledgersearch', function () {

    debugger;
    voucherEvent = $(this);
    var currentEvent = $(this);
    var Search = $('#ledgerSearch').val();
    var url = '/Voucher1/AddVoucherDetails';
    var type = $(this).closest('tr.newrow').find('select#voucherSelect option:selected').val();
    // var data = { "type": type };
    var Query = { "query": Search };



    $.get(url, Query, function (data) {
        debugger;
        $('#getVoucherDetails').html(data);
        $('#getVoucherDetails').find('.modal-body').find('.Search').val(Search);


        $('#getVoucherDetails').modal('show');
    });

});

//End Ledger Search



//Start Temporary Ledger Display Function

$('button#showTempLogs').click(function () {
    debugger;
    var url = $('#getVoucherLogs').data('url');

    $.get(url, function (data) {
        $('#getVoucherLogs').html(data);
        $('#getVoucherLogs').modal('show');
    });

});

//End Temporary Ledger Search


//Start Final Save Ledger function

$('div#create').on('click', 'button#finalsave', function (e) {
    e.stopImmediatePropagation()
    debugger;

    var narration = $('#Narration').val();
    var validateLedger = $('input#ledgerSearch').val();
    var BatchNo = $('#BatchNo').val();
    if (validateLedger == "") {
        ErrorAlert("Please Select Ledger First");
        return false;
    }
    var validateDRAmounts = $('#Voucher2T_0__DebitAmount').val();
    var validateCRAmounts = $('#Voucher2T_0__CreditAmount').val();

    if (validateCRAmounts == "" && validateDRAmounts == "") {
        ErrorAlert('Please Enter Value For Debit Or Credit');
        return false;
    }
    if (narration == "") {
        ErrorAlert("Please Enter Narration", 5000);
        return false;
    }

    var editable = false;
    $('.voucher2-container > tr.newrow').each(function () {
        debugger
        if ($('td').hasClass('editable'))
        {
            ErrorAlert("Please finish the entry", 5000);
            editable = true;
            return false;
        }
    });
    if (editable == true)
    {
        return false;
    }

    //$('.voucher2-container').each('tr.newrow');

    if (BatchNo == "") {
        ErrorAlert("Enter Batch Type", 5000);
        return false;
    }
    else {
        $.MessageBox({
            buttonDone: "Yes",
            buttonFail: "No",
            message: "Are you sure?"
        }).done(function () {
            debugger;
            var finalDebitAmount = 0;
            var finalCreditAmount = 0;

            var allAmounts = $('.voucher2-container').find('tr.newrow');
            //var narration = $('form#mainVoucher').find('textarea#Narration').val();


            $(allAmounts).each(function (index, item) {

                var amountDebit = parseFloat($(item).find('.debit').text()).toFixed(2)

                if (isNaN(amountDebit)) {
                    amountDebit = 0;
                }
                else {
                    finalDebitAmount = (parseFloat(finalDebitAmount) + parseFloat(amountDebit)).toFixed(2)
                }

                var amountCredit = parseFloat($(item).find('.credit').text()).toFixed(2)
                if (isNaN(amountCredit)) {
                    amountCredit = 0;

                }
                else {
                    finalCreditAmount = (parseFloat(finalCreditAmount) + parseFloat(amountCredit)).toFixed(2)
                }

            });
            var vtype = $('#VTypeId').val();
            if (vtype == 1) {
                if (finalDebitAmount != finalCreditAmount) {
                    $.MessageBox({
                        buttonDone: "OK",
                        message: "Error-Please match the Debit and Credit Amounts in Voucher"
                    })
                    //ErrorAlert("Please match the Debit and Credit Amounts in Voucher", 10000);
                    return false;
                }
                else {
                    debugger
                    //GetDeno();
                    var narration = $('#Narration').val();
                    var V1TId = $("#V1TId").val();

                    $.ajax({
                        url: '/TaskVerificationAcc/IsStrictlyVerified/',
                        dataType: "JSON",
                        success: function (isStrictlyVerified) {
                            debugger;
                            if (isStrictlyVerified == "true") {
                                taskVerifierList(30, true, V1TId, narration);
                            }
                            else {
                                taskVerifierList(30, false, V1TId, narration);
                            }
                        }
                    });
                }
            }
            else {
                GetDeno();
            }

        }).fail(function () {
            //$.MessageBox("you clicked no");
        });
    }

});

//End Final Save Ledger 

//Start Temprorary Table Save Function
function getFormObj(form) {
    var formObj = {};
    var inputs = $(form).serializeArray();
    $.each(inputs, function (i, input) {
        formObj[input.name] = input.value;
    });
    return formObj;
}

$('tbody.voucher2-container').on('click', 'i#btnSave', function (e) {
    e.stopImmediatePropagation()
    debugger;
    //var form = $("#createVoucherRow");
    var VoucherTransType = $(this).closest('tr').find('td').find('select#voucherSelect').val();
    var Description = $(this).closest('tr').find('td').find('#particularsSearch').val();
    var validateLedger = $(this).closest('tr.newrow').find('input#ledgerSearch').val();
    var BatchNo = $('#BatchNo').val();

    if (validateLedger == "") {
        ErrorAlert("Please Select Ledger First", 5000);
        return false;
    }
    var validateDRAmounts = $(this).closest('tr.newrow').find('.dbamnt').val();
    var validateCRAmounts = $(this).closest('tr.newrow').find('.cramnt').val();

    if (validateCRAmounts == "" && validateDRAmounts == "") {
        ErrorAlert('Please Enter Value For Debit Or Credit', 5000);
        return false;
    }
    if (BatchNo == undefined || BatchNo == "") {
        if (BatchNo == undefined) {
            if ($('#Batch_Number').attr('readonly') != "readonly") {
                ErrorAlert("Please Select Both Voucher Type and BatchNo", 5000);
                return false;
            }
        }
        if (BatchNo == "") {
            ErrorAlert("Please Select Batch Number", 5000);
            return false;
        }
    }
    if (Description == "") {
        ErrorAlert("Please Enter Description First", 5000);
        return false;
    }

    var saveMaster = 0;
    var currentevent = $(this);
    if ($('select#VTypeId').hasClass('makeDisable') == false) {
        saveMaster = 1;
    }
    var masterId = $('input.voucher-master-Id').val();
    if (masterId != 0 && masterId != null) {
        $("#V1TId").val(masterId);
    }
    $('select#VTypeId').addClass('makeDisable');
    $('select#CTId').addClass('makeDisable');
    $('select#BatchNo').addClass('makeDisable');

    //var container = $('.Voucher1-create').find('.voucher2-container')
    var formData = new FormData($("#mainVoucher")[0]);
    var fid = $(this).closest('tr.newrow').find('#ledgerID-voucher').val();

    //subsi part data retrieval

    var SubsiVSLedgerList = new Array();
    var SId = new Array();
    $("input[id$='SId']").each(function (index) {
        SId.push($(this).val());
    });

    var SIdd = new Array();
    for (var i = 0; i < SId.length / 2; i++) {
        var fullName = SId[i];
        var Cname = new Array();
        var temp = fullName.split(' ');
        for (var i = 0; i < temp.length; i++) {
            Cname.push(temp[i]);
        }
        var data = JSON.stringify(Cname);
        $.ajax({
            url: '/Voucher1/GetCIdFromName/',
            data: { CName: data, fid: fid },
            async: false,
            dataType: "JSON",
            success: function (data) {
                debugger;
                SIdd.push(data);
            }
        });
    }

    var SFId = new Array();
    $("input[id$='SFId']").each(function (index) {
        SFId.push($(this).val());
    });
    var Description = new Array();
    $("input[id$='Description']").each(function (index) {
        Description.push($(this).val());
    });
    var Amount = new Array();
    $("input[id$='Amount']").each(function (index) {
        Amount.push($(this).val());
    });

    formData.append('VoucherTransType', VoucherTransType);

    var count = SId.length;
    var length = count / 2;
    var i;
    for (i = 0; i < SIdd.length; ++i) {
        SubsiVSLedgerList.push({ SId: SIdd[i], SFId: SFId[i], Description: Description[i], Amount: Amount[i] })
    }

    for (var i = 0; i < SubsiVSLedgerList.length; i++) {
        formData.append("SubsiListInVoucher[" + i + "].SId", SubsiVSLedgerList[i].SId);
        formData.append("SubsiListInVoucher[" + i + "].SFId", SubsiVSLedgerList[i].SFId);
        formData.append("SubsiListInVoucher[" + i + "].Description", SubsiVSLedgerList[i].Description);
        formData.append("SubsiListInVoucher[" + i + "].Amount", SubsiVSLedgerList[i].Amount);
    }
    //subsi part data retrieval finish

    //bank part data retrieval start
    var BankInfoList = new Array();

    var slipno = parseInt($(this).closest('tr.newrow').find('.cheque-slip-no').html(), 10);
    var chequeAmount = parseInt($(this).closest('tr.newrow').find('.cheque-amount').html(), 10);
    var chequeDate = parseInt($(this).closest('tr.newrow').find('.cheque-data').html(), 10);
    var chequePayee = $(this).closest('tr.newrow').find('.cheque-payee').html();
    if (chequePayee != null) {
        chequePayee = chequePayee.toString();
    }

    BankInfoList.push({ slipno: slipno, chequeDate: chequeDate, chequeAmount: chequeAmount, chequePayee: chequePayee });

    for (var i = 0; i < BankInfoList.length; i++) {
        formData.append("BankListInVoucher[" + i + "].ChequeNo", BankInfoList[i].slipno);
        formData.append("BankListInVoucher[" + i + "].ChequeDate", BankInfoList[i].chequeDate);
        formData.append("BankListInVoucher[" + i + "].ChequeAmount", BankInfoList[i].chequeAmount);
        formData.append("BankListInVoucher[" + i + "].Payees", BankInfoList[i].chequePayee);
    }

    formData.append('ledgerName', validateLedger);
    formData.append('debitAmount', validateDRAmounts);
    formData.append('creditAmount', validateCRAmounts);
    formData.append('FID2', fid);

    $.ajax({
        url: '/Voucher1/SaveTableTemporary',
        type: 'POST',
        data: formData,
        async: false,
        success: function (data) {
            debugger;
            //var masterId = TempData["Voucher1Id"];
            //$("input#V1Id").val(masterId);
            $(currentevent).closest('tr.newrow').html(data);


            var finalDebitAmount = 0;
            var finalCreditAmount = 0;

            var allAmounts = $('tbody.voucher2-container').find('tr.newrow');

            $(allAmounts).each(function (index, item) {

                var amountDebit = parseFloat($(item).find('.debit').text()).toFixed(2);

                if (isNaN(amountDebit)) {
                    amountDebit = 0;
                }
                else {

                    finalDebitAmount = (parseFloat(finalDebitAmount) + parseFloat(amountDebit)).toFixed(2);
                }
                var amountCredit = parseFloat($(item).find('.credit').text()).toFixed(2);
                if (isNaN(amountCredit)) {
                    amountCredit = 0;

                }
                else {
                    finalCreditAmount = (parseFloat(finalCreditAmount) + parseFloat(amountCredit)).toFixed(2);
                }
            });
            if (isNaN(parseFloat(finalCreditAmount)) == false) {
                $('.main-voucher-table-container').find('td.credit-total-amount').html("<b>" + finalCreditAmount + "<b>");
            }
            if (isNaN(parseFloat(finalDebitAmount)) == false) {
                $('.main-voucher-table-container').find('td.debit-total-amount').html("<b>" + finalDebitAmount + "</b>");
            }

            $('i#btnSaveandNext').hide();
            $('i#btnSave').hide();

            var container = $('.Voucher1-create').find('.voucher2-container');

            var V1Tid = $('#btnSaveandNext').closest('tr.newrow').find('.rowId').val();
            var voucherTypee = $('#VTypeId').val();
            if (voucherTypee != 1) {
                var fid22 = 0;
                $('tr.newrow').each(function () {
                    fid22 = $(this).find('#ledgerID-voucher').val();
                    if (fid22 == 6154 || fid22 == 6155 || fid22 == 6156 || fid22 == 6157) {
                        $(this).remove();
                    }

                });
                $("tr").remove('.newrow-cash');
                $('.Voucher1-create').find('.voucher2-container').append('<tr class="newrow-cash"></tr >');

                var cashContainer = $('.Voucher1-create').find('.voucher2-container').find('.newrow-cash');
                $.ajax({
                    url: '/Voucher1/AddCashLedger/',
                    dataType: "html",
                    async: false,
                    data: {
                        vType: voucherTypee,
                        V1Tid: V1Tid,
                        finalCreditAmount: finalCreditAmount,
                        finalDebitAmount: finalDebitAmount
                    },
                    success: function (data) {
                        debugger
                        $(cashContainer).append(data);
                        $(cashContainer).css("display", "");
                    },
                    error: function (data) {
                        debugger
                    }
                });

                if (voucherTypee == 2) {
                    if (isNaN(parseFloat(finalDebitAmount)) == false) {
                        $('.main-voucher-table-container').find('td.credit-total-amount').html("<b>" + finalDebitAmount + "</b>");
                    }
                }
                else {
                    if (isNaN(parseFloat(finalCreditAmount)) == false) {
                        $('.main-voucher-table-container').find('td.debit-total-amount').html("<b>" + finalCreditAmount + "<b>");
                    }
                }
            }
        },
        error: function (data) {
            debugger;
        },
        cache: false,
        contentType: false,
        processData: false
    });
    return false;
});
//End Temporary Table Save

//Start Temporary Table SaveAndNext Function
$('tbody.voucher2-container').on('click', 'i#btnSaveandNext', function (e) {
    e.stopImmediatePropagation()
    debugger;
    //var form = $("#createVoucherRow");
    var VoucherTransType = $(this).closest('tr').find('td').find('select#voucherSelect').val();
    var Description = $(this).closest('tr').find('td').find('#particularsSearch').val();
    var validateLedger = $(this).closest('tr.newrow').find('input#ledgerSearch').val();
    var BatchNo = $('#BatchNo').val();

    if (validateLedger == "") {
        ErrorAlert("Please Select Ledger First", 5000);
        return false;
    }
    var validateDRAmounts = $(this).closest('tr.newrow').find('.dbamnt').val();
    var validateCRAmounts = $(this).closest('tr.newrow').find('.cramnt').val();

    if (validateCRAmounts == "" && validateDRAmounts == "") {
        ErrorAlert('Please Enter Value For Debit Or Credit', 5000);
        return false;
    }
    if (BatchNo == undefined || BatchNo == "") {
        if (BatchNo == undefined) {
            if ($('#Batch_Number').attr('readonly') != "readonly") {
                ErrorAlert("Please Select Both Voucher Type and BatchNo", 5000);
                return false;
            }
        }
        if (BatchNo == "") {
            ErrorAlert("Please Select Batch Number", 5000);
            return false;
        }
    }
    if (Description == "") {
        ErrorAlert("Please Enter Description First", 5000);
        return false;
    }

    var saveMaster = 0;
    var currentevent = $(this);
    if ($('select#VTypeId').hasClass('makeDisable') == false) {
        saveMaster = 1;
    }
    var masterId = $('input.voucher-master-Id').val();
    if (masterId != 0 && masterId != null) {
        $("#V1TId").val(masterId);
    }
    $('select#VTypeId').addClass('makeDisable');
    $('select#CTId').addClass('makeDisable');
    $('select#BatchNo').addClass('makeDisable');

    //var container = $('.Voucher1-create').find('.voucher2-container')
    var formData = new FormData($("#mainVoucher")[0]);
    var fid = $(this).closest('tr.newrow').find('#ledgerID-voucher').val();

    //subsi part data retrieval

    var SubsiVSLedgerList = new Array();
    var SId = new Array();
    $("input[id$='SId']").each(function (index) {
        SId.push($(this).val());
    });

    var SIdd = new Array();
    for (var i = 0; i < SId.length / 2; i++) {
        var fullName = SId[i];
        var Cname = new Array();
        var temp = fullName.split(' ');
        for (var i = 0; i < temp.length; i++) {
            Cname.push(temp[i]);
        }
        var data = JSON.stringify(Cname);
        $.ajax({
            url: '/Voucher1/GetCIdFromName/',
            data: { CName: data, fid: fid },
            async: false,
            dataType: "JSON",
            success: function (data) {
                debugger;
                SIdd.push(data);
            }
        });
    }

    var SFId = new Array();
    $("input[id$='SFId']").each(function (index) {
        SFId.push($(this).val());
    });
    var Description = new Array();
    $("input[id$='Description']").each(function (index) {
        Description.push($(this).val());
    });
    var Amount = new Array();
    $("input[id$='Amount']").each(function (index) {
        Amount.push($(this).val());
    });

    formData.append('VoucherTransType', VoucherTransType);

    var count = SId.length;
    var length = count / 2;
    var i;
    for (i = 0; i < SIdd.length; ++i) {
        SubsiVSLedgerList.push({ SId: SIdd[i], SFId: SFId[i], Description: Description[i], Amount: Amount[i] })
    }

    for (var i = 0; i < SubsiVSLedgerList.length; i++) {
        formData.append("SubsiListInVoucher[" + i + "].SId", SubsiVSLedgerList[i].SId);
        formData.append("SubsiListInVoucher[" + i + "].SFId", SubsiVSLedgerList[i].SFId);
        formData.append("SubsiListInVoucher[" + i + "].Description", SubsiVSLedgerList[i].Description);
        formData.append("SubsiListInVoucher[" + i + "].Amount", SubsiVSLedgerList[i].Amount);
    }
    //subsi part data retrieval finish

    //bank part data retrieval start
    var BankInfoList = new Array();

    var slipno = parseInt($(this).closest('tr.newrow').find('.cheque-slip-no').html(), 10);
    var chequeAmount = parseInt($(this).closest('tr.newrow').find('.cheque-amount').html(), 10);
    var chequeDate = parseInt($(this).closest('tr.newrow').find('.cheque-data').html(), 10);
    var chequePayee = $(this).closest('tr.newrow').find('.cheque-payee').html();
    if (chequePayee != null) {
        chequePayee = chequePayee.toString();
    }

    BankInfoList.push({ slipno: slipno, chequeDate: chequeDate, chequeAmount: chequeAmount, chequePayee: chequePayee });

    for (var i = 0; i < BankInfoList.length; i++) {
        formData.append("BankListInVoucher[" + i + "].ChequeNo", BankInfoList[i].slipno);
        formData.append("BankListInVoucher[" + i + "].ChequeDate", BankInfoList[i].chequeDate);
        formData.append("BankListInVoucher[" + i + "].ChequeAmount", BankInfoList[i].chequeAmount);
        formData.append("BankListInVoucher[" + i + "].Payees", BankInfoList[i].chequePayee);
    }

    formData.append('ledgerName', validateLedger);
    formData.append('debitAmount', validateDRAmounts);
    formData.append('creditAmount', validateCRAmounts);
    formData.append('FID2', fid);
    //console.log(formData);
    $.ajax({
        url: '/Voucher1/SaveTableTemporary',
        type: 'POST',
        data: formData,
        processData: false,
        cache: false,
        async: false,
        success: function (data) {
            debugger;
            //var masterId = TempData["Voucher1Id"];
            //$("input#V1Id").val(masterId);
            $(currentevent).closest('tr.newrow').html(data);


            var finalDebitAmount = 0;
            var finalCreditAmount = 0;

            var allAmounts = $('tbody.voucher2-container').find('tr.newrow');

            $(allAmounts).each(function (index, item) {

                var amountDebit = parseFloat($(item).find('.debit').text()).toFixed(2);

                if (isNaN(amountDebit)) {
                    amountDebit = 0;
                }
                else {

                    finalDebitAmount = (parseFloat(finalDebitAmount) + parseFloat(amountDebit)).toFixed(2);
                }
                var amountCredit = parseFloat($(item).find('.credit').text()).toFixed(2);
                if (isNaN(amountCredit)) {
                    amountCredit = 0;

                }
                else {
                    finalCreditAmount = (parseFloat(finalCreditAmount) + parseFloat(amountCredit)).toFixed(2);
                }
            });
            if (isNaN(parseFloat(finalCreditAmount)) == false) {
                $('.main-voucher-table-container').find('td.credit-total-amount').html("<b>" + finalCreditAmount + "<b>");
            }
            if (isNaN(parseFloat(finalDebitAmount)) == false) {
                $('.main-voucher-table-container').find('td.debit-total-amount').html("<b>" + finalDebitAmount + "</b>");
            }

            $('i#btnSaveandNext').hide();
            $('i#btnSave').hide();

            var container = $('.Voucher1-create').find('.voucher2-container');

            var V1Tid = $('#btnSaveandNext').closest('tr.newrow').find('.rowId').val();
            var voucherTypee = $('select#VTypeId').val();
            if (voucherTypee != 1) {
                $("tr").remove('.newrow-cash');
                $('.Voucher1-create').find('.voucher2-container').append('<tr class="newrow-cash"></tr >');

                var cashContainer = $('.Voucher1-create').find('.voucher2-container').find('.newrow-cash');
                $.ajax({
                    url: '/Voucher1/AddCashLedger/',
                    dataType: "html",
                    async: false,
                    data: {
                        vType: voucherTypee,
                        V1Tid: V1Tid,
                        finalCreditAmount: finalCreditAmount,
                        finalDebitAmount: finalDebitAmount
                    },
                    success: function (data) {
                        debugger
                        $(cashContainer).append(data);
                        $(cashContainer).css("display", "");
                    },
                    error: function (data) {
                        debugger
                    }
                });

                if (voucherTypee == 2) {
                    if (isNaN(parseFloat(finalDebitAmount)) == false) {
                        $('.main-voucher-table-container').find('td.credit-total-amount').html("<b>" + finalDebitAmount + "</b>");
                    }
                }
                else {
                    if (isNaN(parseFloat(finalCreditAmount)) == false) {
                        $('.main-voucher-table-container').find('td.debit-total-amount').html("<b>" + finalCreditAmount + "<b>");
                    }
                }
            }

            var form = $(container).closest("form");
            $.ajax({
                url: '/Voucher1/AddTableRow/',
                dataType: "html",
                success: function (data) {
                    debugger
                    $(container).show();
                    if (data.includes('<ul') || data.includes('<li')) {
                        var newdata = data.replace('<ul>', '').replace('<li>', '').replace('</ul>', '').replace('</li>', '');
                        newdata = newdata.slice(160);
                        $(container).append(newdata);
                    }
                    else {
                        $(container).append(data);
                    }

                    var voucherType = $('select#VTypeId').val();
                    var debitcontainer = $('table#voucher2Table').find('input.dbamnt');
                    var creditcontainer = $('table#voucher2Table').find('input.cramnt');

                    if (voucherType == 1) {
                        $('select#voucherSelect').val(0);
                        $('select#voucherSelect').prop("disabled", false);
                        $(creditcontainer).hide();
                        $(creditcontainer).val('');
                        $(debitcontainer).show();

                    }
                    else if (voucherType == 2) {
                        $('select#voucherSelect').val(0);
                        $('select#voucherSelect').prop("disabled", true);
                        $(creditcontainer).hide();
                        $(creditcontainer).val('');
                        $(debitcontainer).show();

                    }
                    else if (voucherType == 3) {
                        $('select#voucherSelect').val(1);
                        $('select#voucherSelect').prop("disabled", true);
                        $(debitcontainer).hide();
                        $(debitcontainer).val('');
                        $(creditcontainer).show();
                    }
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                }
            });
        },
        error: function (data) {
            debugger
        },
        cache: false,
        contentType: false,
        processData: false
    });


    return false;
});
//End Temporary Table SaveAndNext


//deno
function GetDeno() {
    debugger;
    var total = parseInt($('.main-voucher-table-container table tfoot').find('.debit-total-amount b').html(), 10);
    //var total = parseInt($('.main-voucher-table-container table tfoot').find('.total-tr .debit-total-amount b').html(), 10);
    var V1TId = $("#V1TId").val();

    $.ajax({
        type: 'GET',
        url: "/Voucher1/DenoTransaction",
        data: { amount: total, V1TId: V1TId },
        success: function (result) {
            debugger;
            $('#getDenoOuter').html(result);
            $('#getDenoOuter').modal('show'); /*modal('hide');*/
        },
        error: function () {

        }
    });
}

$('div#getDenoOuter').on('click', '.denoConfirm', function (e) {
    e.stopImmediatePropagation();
    debugger;
    var narration = $('#Narration').val();
    var V1TId = $("#V1TId").val();

    $('#getDenoOuter').modal('hide');

    $('#denoTransaction').ajaxSubmit({

        beforeSubmit: function () {
        },
        success: function (result) {
            if (result == true) {
                $.ajax({
                    url: '/TaskVerificationAcc/IsStrictlyVerified/',
                    dataType: "JSON",
                    success: function (isStrictlyVerified) {
                        debugger;
                        if (isStrictlyVerified == "true") {
                            taskVerifierList(30, true, V1TId, narration);
                        }
                        else {
                            taskVerifierList(30, false, V1TId, narration);
                        }
                    }
                });
            }
            else {
                ErrorAlert("Deno doesnt match", 5000);
                document.getElementById('alert-error').scrollIntoView();
            }
        },
        error: function () {
            ErrorAlert("Error in Updating Error", 15000);
        }
    });
});
//demo end
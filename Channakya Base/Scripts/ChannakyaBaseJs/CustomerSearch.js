var _globalObject;

$(document).on("click", "#btn-customer-search", function (e) {
    debugger;
    e.stopImmediatePropagation();
    debugger
    var searchValue = $("#SearchParameter").val();
    var selectedOption = $("#SearchOption option:selected").val();
    var listBox = $(this).parents().find(".listBox").find(".multiselect").val();
    var mode = $("#Mode").val();
    var custType = $("#CustomerType").val();
    $('.img-container').hide();
    $('.view-history-image').hide();
    $.ajax({
        type: 'GET',
        url: '/Customer/_CustomerInfoList',
        data: {
            searchParam: searchValue,
            searchOption: selectedOption,
            listBox: listBox,
            mode: mode,
            custType: custType
        },
        traditional: true,
        success: function (result) {
            $("#customerDetails").html("");
            $("#customerDetails").html(result);
        },
    });

});

$(document).on('click', '.table-click-customer table tr', function (e) {

    e.stopImmediatePropagation();
    var closestTr = $(this).closest('tr');
    var objCheck = $(closestTr).find('.Ischeck');
    var isChecked = $(closestTr).find('.Ischeck').prop("checked");
    var listBox = $(this).parents().find(".listBox").find(".multiselect")
    var mode = $(this).parents().find("#Mode").val();
    var custType = $(this).parents().find("#CustomerType").val();
    $('.img-container').hide();
    $('.view-history-image').hide();
    var me = $(this);
    if (isChecked == true) {
        //  objCheck.prop('checked', false);
        //  $(closestTr).removeAttr('style');
        //  var customerName = $(closestTr).find('.Customername').text();

        //  var chosenValueName = $('select.multiselect option:contains("' + customerName + '")').remove()
        //var a=  listBox.chosen().trigger("chosen:updated");
        return;
    } else {

        var customerId = objCheck.val();
        var listBoxIDs = listBox.val();
        $.ajax({
            type: 'GET',
            url: '/Customer/GetSelectedCustomer',
            data: {
                customerId: customerId,
                listBox: listBoxIDs,
                mode: mode,
                custType: custType
            },
            traditional: true,
            success: function (result) {

                if (result.Isselect == true) {
                    if (mode == 'Single' || mode == 'CompanyOnly' || mode == 'CustomerOnly' || mode == "All") {
                        var id = _globalObject;

                        var multiSelectCustomer = $(me).parents().find(".multiselectCustomer#" + id)
                        $('select.multiselectCustomer#' + id + ' option:contains("' + multiSelectCustomer.text() + '")').remove();
                        $(".multiselectCustomer#" + id).trigger("chosen:updated");
                        $(multiSelectCustomer).append('<option value="' + result.CID + '" selected>' + result.Name + '</option>');
                        $(multiSelectCustomer).trigger("chosen:updated");
                        $($(me).parents().find("#pop-up-div")).modal('toggle');
                        if (mode == "All") {
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

                                //var id= link.parents().find('#Aname').val();
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
                    }
                        //else if (mode == 'VerifiedRegisteredLoanList') {
                        //    debugger;


                        //    var customerName = $('#CustomerId').find(':selected').text();
                        //        $.ajax({
                        //            type: 'GET',
                        //            url: '/Credit/_VerifiedRegisteredLoanAccounts',
                        //            data: {
                        //                customerName: customerName,

                        //            },
                        //            success: function (result) {
                        //                debugger;
                        //                if (result.Success) {
                        //                    $.ajax({
                        //                        url: "/Credit/VerifiedRegisteredLoanAccountsIndex",
                        //                        type: 'GET',
                        //                        async: false,
                        //                        success: function (result) {
                        //                            debugger;
                        //                            $('section.content').html(result);
                        //                        }
                        //                    })
                        //                    //$('#registered-loan-accountnumber-list').html(result);
                        //                }

                        //            else{

                        //        }
                        //            },

                        //        });


                        //    objCheck.prop('checked', true);
                        //    $(closestTr).css('background-color', '#c2c4c3');
                        //    $(listBox).append('<option value="' + result.CID + '" selected>' + result.Name + '</option>');
                        //    $(listBox).trigger("chosen:updated");

                        //}
                    else {
                        objCheck.prop('checked', true);
                        $(closestTr).css('background-color', '#c2c4c3');
                        $(listBox).append('<option value="' + result.CID + '" selected>' + result.Name + '</option>');
                        $(listBox).trigger("chosen:updated");

                    }

                } else {

                    ErrorAlert("Join account not allowed for this type of customer!!", 5000)
                }

            },
        });
    }

});
$(document).on("click", ".addandClose", function (e) {
    debugger;
    e.stopImmediatePropagation();
    debugger;
    var Parent = $(this).parents();
    var id = _globalObject;

    var multiSelectCustomer = $(Parent).find(".multiselectCustomer#" + id);
    var customerSelectList = $(Parent).find(".listBox").find(".multiselect").val();
    var EventId = $("#MessageEventID").val();
    // var evid = $(Parent).find("#MessageEventID").val();
    var mode = $(Parent).parents().find("#Mode").val()
    var AccountName = "";
    $.ajax({
        type: 'GET',
        url: '/Customer/GetMultipleSelectedCustomer',
        data: {
            listBox: customerSelectList
        },
        traditional: true,
        success: function (result) {

            $($(Parent).find(".multiselectCustomer#" + id + " option:selected")).each(function () {
                $("select.multiselectCustomer#" + id + " option:contains('" + $(this).text() + "')").remove();
                $(".multiselectCustomer#" + id).trigger("chosen:updated");
            });
            $(result).each(function (index, element) {
                $(multiSelectCustomer).append('<option value="' + element.CID + '" selected>' + element.Name + '</option>');
                $(multiSelectCustomer).trigger("chosen:updated");
                if (mode == 'AccountOpen' || mode == 'VerifiedRegisteredLoanList') {
                    if (AccountName != '') {
                        AccountName = AccountName + ',' + element.Name;
                    } else {
                        AccountName = element.Name;
                    }

                }
            });
            if (mode == 'AccountOpen') {
                $(Parent).find("#Aname").val("");
                $(Parent).find("#Aname").val(AccountName);
            }

            if (mode == 'VerifiedRegisteredLoanList') {
                debugger;
                $(Parent).find("#Aname").val("");
                $(Parent).find("#Aname").val(AccountName);
               
                   var customerName=AccountName;
                $.ajax({
                    type: 'get',
                    url: '/Credit/_VerifiedRegisteredLoanAccounts',
                    data: {
                        customerName: customerName,

                    },
                    success: function (result) {
                        debugger;
                   

                        if (result == null) {
                            $('#verified-registered-loan').text("No Item to Display");
                        }
                        else {
                            $('#verified-registered-loan').html(result);

                        }
                          
                      

                    
                    },

                });
            }

        },
    });
    
    $.ajax({
        url: "/Customer/GetEventListByCustomerIds",
        type: 'GET',
        data: {
            customerid: customerSelectList,
            EventId: EventId
        },
        traditional: true,
        success: function (result) {
            debugger;
            $("#customer-related-message").html(result);
        }
    })
    //}



});


//$(document).ready(function () {

$(".btncustomersearch").on('click', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var Parent = $(this).parents();
    var mode = $(this).attr("mode");
    var CustomerType = $(this).attr("customerType")
    var multiSelectCustomer = $(Parent).find(".multiselectCustomer");
    _globalObject = $(this).closest("div.CommonSearchDiv").find(".multiselectCustomer").attr("id");

    var multiSelectCustomerList = $(multiSelectCustomer).val();
    $('.img-container').hide();
    $('.view-history-image').hide();
    $.ajax({
        type: 'GET',
        url: "/Customer/CustomerInfoList",
        data: {
            listBox: multiSelectCustomerList,
            mode: mode,
            custType: CustomerType
        },
        traditional: true,
        success: function (result) {

            $('#pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });

        },
        error: function () {

        }
    });



});
//});



var _globalObject;

$(document).on("click", "#btn-customer-search", function (e) {
    e.stopImmediatePropagation();
   
       
        var searchValue = $("#SearchParameter").val();
        var selectedOption = $("#SearchOption option:selected").val()
        var listBox = $(this).parents().find(".listBox").find(".multiselect").val()
        var mode = $("#Mode").val()
        $.ajax({
            type: 'GET',
            url: '/Customer/_CustomerInfoList',
            data: {
                searchParam: searchValue,
                searchOption: selectedOption,
                listBox: listBox,
                mode: mode
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
    var me = $(this);
    if (isChecked == true) {
        objCheck.prop('checked', false);
        $(closestTr).removeAttr('style');
        var customerName = $(closestTr).find('.Customername').text();
        var chosenValueName = $('select.multiselect option:contains("' + customerName + '")').remove()
        listBox.chosen().trigger("chosen:updated");
    } else {

        var customerId = objCheck.val();
        var listBoxIDs = listBox.val();
        $.ajax({
            type: 'GET',
            url: '/Customer/GetSelectedCustomer',
            data: {
                customerId: customerId,
                listBox: listBoxIDs,
                mode: mode
            },
            traditional: true,
            success: function (result) {
                debugger;
                if (result.Isselect == true) {
                    if (mode == 'Single' || mode == 'CompanyOnly' || mode == 'CustomerOnly'||mode=="All") {
                        var id = _globalObject;

                        var multiSelectCustomer = $(me).parents().find(".multiselectCustomer#" + id)
                        $('select.multiselectCustomer#' + id + ' option:contains("' + multiSelectCustomer.text() + '")').remove();
                        $(".multiselectCustomer#" + id).trigger("chosen:updated");
                        $(multiSelectCustomer).append('<option value="' + result.CID + '" selected>' + result.Name + '</option>');
                        $(multiSelectCustomer).trigger("chosen:updated");
                        $($(me).parents().find("#pop-up-div")).modal('toggle');

                    } else {
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
    e.stopImmediatePropagation();
    var Parent = $(this).parents();
    var id = _globalObject;
    var multiSelectCustomer = $(Parent).find(".multiselectCustomer#" + id);
    var customerSelectList = $(Parent).find(".listBox").find(".multiselect").val();
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
                if (mode == 'AccountOpen') {
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
        },
    });
});


$(document).ready(function () {

    $(".btncustomersearch").on('click', function (e) {
        e.stopImmediatePropagation();
        var Parent = $(this).parents();
        var mode = $(this).attr("mode");
        var multiSelectCustomer = $(Parent).find(".multiselectCustomer");
        _globalObject = $(this).closest("div.CommonSearchDiv").find(".multiselectCustomer").attr("id");
        
        var multiSelectCustomerList = $(multiSelectCustomer).val();
        $.ajax({
            type: 'GET',
            url: "/Customer/CustomerInfoList",
            data: {
                listBox: multiSelectCustomerList,
                mode: mode
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
});


